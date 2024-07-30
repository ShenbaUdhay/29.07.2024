using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Notifications;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Export;
using DevExpress.Web;
using ICM.Module.BusinessObjects;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web;

namespace LDM.Module.Controllers.ICM
{
    public partial class ItemViewController : ViewController, IXafCallbackHandler
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        ShowNavigationItemController ShowNavigationController;
        //ModificationsController mdcSave;
        //ModificationsController mdcSaveClose;
        //ModificationsController mdcSaveNew;
        ICMinfo objInfo = new ICMinfo();
        string strItemName = string.Empty;
        string strVendorName = string.Empty;
        string strSpecification = string.Empty;
        string strCategory = string.Empty;
        string strVendorCatName = string.Empty;
        string strAmount = string.Empty;
        string strAmountUnits = string.Empty;
        string strcatalog = string.Empty;
        string strgrade = string.Empty;
        string strStockQty = string.Empty;
        string strPackUnits = string.Empty;
        int intstockqty = 0;
        double doubleunitprice = 0;
        double doubleAmount = 0;
        int intitemunit = 0;
        int intdayremain = 0;
        int intalert = 0;
        string strdepartment = string.Empty;
        string strcomment = string.Empty;
        string strmanufacture = string.Empty;
        DataTable dt;

        requisitionquerypanelinfo objreq = new requisitionquerypanelinfo();
        //IValueManager<IList<Packageunits>> packageunits;
        itemsvaluemanager packageunits = new itemsvaluemanager();
        spreadsheetitemsltno itemsltno = new spreadsheetitemsltno();
        //ItemRequistionListInfo ItemRequistionListInfo = new ItemRequistionListInfo();

        //IList<Packageunits> listPackageunits = null;
        #endregion

        #region Constructor
        public ItemViewController()
        {
            InitializeComponent();
            this.TargetViewId = "Requisition_ListView;" + "Requisition_DetailView;" + "Supplies_LookupListView;" + "Requisition_ListView_Receive;" + "Requisition_DetailView_Receive;" + "Requisition_ListView;" + "Requisition_LookupListView;" + "Items_DetailView;" + "Vendors_DetailView;" + "Items_ListView;" + "Items_ListView_Copy;";
            Reorder.TargetViewId = "Requisition_ListView";
            RequisitionDate.TargetViewId = "Requisition_ListView";
            Itemsave.TargetViewId = "Items_ListView_Copy;";
            exportitemsstock.TargetViewId = "Items_ListView";
            //exportitemsstock.Category = "RecordEdit";
            //exportitemsstock.Model.Index = 4;
            ImportFromFileAction.TargetViewId = "Items_ListView;";
            //ImportFromFileAction.Category = "RecordEdit";
            //ImportFromFileAction.Model.Index = 5;
            removeItemsAction.TargetViewId = "Items_ListView_Copy";
            //removeItemsAction.ImageName = "Action_Delete";
        }
        #endregion

        #region Default Methods
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                Reorder.Active["ShowActive"] = false;
                //mdcSave = Frame.GetController<ModificationsController>();
                //mdcSave.SaveAction.Execute += SaveAction_Execute;
                //mdcSaveClose = Frame.GetController<ModificationsController>();
                //mdcSaveClose.SaveAndCloseAction.Execute += SaveAndCloseAction_Execute;
                //mdcSaveClose.SaveAndCloseAction.Executed += SaveAndCloseAction_Executed;
                //mdcSaveNew = Frame.GetController<ModificationsController>();
                //mdcSaveNew.SaveAndNewAction.Execute += SaveAndNewAction_Execute;
                //mdcSaveNew.SaveAndNewAction.Executing += SaveAndNewAction_Executing;
                ObjectSpace.Committing += ObjectSpace_Committing;
                if (View.Id == "Items_ListView_Copy")
                {
                    ObjectSpace.Committing += ObjectSpace_Committing;
                }
                ImportFromFileAction.ExecuteCompleted += ImportFromFileAction_ExecuteCompleted;
                ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                //Permisson code
                Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                if (currentUser != null && View != null && View.Id != null)
                {

                    if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                    {
                        objPermissionInfo.RequisitionIsWrite = false;
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            objPermissionInfo.RequisitionIsWrite = true;
                            objPermissionInfo.ICMImportFile = true;
                        }
                        else
                        {
                            foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Requisition" && i.Write == true) != null)
                                {
                                    objPermissionInfo.RequisitionIsWrite = true;
                                    //return;
                                }

                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Items" && i.Write == true) != null)
                                {
                                    objPermissionInfo.ICMImportFile = true;
                                    //return;
                                }
                            }
                        }
                    }
                }
                Reorder.Active.SetItemValue("ShowReorder", objPermissionInfo.RequisitionIsWrite);
                ImportFromFileAction.Active.SetItemValue("ShowImpoort", objPermissionInfo.ICMImportFile);
                //Employee currentUser = SecuritySystem.CurrentUser as Employee;
                //if (currentUser != null && View != null && View.Id != null)
                //{
                //    IObjectSpace objSpace = Application.CreateObjectSpace();
                //    CriteriaOperator criteria = CriteriaOperator.Parse("[User]='" + currentUser.Oid + "'");
                //    UserNavigationPermission userpermission = objSpace.FindObject<UserNavigationPermission>(criteria);
                //    CriteriaOperator navcriteria = CriteriaOperator.Parse("[NavigationView]='" + View.Id + "'");
                //    Modules.BusinessObjects.Setting.NavigationItem navigation = objSpace.FindObject<Modules.BusinessObjects.Setting.NavigationItem>(navcriteria);
                //    if (navigation != null && userpermission != null)
                //    {
                //        CriteriaOperator navpermissioncriteria = CriteriaOperator.Parse("[NavigationItem]='" + navigation.Oid + "' and [UserNavigationPermission]='" + userpermission.Oid + "'");
                //        UserNavigationPermissionDetails navPermissionDetails = objSpace.FindObject<UserNavigationPermissionDetails>(navpermissioncriteria);
                //        if (navPermissionDetails != null && View.Id == "Requisition_ListView")
                //        {
                //            if (navPermissionDetails.Write == true)
                //            {
                //                Reorder.Active.SetItemValue("ItemViewController.Reorder", true);
                //            }
                //            else if (navPermissionDetails.Write == false)
                //            {
                //                Reorder.Active.SetItemValue("temViewController.Reorder", false);
                //            }
                //        }
                //    }
                //}
                if (View != null && View.Id == "Requisition_ListView")
                {
                    if (RequisitionDate.Items.Count == 0)
                    {
                        var item1 = new ChoiceActionItem();
                        var item2 = new ChoiceActionItem();
                        var item3 = new ChoiceActionItem();
                        var item4 = new ChoiceActionItem();
                        var item5 = new ChoiceActionItem();
                        var item6 = new ChoiceActionItem();
                        var item7 = new ChoiceActionItem();
                        var item8 = new ChoiceActionItem();

                        //RequisitionDate.Items.Add(new ChoiceActionItem("1W", item1));
                        RequisitionDate.Items.Add(new ChoiceActionItem("1M", item1));
                        RequisitionDate.Items.Add(new ChoiceActionItem("3M", item2));
                        RequisitionDate.Items.Add(new ChoiceActionItem("6M", item3));
                        RequisitionDate.Items.Add(new ChoiceActionItem("1Y", item4));
                        RequisitionDate.Items.Add(new ChoiceActionItem("2Y", item5));
                        RequisitionDate.Items.Add(new ChoiceActionItem("5Y", item6));
                        RequisitionDate.Items.Add(new ChoiceActionItem("ALL", item7));
                    }
                    //RequisitionDate.SelectedIndex = 0;
                    //((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 1");
                    //((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffDay(RequestedDate, Now()) < 1");
                    //((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 3");
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (setting.InventoryWorkFlow == EnumDateFilter.OneMonth)
                    {
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 1");
                        RequisitionDate.SelectedIndex = 0;
                    }
                    else if (setting.InventoryWorkFlow == EnumDateFilter.ThreeMonth)
                    {
                        RequisitionDate.SelectedIndex = 1;
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 3");
                    }
                    else if (setting.InventoryWorkFlow == EnumDateFilter.SixMonth)
                    {
                        RequisitionDate.SelectedIndex = 2;
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 6");
                    }
                    else if (setting.InventoryWorkFlow == EnumDateFilter.OneYear)
                    {
                        RequisitionDate.SelectedIndex = 3;
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 1");
                    }
                    else if (setting.InventoryWorkFlow == EnumDateFilter.TwoYear)
                    {
                        RequisitionDate.SelectedIndex = 4;
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 2");
                    }
                    else if (setting.InventoryWorkFlow == EnumDateFilter.FiveYear)
                    {
                        RequisitionDate.SelectedIndex = 5;
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 5");
                    }
                    else if (setting.InventoryWorkFlow == EnumDateFilter.All)
                    {
                        RequisitionDate.SelectedIndex = 6;
                        ((ListView)View).CollectionSource.Criteria.Clear();
                    }
                    //RequisitionDate.SelectedIndex = 0;

                }
                if(View!=null&&View.Id== "Items_DetailView")
                {
                    Items objIT = View.CurrentObject as Items;
                    if (objIT!=null)
                    {
                        if (objIT.Amount != null)
                        {
                            objIT.NpAmount = objIT.Amount; 
                        }
                        if (objIT.AmountUnit!=null)
                        {
                            objIT.NPAmountUnit = objIT.AmountUnit;  
                        }
                    }
                }
                //if (View.Id== "Items_ListView_Copy")
                //{
                //    ASPxGridListEditor listEditor = (ASPxGridListEditor)((ListView)View).Editor;
                //    listEditor.CreateCustomEditItemTemplate += itemlistEditor_CreateCustomEditItemTemplate;
                //    listEditor.CreateCustomGridViewDataColumn += itemlistEditor_CreateCustomGridViewDataColumn; 
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ImportFromFileAction_ExecuteCompleted(object sender, ActionBaseEventArgs e)
        {
            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages", "ItemsImported"), InformationType.Success, timer.Seconds, InformationPosition.Top);
        }

        //private void itemlistEditor_CreateCustomGridViewDataColumn(object sender, CreateCustomGridViewDataColumnEventArgs e)
        //{
        //    if (e.ModelColumn.PropertyEditorType == typeof(ASPxLookupPropertyEditor))
        //    {
        //        if (e.ModelColumn.PropertyName == "Category")
        //        {
        //            var gridColumn = new GridViewDataComboBoxColumn();
        //            gridColumn.Name = e.ModelColumn.PropertyName;
        //            gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
        //            gridColumn.PropertiesComboBox.ValueType = typeof(int?);
        //            gridColumn.PropertiesComboBox.ValueField = "Oid";
        //            gridColumn.PropertiesComboBox.TextField = "Name";
        //            gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Items>();
        //            e.Column = gridColumn;
        //        }
        //        if (e.ModelColumn.PropertyName == "Grade")
        //        {
        //            var gridColumn = new GridViewDataComboBoxColumn();
        //            gridColumn.Name = e.ModelColumn.PropertyName;
        //            gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
        //            gridColumn.PropertiesComboBox.ValueType = typeof(int?);
        //            gridColumn.PropertiesComboBox.ValueField = "Oid";
        //            gridColumn.PropertiesComboBox.TextField = "Name";
        //            gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Items>();
        //            e.Column = gridColumn;
        //        }
        //        if (e.ModelColumn.PropertyName == "Unit")
        //        {
        //            var gridColumn = new GridViewDataComboBoxColumn();
        //            gridColumn.Name = e.ModelColumn.PropertyName;
        //            gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
        //            gridColumn.PropertiesComboBox.ValueType = typeof(int?);
        //            gridColumn.PropertiesComboBox.ValueField = "Oid";
        //            gridColumn.PropertiesComboBox.TextField = "Name";
        //            gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Items>();
        //            e.Column = gridColumn;
        //        }
        //        if (e.ModelColumn.PropertyName == "Manufacturer")
        //        {
        //            var gridColumn = new GridViewDataComboBoxColumn();
        //            gridColumn.Name = e.ModelColumn.PropertyName;
        //            gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
        //            gridColumn.PropertiesComboBox.ValueType = typeof(int?);
        //            gridColumn.PropertiesComboBox.ValueField = "Oid";
        //            gridColumn.PropertiesComboBox.TextField = "Name";
        //            gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Items>();
        //            e.Column = gridColumn;
        //        }
        //        if (e.ModelColumn.PropertyName == "Vendor")
        //        {
        //            var gridColumn = new GridViewDataComboBoxColumn();
        //            gridColumn.Name = e.ModelColumn.PropertyName;
        //            gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
        //            gridColumn.PropertiesComboBox.ValueType = typeof(int?);
        //            gridColumn.PropertiesComboBox.ValueField = "Oid";
        //            gridColumn.PropertiesComboBox.TextField = "Name";
        //            gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Items>();
        //            e.Column = gridColumn;
        //        }
        //        if (e.ModelColumn.PropertyName == "AmountUnit")
        //        {
        //            var gridColumn = new GridViewDataComboBoxColumn();
        //            gridColumn.Name = e.ModelColumn.PropertyName;
        //            gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
        //            gridColumn.PropertiesComboBox.ValueType = typeof(int?);
        //            gridColumn.PropertiesComboBox.ValueField = "Oid";
        //            gridColumn.PropertiesComboBox.TextField = "Name";
        //            gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Items>();
        //            e.Column = gridColumn;
        //        }

        //    }
        //}

        //private void itemlistEditor_CreateCustomEditItemTemplate(object sender, CreateCustomEditItemTemplateEventArgs e)
        //{
        //    if (e.ModelColumn.PropertyName == "LookupReferencedObject")
        //    {
        //        IEnumerable<Items> referencedObjectsList =
        //            ObjectSpace.CreateCollection(typeof(Items), null,
        //            new SortProperty[] { new SortProperty("Name",
        //        DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Items>();
        //        e.Template = new ReferencedTemplate(referencedObjectsList);
        //        e.Handled = true;
        //    }
        //}

        private void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View.Id == "Items_DetailView")
                {
                    Items objreq = (Items)View.CurrentObject;
                    if (objreq.ItemCode == null)
                    {
                        IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(Items));
                        CriteriaOperator ItemCode = CriteriaOperator.Parse("Max(ItemCode)");
                        string NewItemCode = (Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Items), ItemCode, null)) + 1).ToString();
                        if (NewItemCode.Length == 1)
                        {
                            NewItemCode = "00" + NewItemCode;
                        }
                        else if (NewItemCode.Length == 2)
                        {
                            NewItemCode = "0" + NewItemCode;
                        }
                        objreq.ItemCode = NewItemCode;
                        //ObjectSpace.CommitChanges();
                    }
                    if (objreq!=null)
                    {
                        if (!objreq.IsFractional)
                        {
                            objreq.Amount = objreq.NpAmount;
                            objreq.AmountUnit = objreq.NPAmountUnit;  
                        }
                    }
                }
                else if (View.Id == "Vendors_DetailView")
                {
                    Vendors objreq = (Vendors)View.CurrentObject;
                    if (objreq.Vendorcode == null)
                    {
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
                        objreq.Vendorcode = NewVendorCode;
                        //ObjectSpace.CommitChanges();
                    }
                }
                else if (View.Id == "Requisition_DetailView")
                {
                    Requisition objreq = (Requisition)View.SelectedObjects;
                    if (objreq.RQID == null)
                    {
                        IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(Distribution));
                        CriteriaOperator sam = CriteriaOperator.Parse("Max(SUBSTRING(RQID, 2))");
                        string temprc = (Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Requisition), sam, null)) + 1).ToString();
                        var curdate = DateTime.Now.ToString("yyMMdd");
                        if (temprc != "1")
                        {
                            var predate = temprc.Substring(0, 6);
                            if (predate == curdate)
                            {
                                temprc = "RQ" + temprc;
                            }
                            else
                            {
                                temprc = "RQ" + curdate + "01";
                            }
                        }
                        else
                        {
                            temprc = "RQ" + curdate + "01";
                        }
                        objreq.RQID = temprc;
                        //ObjectSpace.CommitChanges();
                    }
                    ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                    foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                    {
                        if (parent.Id == "InventoryManagement")
                        {
                            foreach (ChoiceActionItem child in parent.Items)
                            {
                                if (child.Id == "Operations")
                                {
                                    foreach (ChoiceActionItem subchild in child.Items)
                                    {
                                        if (subchild.Id == "Review")
                                        {
                                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingReview'"));
                                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                            if (count > 0)
                                            {
                                                subchild.Caption = cap[0] + " (" + count + ")";
                                            }
                                            else
                                            {
                                                subchild.Caption = cap[0];
                                            }
                                        }
                                    }
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

        //private void SaveAndCloseAction_Execute(object sender, ActionBaseEventArgs e)
        //{
        //    try
        //    {
        //        if (View.Id == "Items_DetailView" || View.Id == "Items_ListView")
        //        {
        //            Items objreq = (Items)View.CurrentObject;
        //            if (objreq.ItemCode == null)
        //            {
        //                IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(Items));
        //                CriteriaOperator ItemCode = CriteriaOperator.Parse("Max(ItemCode)");
        //                string NewItemCode = (Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Items), ItemCode, null)) + 1).ToString();
        //                if (NewItemCode.Length == 1)
        //                {
        //                    NewItemCode = "00" + NewItemCode;
        //                }
        //                else if (NewItemCode.Length == 2)
        //                {
        //                    NewItemCode = "0" + NewItemCode;
        //                }
        //                objreq.ItemCode = NewItemCode;
        //                ObjectSpace.CommitChanges();
        //            }
        //        }
        //        else if (View.Id == "Vendors_DetailView" || View.Id == "Vendors_ListView")
        //        {
        //            Vendors objreq = (Vendors)((NestedFrame)Frame).ViewItem.View.CurrentObject;
        //            if (objreq.Vendorcode == null)
        //            {
        //                IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(Vendors));
        //                CriteriaOperator VendorCode = CriteriaOperator.Parse("Max(Vendorcode)");
        //                string NewVendorCode = (Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Vendors), VendorCode, null)) + 1).ToString();
        //                if (NewVendorCode.Length == 1)
        //                {
        //                    NewVendorCode = "00" + NewVendorCode;
        //                }
        //                else if (NewVendorCode.Length == 2)
        //                {
        //                    NewVendorCode = "0" + NewVendorCode;
        //                }
        //                objreq.Vendorcode = NewVendorCode;
        //                ObjectSpace.CommitChanges();
        //            }
        //        }

        //        else if (View.Id == "Requisition_DetailView")
        //        {
        //            Requisition objreq = (Requisition)View.SelectedObjects;
        //            if (objreq.RQID == null)
        //            {
        //                IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(Distribution));
        //                CriteriaOperator sam = CriteriaOperator.Parse("Max(SUBSTRING(RQID, 2))");
        //                string temprc = (Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Requisition), sam, null)) + 1).ToString();
        //                var curdate = DateTime.Now.ToString("yyMMdd");
        //                if (temprc != "1")
        //                {
        //                    var predate = temprc.Substring(0, 6);
        //                    if (predate == curdate)
        //                    {
        //                        temprc = "RQ" + temprc;
        //                    }
        //                    else
        //                    {
        //                        temprc = "RQ" + curdate + "01";
        //                    }
        //                }
        //                else
        //                {
        //                    temprc = "RQ" + curdate + "01";
        //                }
        //                objreq.RQID = temprc;
        //                ObjectSpace.CommitChanges();
        //            }
        //            ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
        //            foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
        //            {
        //                if (parent.Id == "ICM")
        //                {
        //                    foreach (ChoiceActionItem child in parent.Items)
        //                    {
        //                        if (child.Id == "Operations")
        //                        {
        //                            foreach (ChoiceActionItem subchild in child.Items)
        //                            {
        //                                if (subchild.Id == "Review")
        //                                {
        //                                    IObjectSpace objectSpace = Application.CreateObjectSpace();
        //                                    var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingReview'"));
        //                                    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
        //                                    if (count > 0)
        //                                    {
        //                                        subchild.Caption = cap[0] + " (" + count + ")";
        //                                    }
        //                                    else
        //                                    {
        //                                        subchild.Caption = cap[0];
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        //private void SaveAndNewAction_Execute(object sender, ActionBaseEventArgs e)
        //{
        //    try
        //    {
        //        if (View.Id == "Items_DetailView")
        //        {
        //            Items objreq = (Items)View.CurrentObject;
        //            if (objreq.ItemCode == null)
        //            {
        //                IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(Items));
        //                CriteriaOperator ItemCode = CriteriaOperator.Parse("Max(ItemCode)");
        //                string NewItemCode = (Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Items), ItemCode, null)) + 1).ToString();
        //                if (NewItemCode.Length == 1)
        //                {
        //                    NewItemCode = "00" + NewItemCode;
        //                }
        //                else if (NewItemCode.Length == 2)
        //                {
        //                    NewItemCode = "0" + NewItemCode;
        //                }
        //                objreq.ItemCode = NewItemCode;
        //                ObjectSpace.CommitChanges();
        //            }
        //        }
        //        else if (View.Id == "Vendors_DetailView")
        //        {
        //            Vendors objreq = (Vendors)View.CurrentObject;
        //            if (objreq.Vendorcode == null)
        //            {
        //                IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(Vendors));
        //                CriteriaOperator VendorCode = CriteriaOperator.Parse("Max(Vendorcode)");
        //                string NewVendorCode = (Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Vendors), VendorCode, null)) + 1).ToString();
        //                if (NewVendorCode.Length == 1)
        //                {
        //                    NewVendorCode = "00" + NewVendorCode;
        //                }
        //                else if (NewVendorCode.Length == 2)
        //                {
        //                    NewVendorCode = "0" + NewVendorCode;
        //                }
        //                objreq.Vendorcode = NewVendorCode;
        //                ObjectSpace.CommitChanges();
        //            }
        //        }

        //        else if (View.Id == "Requisition_DetailView")
        //        {
        //            Requisition objreq = (Requisition)View.CurrentObject;
        //            if (objreq.RQID == null)
        //            {
        //                IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(Distribution));
        //                CriteriaOperator sam = CriteriaOperator.Parse("Max(SUBSTRING(RQID, 2))");
        //                string temprc = (Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Requisition), sam, null)) + 1).ToString();
        //                var curdate = DateTime.Now.ToString("yyMMdd");
        //                if (temprc != "1")
        //                {
        //                    var predate = temprc.Substring(0, 6);
        //                    if (predate == curdate)
        //                    {
        //                        temprc = "RQ" + temprc;
        //                    }
        //                    else
        //                    {
        //                        temprc = "RQ" + curdate + "01";
        //                    }
        //                }
        //                else
        //                {
        //                    temprc = "RQ" + curdate + "01";
        //                }
        //                objreq.RQID = temprc;
        //                ObjectSpace.CommitChanges();
        //            }
        //            ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
        //            foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
        //            {
        //                if (parent.Id == "ICM")
        //                {
        //                    foreach (ChoiceActionItem child in parent.Items)
        //                    {
        //                        if (child.Id == "Operations")
        //                        {
        //                            foreach (ChoiceActionItem subchild in child.Items)
        //                            {
        //                                if (subchild.Id == "Review")
        //                                {
        //                                    IObjectSpace objectSpace = Application.CreateObjectSpace();
        //                                    var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingReview'"));
        //                                    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
        //                                    if (count > 0)
        //                                    {
        //                                        subchild.Caption = cap[0] + " (" + count + ")";
        //                                    }
        //                                    else
        //                                    {
        //                                        subchild.Caption = cap[0];
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        //private void SaveAction_Execute(object sender, ActionBaseEventArgs e)
        //{
        //    try
        //    {
        //        if (View.Id == "Items_DetailView")
        //        {
        //            Items objreq = (Items)View.CurrentObject;
        //            if (objreq.ItemCode == null)
        //            {
        //                IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(Items));
        //                CriteriaOperator ItemCode = CriteriaOperator.Parse("Max(ItemCode)");
        //                string NewItemCode = (Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Items), ItemCode, null)) + 1).ToString();
        //                if (NewItemCode.Length == 1)
        //                {
        //                    NewItemCode = "00" + NewItemCode;
        //                }
        //                else if (NewItemCode.Length == 2)
        //                {
        //                    NewItemCode = "0" + NewItemCode;
        //                }
        //                objreq.ItemCode = NewItemCode;
        //                ObjectSpace.CommitChanges();
        //            }
        //        }
        //        else if (View.Id == "Vendors_DetailView")
        //        {                    
        //            Vendors objreq = (Vendors)View.CurrentObject;
        //            if (objreq.Vendorcode == null)
        //            {
        //                IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(Vendors));
        //                CriteriaOperator VendorCode = CriteriaOperator.Parse("Max(Vendorcode)");
        //                string NewVendorCode = (Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Vendors), VendorCode, null)) + 1).ToString();
        //                if (NewVendorCode.Length == 1)
        //                {
        //                    NewVendorCode = "00" + NewVendorCode;
        //                }
        //                else if (NewVendorCode.Length == 2)
        //                {
        //                    NewVendorCode = "0" + NewVendorCode;
        //                }
        //                objreq.Vendorcode = NewVendorCode;
        //                ObjectSpace.CommitChanges();
        //            }
        //        }

        //        else if (View.Id == "Requisition_DetailView")
        //        {
        //            Requisition objreq = (Requisition)View.CurrentObject;
        //            if (objreq.RQID == null)
        //            {
        //                IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(Distribution));
        //                CriteriaOperator sam = CriteriaOperator.Parse("Max(SUBSTRING(RQID, 2))");
        //                string temprc = (Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Requisition), sam, null)) + 1).ToString();
        //                var curdate = DateTime.Now.ToString("yyMMdd");
        //                if (temprc != "1")
        //                {
        //                    var predate = temprc.Substring(0, 6);
        //                    if (predate == curdate)
        //                    {
        //                        temprc = "RQ" + temprc;
        //                    }
        //                    else
        //                    {
        //                        temprc = "RQ" + curdate + "01";
        //                    }
        //                }
        //                else
        //                {
        //                    temprc = "RQ" + curdate + "01";
        //                }
        //                objreq.RQID = temprc;
        //                ObjectSpace.CommitChanges();
        //            }
        //            ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
        //            foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
        //            {
        //                if (parent.Id == "ICM")
        //                {
        //                    foreach (ChoiceActionItem child in parent.Items)
        //                    {
        //                        if (child.Id == "Operations")
        //                        {
        //                            foreach (ChoiceActionItem subchild in child.Items)
        //                            {
        //                                if (subchild.Id == "Review")
        //                                {
        //                                    IObjectSpace objectSpace = Application.CreateObjectSpace();
        //                                    var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingReview'"));
        //                                    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
        //                                    if (count > 0)
        //                                    {
        //                                        subchild.Caption = cap[0] + " (" + count + ")";
        //                                    }
        //                                    else
        //                                    {
        //                                        subchild.Caption = cap[0];
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }                   
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                if (View != null && View.Id == "Requisition_LookupListView" && objInfo.Vendor != string.Empty)
                {
                    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["VendorFilter"] = CriteriaOperator.Parse("[Vendor.Vendor] ='" + objInfo.Vendor + "'");
                }
                else if (View != null && View.Id == "Items_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;

                    gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s, e) {  
                            if(e.focusedColumn.fieldName == 'UnitPrice')
                            {
                            e.cancel = false;                  
                            }    
                            else{
                            e.cancel = true;                  
                            }                          
                            }";
                }
                else if (View != null && View.Id == "Items_ListView_Copy")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    gv.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                    gv.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                    gv.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                    //gv.Settings.VerticalScrollableHeight = 200;

                    gv.CustomColumnDisplayText += Gv_CustomColumnDisplayText;
                    gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;

                    string js = @"
                        if (window.ASPxClientGridView)
                            {{
                                var allGrids = ASPx.GetControlCollection().GetControlsByType(ASPxClientGridView);
                            }}
                            var errstat = 0;
                            var errmsg;
                            for (var a in allGrids)
                            {{
                                for (var i = 0; i <= allGrids[a].GetVisibleRowsOnPage() - 1; i++)
                                {{
                                    allGrids[a].batchEditApi.SetCellValue(i, 'Errorlog', null);
                                    if (allGrids[a].batchEditApi.GetCellValue(i, 'Specification') == null || allGrids[a].batchEditApi.GetCellValue(i, 'Unit.Oid') == null || allGrids[a].batchEditApi.GetCellValue(i, 'UnitPrice') <1 || allGrids[a].batchEditApi.GetCellValue(i, 'UnitPrice') >10000 || allGrids[a].batchEditApi.GetCellValue(i, 'StockQty') <0 || allGrids[a].batchEditApi.GetCellValue(i, 'StockQty') >10000 || allGrids[a].batchEditApi.GetCellValue(i, 'ItemUnit') <1 || allGrids[a].batchEditApi.GetCellValue(i, 'ItemUnit') >10000 || allGrids[a].batchEditApi.GetCellValue(i, 'AlertQty') <0 || allGrids[a].batchEditApi.GetCellValue(i, 'AlertQty') >10000 || allGrids[a].batchEditApi.GetCellValue(i, 'VendorCatName') == null)
                                    {{
                                        errmsg = 'Select ';
                                        if(allGrids[a].batchEditApi.GetCellValue(i, 'Unit.Oid') == null)
                                        {{
                                            errmsg = errmsg + 'PackUnit';
                                        }}
                                        var number=parseInt(allGrids[a].batchEditApi.GetCellValue(i, 'UnitPrice'));
                                        if(number <1 || number >100000)
                                        {{
                                            if(errmsg.length > 7)
                                            {{
                                                errmsg = errmsg + ',UnitPrice';
                                            }}
                                            else
                                            {{
                                                errmsg = errmsg + 'UnitPrice ';
                                            }}
                                        }}
                                        if(allGrids[a].batchEditApi.GetCellValue(i, 'StockQty') <0 || allGrids[a].batchEditApi.GetCellValue(i, 'StockQty') >10000)
                                        {{
                                            if(errmsg.length > 7)
                                            {{
                                                errmsg = errmsg + ',StockQty';
                                            }}
                                            else
                                            {{
                                                errmsg = errmsg + 'StockQty ';
                                            }}
                                        }}
                                        if(allGrids[a].batchEditApi.GetCellValue(i, 'ItemUnit') <1 || allGrids[a].batchEditApi.GetCellValue(i, 'ItemUnit') >10000)
                                        {{
                                            if(errmsg.length > 7)
                                            {{
                                                errmsg = errmsg + ',ItemUnit';
                                            }}
                                            else
                                            {{
                                                errmsg = errmsg + 'ItemUnit ';
                                            }}
                                        }}
                                        if(allGrids[a].batchEditApi.GetCellValue(i, 'AlertQty') <0 || allGrids[a].batchEditApi.GetCellValue(i, 'AlertQty') >10000)
                                        {{
                                            if(errmsg.length > 7)
                                            {{
                                                errmsg = errmsg + ',AlertQty';
                                            }}
                                            else
                                            {{
                                                errmsg = errmsg + 'AlertQty ';
                                            }}
                                        }}
                                        if(allGrids[a].batchEditApi.GetCellValue(i, 'Specification') == null)
                                        {{
                                            if(errmsg.length > 7)
                                            {{
                                                errmsg = errmsg + ',Specification';
                                            }}
                                            else
                                            {{
                                                errmsg = errmsg + 'Specification';
                                            }}
                                        }}
                                        if(allGrids[a].batchEditApi.GetCellValue(i, 'VendorCatName') == null)
                                        {{
                                            if(errmsg.length > 7)
                                            {{
                                                errmsg = errmsg + ',VendorCatName';
                                            }}
                                            else
                                            {{
                                                errmsg = errmsg + 'VendorCatName ';
                                            }}
                                        }}
                                        allGrids[a].batchEditApi.SetCellValue(i, 'Errorlog', errmsg);     
                                        errstat = 1
                                    }}
                                }}
                            }}
                            {0}
                    ";
                    ICallbackManagerHolder holder = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    holder.CallbackManager.RegisterHandler("spreadsheetdata", this);
                    Itemsave.SetClientScript(string.Format(CultureInfo.InvariantCulture, js, holder.CallbackManager.GetScript("spreadsheetdata", "errstat")), false);

                    gridListEditor.Grid.ClientSideEvents.CustomButtonClick = @"function(s ,e){ 
                                s.batchEditApi.ResetChanges(e.visibleIndex);     
                            }";
                    //window.scrollTo(width, 0);
                    gridListEditor.Grid.ClientSideEvents.Init =
                        @"function(s, e)
                        { 
                            var element = s.GetMainElement();  
                            var Height = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
                            s.SetHeight(Height - 200);
                            for (var i = 0; i <= s.GetVisibleRowsOnPage() - 1; i++) 
                            {
                                if (s.batchEditApi.GetCellValue(i, 'Errorlog') != null) 
                                {
                                    s.GetRow(i).style.backgroundColor='OrangeRed';   
                                    setTimeout(function () 
                                    {
                                        var width = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
                                        window.scrollTo(width, 0);                                       
                                    }, 2);                                   
                                }
                            }
                        }";
                    gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s, e) {  
                            if(e.focusedColumn.fieldName == 'StockQty')
                            {
                            e.cancel = true;                  
                            }    
                            else{
                            e.cancel = false;                  
                            }                          
                            }";
                }

                else if (View.Id == "Requisition_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.HtmlCommandCellPrepared += GridView_HtmlCommandCellPrepared;
                    gridListEditor.Grid.CommandButtonInitialize += Grid_CommandButtonInitialize;
                    //ASPxGridView gridView = gridListEditor.Grid;
                    //if (gridView != null)
                    //{
                    //    gridView.HtmlCommandCellPrepared += GridView_HtmlCommandCellPrepared;
                    //}
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
                //if (View.Id == "Requisition_ListView")
                //{
                //    ASPxGridView gv = sender as ASPxGridView;
                //    if (gv != null)
                //    {
                //        if (e.ButtonType == DevExpress.Web.ColumnCommandButtonType.SelectCheckbox)
                //        {
                //            object oid = gv.GetRowValues(e.VisibleIndex, "RequestedBy.UserName");
                //            if (oid != SecuritySystem.CurrentUserId)
                //            {
                //                e.Enabled = false;
                //            }
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

        private void GridView_HtmlCommandCellPrepared(object sender, ASPxGridViewTableCommandCellEventArgs e)
        {
            try
            {
                ASPxGridView gv = sender as ASPxGridView;
                if (gv != null)
                {
                    if (e.CommandCellType == GridViewTableCommandCellType.Data)
                    {
                        if (e.CommandColumn.Name == "SelectionCommandColumn" || e.CommandColumn.Name == "InlineEditCommandColumn")
                        {
                            string strRequestedBy = gv.GetRowValuesByKeyValue(e.KeyValue, "RequestedBy.DisplayName").ToString();
                            string strStatus = gv.GetRowValuesByKeyValue(e.KeyValue, "NonPersistentStatus").ToString();
                            if (strRequestedBy != SecuritySystem.CurrentUserName || strStatus == "PendingPurchasing")
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

        private void GridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (View.Id == "Requisition_ListView")
                {

                    if (e.DataColumn.FieldName != "Item.items") return;
                    e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'ItemRequisitionHandler',  'Requisition|'+{0}, '', false)", e.VisibleIndex));
                }


            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Gv_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            try
            {

                if (e.Column.FieldName == "Category.Oid" && e.Value != null)
                {
                    Items importMappingObject = ((ASPxGridView)sender).GetRow(e.VisibleIndex) as Items;
                    e.DisplayText = importMappingObject.Category.category;
                }
                if (e.Column.FieldName == "Grade.Oid" && e.Value != null)
                {
                    Items importMappingObject = ((ASPxGridView)sender).GetRow(e.VisibleIndex) as Items;
                    e.DisplayText = importMappingObject.Grade.Grade;
                }
                if (e.Column.FieldName == "Manufacturer.Oid" && e.Value != null)
                {
                    Items importMappingObject = ((ASPxGridView)sender).GetRow(e.VisibleIndex) as Items;
                    e.DisplayText = importMappingObject.Manufacturer.ManufacturerName;
                }
                if (e.Column.FieldName == "Unit.Oid" && e.Value != null)
                {
                    Items importMappingObject = ((ASPxGridView)sender).GetRow(e.VisibleIndex) as Items;
                    e.DisplayText = importMappingObject.Unit.Option;
                }
                if (e.Column.FieldName == "Vendor.Oid" && e.Value != null)
                {
                    Items importMappingObject = ((ASPxGridView)sender).GetRow(e.VisibleIndex) as Items;
                    e.DisplayText = importMappingObject.Vendor.Vendor;
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
                ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                ObjectSpace.Committing -= ObjectSpace_Committing;
                //mdcSave.SaveAction.Execute -= SaveAction_Execute;
                //mdcSaveClose.SaveAndCloseAction.Execute -= SaveAndCloseAction_Execute;
                //mdcSaveNew.SaveAndNewAction.Execute -= SaveAndNewAction_Execute;
                //if (View.Id == "Items_ListView_Copy")
                //{
                //    ASPxGridListEditor listEditor = (ASPxGridListEditor)((ListView)View).Editor;
                //    listEditor.CreateCustomEditItemTemplate -= itemlistEditor_CreateCustomEditItemTemplate;
                //    listEditor.CreateCustomGridViewDataColumn -= itemlistEditor_CreateCustomGridViewDataColumn;
                //}
                base.OnDeactivated();
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region Events

        //private void SaveAndNewAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    try
        //    {
        //        if (View.Id == "Requisition_DetailView")
        //        {
        //            Requisition objreq = (Requisition)View.CurrentObject;
        //            IObjectSpace space = Application.CreateObjectSpace(typeof(Requisition));
        //            Requisition objchkreq = space.FindObject<Requisition>(CriteriaOperator.Parse("([Status] = 'PendingReview' Or [Status] = 'PendingApproval' Or [Status] = 'PendingOrdering') AND [Item] = ? AND [RequestedBy] = ?", space.GetObjectByKey<Items>(objreq.Item.Oid), space.GetObjectByKey<CustomSystemUser>(objreq.RequestedBy.Oid)));
        //            if (objchkreq != null)
        //            {
        //                Application.ShowViewStrategy.ShowMessage("You have requested the same items which are pending for ordering", InformationType.Error, 1500, InformationPosition.Top);
        //                e.Cancel = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        //private void SaveAndCloseAction_Executed(object sender, ActionBaseEventArgs e)
        //{
        //    try
        //    {
        //        if (View != null && View.Id == "Requisition_DetailView")
        //        {
        //            ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
        //            foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
        //            {
        //                if (parent.Id == "ICM")
        //                {
        //                    foreach (ChoiceActionItem child in parent.Items)
        //                    {
        //                        if (child.Id == "Operations")
        //                        {
        //                            foreach (ChoiceActionItem subchild in child.Items)
        //                            {
        //                                if (subchild.Id == "Review")
        //                                {
        //                                    IObjectSpace objectSpace = Application.CreateObjectSpace();
        //                                    var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingReview'"));
        //                                    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
        //                                    if (count > 0)
        //                                    {
        //                                        subchild.Caption = cap[0] + " (" + count + ")";
        //                                    }
        //                                    else
        //                                    {
        //                                        subchild.Caption = cap[0];
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void objectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Item")
                {
                    if (View.ObjectTypeInfo.Type == typeof(Requisition))
                    {
                        Requisition objreq = (Requisition)e.Object;
                        //&& objreq.Item.Specification != null
                        if (objreq.Item != null)
                        {
                            if (objreq.Item.Manufacturer != null)
                            {
                                objreq.Manufacturer = objreq.Item.Manufacturer;
                            }
                            if (objreq.Item.Vendor != null)
                            {
                                objreq.Vendor = objreq.Item.Vendor;
                            }
                            if (objreq.Item.UnitPrice.ToString() != null)
                            {
                                objreq.UnitPrice = objreq.Item.UnitPrice;
                                objreq.TotalItems = objreq.OrderQty * objreq.Item.ItemUnit;

                                //ISLabLT checking block
                                //if (objreq.Item.IsLabLT == true)
                                //{
                                //    objreq.TotalItems = objreq.OrderQty * objreq.Item.ItemUnit;
                                //}
                                //else
                                //{
                                //    objreq.TotalItems = objreq.OrderQty;
                                //}
                            }
                            IList<Employee> obj1 = ObjectSpace.GetObjects<Employee>(CriteriaOperator.Parse("Oid = ?", objreq.RequestedBy.Oid));
                            foreach (var rec in obj1)
                            {
                                if (rec.Department != null)
                                {
                                    objreq.department = rec.Department;
                                }
                            }
                        }
                    }
                }
                else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "UnitPrice")
                {
                    if (View.ObjectTypeInfo.Type == typeof(Requisition))
                    {
                        Requisition objreq = (Requisition)e.Object;
                        if (objreq.OrderQty.ToString() != null && e.NewValue != null)
                        {
                            objreq.ExpPrice = Math.Round(objreq.OrderQty * Convert.ToDouble(e.NewValue), 2, MidpointRounding.ToEven);
                            //objreq.ExpPrice = string.Format("{0:0.00}", objreq.OrderQty * Convert.ToDouble(e.NewValue));
                            objreq.TotalItems = objreq.OrderQty * objreq.Item.ItemUnit;

                            //ISLabLT checking block
                            //if (objreq.Item.IsLabLT == true)
                            //{
                            //    objreq.TotalItems = objreq.OrderQty * objreq.Item.ItemUnit;
                            //}
                            //else
                            //{
                            //    objreq.TotalItems = objreq.OrderQty;
                            //}
                        }
                        else
                        {
                            objreq.TotalItems = objreq.OrderQty * objreq.Item.ItemUnit;
                        }
                    }
                }

                else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "OrderQty")
                {
                    if (View.ObjectTypeInfo.Type == typeof(Requisition))
                    {
                        Requisition objreq = (Requisition)e.Object;
                        if (objreq.UnitPrice > 0 && e.NewValue != null)
                        {
                            //objreq.ExpPrice = string.Format("{0:0.00}", Convert.ToDouble(objreq.UnitPrice) * Convert.ToDouble(e.NewValue));
                            objreq.ExpPrice = Math.Round(Convert.ToDouble(objreq.UnitPrice) * Convert.ToDouble(e.NewValue), 2, MidpointRounding.ToEven);
                            objreq.TotalItems = Convert.ToInt32(e.NewValue) * objreq.Item.ItemUnit;

                            //ISLabLT checking block
                            //if (objreq.Item.IsLabLT == true)
                            //{
                            //    objreq.TotalItems = Convert.ToInt32(e.NewValue) * objreq.Item.ItemUnit;
                            //}
                            //else
                            //{
                            //    objreq.TotalItems = objreq.OrderQty;
                            //}
                        }
                        else
                        {
                            objreq.TotalItems = Convert.ToInt32(e.NewValue) * objreq.Item.ItemUnit;
                            //objreq.itemremaining = Convert.ToInt32(e.NewValue) * objreq.Item.ItemUnit;
                            //objreq.itemreceived = "0 of " + (Convert.ToInt32(e.NewValue) * objreq.Item.ItemUnit).ToString();
                        }
                    }
                }
                else if (View != null && View.Id == "Items_DetailView" && View.CurrentObject == e.Object && e.PropertyName == "IsFractional")
                {
                    if (View.ObjectTypeInfo.Type == typeof(Items))
                    {
                        Items obj = View.CurrentObject as Items;
                        if(obj.IsFractional)
                        {
                            obj.Amount = obj.NpAmount;
                            obj.AmountUnit = obj.NPAmountUnit;
                        }
                        else
                        {
                            obj.NpAmount = obj.Amount;
                            obj.NPAmountUnit = obj.AmountUnit;
                        }
                    }
                } 
                //else if (View != null && View.Id == "Items_DetailView" && View.CurrentObject == e.Object && (e.PropertyName == "NpAmount"|| e.PropertyName == "NPAmountUnit"))
                //{
                //    if (View.ObjectTypeInfo.Type == typeof(Items))
                //    {
                //        Items obj = View.CurrentObject as Items;
                //        if(obj.IsFractional)
                //        {
                //            obj.Amount = obj.NpAmount;
                //            obj.AmountUnit = obj.NPAmountUnit;
                //        }
                //        else
                //        {
                //            obj.NpAmount = obj.Amount;
                //            obj.NPAmountUnit = obj.AmountUnit;
                //        }
                //    }
                //}

            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Reorder_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "Requisition_ListView")
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        IObjectSpace objspace = Application.CreateObjectSpace();
                        CollectionSource cs = new CollectionSource(objspace, typeof(Requisition));
                        cs.Criteria["filter"] = CriteriaOperator.Parse("IsNullOrEmpty([RQID])");
                        ListView CreatedView = Application.CreateListView("Requisition_ListViewEntermode", cs, true);
                        Frame.SetView(CreatedView);
                        if (objreq.Items == null)
                        {
                            objreq.Items = new List<string>();
                        }
                        foreach (Requisition obj in e.SelectedObjects)
                        {
                            if (!objreq.Items.Contains(obj.Item.items))
                            {
                                objreq.Items.Add(obj.Item.items);
                                Requisition objnewreq = ObjectSpace.CreateObject<Requisition>();
                                objnewreq.Item = ObjectSpace.GetObject<Items>(obj.Item);
                                if (obj.Vendor != null)
                                {
                                    objnewreq.Vendor = ObjectSpace.GetObjectByKey<Vendors>(obj.Vendor.Oid);
                                }
                                if (obj.Manufacturer != null)
                                {
                                    objnewreq.Manufacturer = ObjectSpace.GetObjectByKey<Manufacturer>(obj.Manufacturer.Oid);
                                }
                                objnewreq.UnitPrice = obj.UnitPrice;
                                if (obj.Item.VendorCatName != null)
                                {
                                    objnewreq.Catalog = obj.Item.VendorCatName;
                                }
                                if (obj.OrderQty > 0)
                                {
                                    objnewreq.OrderQty = obj.OrderQty;
                                }
                                if (obj.ExpPrice > 0)
                                {
                                    objnewreq.ExpPrice = obj.ExpPrice;
                                }
                                if (obj.ShippingOption != null)
                                {
                                    objnewreq.ShippingOption = ObjectSpace.GetObjectByKey<Shippingoptions>(obj.ShippingOption.Oid);
                                }
                                if (obj.department != null)
                                {
                                    objnewreq.department = obj.department;
                                }
                                objnewreq.TotalItems = obj.TotalItems;
                                objnewreq.Status = Requisition.TaskStatus.PendingReview;
                                //IList<Employee> obj1 = ObjectSpace.GetObjects<Employee>(CriteriaOperator.Parse("Oid = ?", objnewreq.RequestedBy.Oid));
                                //foreach (var rec in obj1)
                                //{
                                //    if (rec.Department != null)
                                //    {
                                //        objnewreq.department = rec.Department.Name;
                                //    }
                                //}
                                //objnewreq.ExpPrice = objnewreq.OrderQty * obj.UnitPrice;
                                ((ListView)View).CollectionSource.Add(ObjectSpace.GetObject(objnewreq));
                            }
                        }
                        View.Refresh();
                        //Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                        //IObjectSpace os = Application.CreateObjectSpace();
                        //Requisition theObject = os.CreateObject<Requisition>();
                        //foreach (Requisition objItem in View.SelectedObjects)
                        //{                            
                        //    Requisition item = os.GetObject(objItem);
                        //    if (item.Item != null)
                        //    {
                        //        theObject.Item = item.Item;
                        //    }
                        //    if (item.Vendor != null)
                        //    {
                        //        theObject.Vendor = item.Vendor;
                        //    }
                        //    if (item.Manufacturer != null)
                        //    {
                        //        theObject.Manufacturer = item.Manufacturer;
                        //    }
                        //    if (item.Catalog != null)
                        //    {
                        //        theObject.Catalog = item.Catalog;
                        //    }
                        //    if (item.OrderQty.ToString() != null)
                        //    {
                        //        theObject.OrderQty = item.OrderQty;
                        //    }
                        //    if (item.ExpPrice != null)
                        //    {
                        //        theObject.ExpPrice = item.ExpPrice;
                        //    }
                        //    if (item.ShippingOption != null)
                        //    {
                        //        theObject.ShippingOption = item.ShippingOption;
                        //    }
                        //    if (item.department != null)
                        //    {
                        //        theObject.department = item.department;
                        //    }
                        //    theObject.TotalItems = item.TotalItems;
                        //    theObject.UnitPrice = item.UnitPrice;
                        //    theObject.Status = Requisition.TaskStatus.PendingReview;
                        //}
                        //if (ShowNavigationController != null)
                        //{
                        //    foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                        //    {
                        //        if (parent.Id == "ICM")
                        //        {
                        //            foreach (ChoiceActionItem child in parent.Items)
                        //            {
                        //                if (child.Id == "Operations")
                        //                {
                        //                    foreach (ChoiceActionItem subchild in child.Items)
                        //                    {
                        //                        if (subchild.Id == "Review")
                        //                        {
                        //                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                        //                            var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingReview'"));
                        //                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                        //                            if (count > 0)
                        //                            {
                        //                                subchild.Caption = cap[0] + " (" + count + ")";
                        //                            }
                        //                            else
                        //                            {
                        //                                subchild.Caption = cap[0];
                        //                            }
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        //Application.MainWindow.SetView(Application.CreateDetailView(os, theObject));
                    }
                    //else if (View.SelectedObjects.Count > 1)
                    //{
                    //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Info, 1500, InformationPosition.Top);
                    //}
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        private void RequisitionDate_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            try
            {

                if (e.SelectedChoiceActionItem.Id == "1W")
                {
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffDay(RequestedDate, Now()) < 7");
                }
                else if (e.SelectedChoiceActionItem.Id == "1M")
                {
                    //if (SecuritySystem.CurrentUserName != "Admin")
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 1 And [RequestedBy] = ?", SecuritySystem.CurrentUserId);
                    //}
                    //else
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 1");
                    //}
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 1");
                }
                else if (e.SelectedChoiceActionItem.Id == "3M")
                {
                    //if (SecuritySystem.CurrentUserName != "Admin")
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 3 And [RequestedBy] = ?", SecuritySystem.CurrentUserId);
                    //}
                    //else
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 3");
                    //}
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 3");
                }
                else if (e.SelectedChoiceActionItem.Id == "6M")
                {
                    //if (SecuritySystem.CurrentUserName != "Admin")
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 6 And [RequestedBy] = ?", SecuritySystem.CurrentUserId);
                    //}
                    //else
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 6 ");
                    //}
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 6 ");
                }
                else if (e.SelectedChoiceActionItem.Id == "1Y")
                {
                    //if (SecuritySystem.CurrentUserName != "Admin")
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 1 And [RequestedBy] = ?", SecuritySystem.CurrentUserId);
                    //}
                    //else
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 1");
                    //}
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 1");
                }
                else if (e.SelectedChoiceActionItem.Id == "2Y")
                {
                    //if (SecuritySystem.CurrentUserName != "Admin")
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 2 And [RequestedBy] = ?", SecuritySystem.CurrentUserId);
                    //}
                    //else
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 2");
                    //}
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 2");
                }
                else if (e.SelectedChoiceActionItem.Id == "5Y")
                {
                    //if (SecuritySystem.CurrentUserName != "Administrator")
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 5 And [RequestedBy] = ?", SecuritySystem.CurrentUserId);
                    //}
                    //else
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 5");
                    //}
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 5");
                }
                else if (e.SelectedChoiceActionItem.Id == "ALL")
                {
                    //if (SecuritySystem.CurrentUserName != "Administrator")
                    //{
                    //    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("[RequestedBy] = ?", SecuritySystem.CurrentUserId);
                    //}
                    //else
                    //{
                    //    ((ListView)View).CollectionSource.Criteria.Clear();
                    //}
                    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("GCRecord is null", SecuritySystem.CurrentUserId);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Itemsave_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //try
            //{
            //    if (View.Id == "Items_ListView")
            //    {
            //        ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
            //    }
            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        IObjectSpace Space = Application.CreateObjectSpace(typeof(Requisition));
            //        CriteriaOperator sam = CriteriaOperator.Parse("Max(SUBSTRING(ReceiveID, 2))");
            //        string temprc = (Convert.ToInt32(((XPObjectSpace)Space).Session.Evaluate(typeof(Requisition), sam, null)) + 1).ToString();
            //        var curdate = DateTime.Now.ToString("yyMMdd");
            //        if (temprc != "1")
            //        {
            //            var predate = temprc.Substring(0, 6);
            //            if (predate == curdate)
            //            {
            //                temprc = "RC" + temprc;
            //            }
            //            else
            //            {
            //                temprc = "RC" + curdate + "01";
            //            }     
            //        }
            //        else
            //        {
            //            temprc = "RC" + curdate + "01";
            //        }
            //        IObjectSpace objspace = Application.CreateObjectSpace();
            //        Requisition newobj = objspace.CreateObject<Requisition>();
            //        foreach (Items strItem in View.SelectedObjects)
            //        {
            //            newobj.Item = strItem;
            //            newobj.Vendor = strItem.Vendor;
            //            newobj.Manufacturer = strItem.Manufacturer;
            //            newobj.UnitPrice = strItem.UnitPrice;
            //            newobj.ReceiveID = temprc;                                    
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            //}
        }

        private void ImportFromFileAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace sheetObjectSpace = Application.CreateObjectSpace(typeof(ItemsFileUpload));
                ItemsFileUpload spreadSheet = sheetObjectSpace.CreateObject<ItemsFileUpload>();
                DetailView createdView = Application.CreateDetailView(sheetObjectSpace, spreadSheet);
                createdView.ViewEditMode = ViewEditMode.Edit;
                e.DialogController.SaveOnAccept = false;
                e.View = createdView;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        #region OldImportCode
        //private void ImportFromFileAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        ResourceManager rmEnglish = new ResourceManager("Resources.LocalizeResourcesEnglish", Assembly.Load("App_GlobalResources"));
        //        ResourceManager rmChinese = new ResourceManager("Resources.LocalizeResourcesChinese", Assembly.Load("App_GlobalResources"));
        //        ItemsFileUpload itemsFile = (ItemsFileUpload)e.PopupWindowViewCurrentObject;
        //        if (itemsFile.InputFile != null)
        //        {
        //            string strFileName = itemsFile.InputFile.FileName;
        //            string strFilePath = HttpContext.Current.Server.MapPath(@"~\TestSpreadSheet\");
        //            string strLocalPath = HttpContext.Current.Server.MapPath(@"~\TestSpreadSheet\" + strFileName);
        //            if (Directory.Exists(strFilePath) == false)
        //            {
        //                Directory.CreateDirectory(strFilePath);
        //            }
        //            byte[] file = itemsFile.InputFile.Content;
        //            File.WriteAllBytes(strLocalPath, file);
        //            string fileExtension = Path.GetExtension(itemsFile.InputFile.FileName);
        //            string connectionString = string.Empty;
        //            if (fileExtension == ".xlsx")
        //            {
        //                connectionString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0 Xml;", strLocalPath);
        //            }
        //            else if (fileExtension == ".xls")
        //            {
        //                connectionString = String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;", strLocalPath);
        //            }
        //            //objectspace change
        //            IObjectSpace cloneitemObjectSpace = Application.CreateObjectSpace();
        //            CollectionSource source = new CollectionSource(cloneitemObjectSpace, typeof(Items));
        //            source.Criteria["filter"] = CriteriaOperator.Parse("IsNullOrEmpty([ItemCode])");
        //            ListView cloneItemListView = Application.CreateListView("Items_ListView_Copy", source, true);
        //            Frame.SetView(cloneItemListView);

        //            if (connectionString != string.Empty)
        //            {
        //                using (var conn = new OleDbConnection(connectionString))
        //                {
        //                    conn.Open();
        //                    List<string> sheets = new List<string>();
        //                    OleDbDataAdapter oleda = new OleDbDataAdapter();
        //                    DataTable sheetNameTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //                    foreach (DataRow drSheet in sheetNameTable.Rows)
        //                    {
        //                        if (drSheet["TABLE_NAME"].ToString().Contains("$"))
        //                        {
        //                            string s = drSheet["TABLE_NAME"].ToString();
        //                            sheets.Add(s.StartsWith("'") ? s.Substring(1, s.Length - 3) : s.Substring(0, s.Length - 1));
        //                        }
        //                    }

        //                    var cmd = conn.CreateCommand();
        //                    cmd.CommandText = String.Format(
        //                        @"SELECT * FROM [{0}]", sheets[0] + "$"
        //                        );

        //                    //variable name change
        //                    packageunits.packageunit = new List<Packageunits>();
        //                    packageunits.vendors = new List<Vendors>();
        //                    itemsltno.Items = new List<string>();
        //                    oleda = new OleDbDataAdapter(cmd);
        //                    using (dt = new DataTable())
        //                    {
        //                        oleda.Fill(dt);
        //                        foreach (DataRow row in dt.Rows)
        //                        {
        //                            var isEmpty = row.ItemArray.All(c => c is DBNull);
        //                            if (!isEmpty)
        //                            {
        //                                List<string> errorlist = new List<string>();
        //                                DateTime dateTime;
        //                                if (dt.Columns.Contains(rmChinese.GetString("ItemName")) && !row.IsNull(rmChinese.GetString("ItemName")))
        //                                {
        //                                    strItemName = row[rmChinese.GetString("ItemName")].ToString();
        //                                }
        //                                else if (dt.Columns.Contains(rmEnglish.GetString("ItemName")) && !row.IsNull(rmEnglish.GetString("ItemName")))
        //                                {
        //                                    strItemName = row[rmEnglish.GetString("ItemName")].ToString();
        //                                }
        //                                else
        //                                {
        //                                    errorlist.Add("ItemName");
        //                                }

        //                                if (dt.Columns.Contains(rmChinese.GetString("Vendor")) && !row.IsNull(rmChinese.GetString("Vendor")))
        //                                {
        //                                    strVendorName = row[rmChinese.GetString("Vendor")].ToString();
        //                                }
        //                                else if (dt.Columns.Contains(rmEnglish.GetString("Vendor")) && !row.IsNull(rmEnglish.GetString("Vendor")))
        //                                {
        //                                    strVendorName = row[rmEnglish.GetString("Vendor")].ToString();
        //                                }
        //                                else
        //                                {
        //                                    errorlist.Add("Vendor");
        //                                }

        //                                if (dt.Columns.Contains(rmChinese.GetString("Catalog#")) && !row.IsNull(rmChinese.GetString("Catalog#")))
        //                                {
        //                                    strVendorCatName = row[rmChinese.GetString("Catalog#")].ToString();
        //                                }
        //                                else if (dt.Columns.Contains(rmEnglish.GetString("Catalog#")) && !row.IsNull(rmEnglish.GetString("Catalog#")))
        //                                {
        //                                    strVendorCatName = row[rmEnglish.GetString("Catalog#")].ToString();
        //                                }
        //                                else
        //                                {
        //                                    strVendorCatName = "N/A";
        //                                }

        //                                if (dt.Columns.Contains(rmChinese.GetString("Amount")) && !row.IsNull(rmChinese.GetString("Amount")))
        //                                {
        //                                    strAmount = row[rmChinese.GetString("Amount")].ToString();
        //                                }
        //                                else if (dt.Columns.Contains(rmEnglish.GetString("Amount")) && !row.IsNull(rmEnglish.GetString("Amount")))
        //                                {
        //                                    strAmount = row[rmEnglish.GetString("Amount")].ToString();
        //                                }
        //                                else
        //                                {
        //                                    strAmount = string.Empty;
        //                                }

        //                                if (dt.Columns.Contains(rmChinese.GetString("AmountUnit")) && !row.IsNull(rmChinese.GetString("AmountUnit")))
        //                                {
        //                                    strAmountUnits = row[rmChinese.GetString("AmountUnit")].ToString();
        //                                }
        //                                else if (dt.Columns.Contains(rmEnglish.GetString("AmountUnit")) && !row.IsNull(rmEnglish.GetString("AmountUnit")))
        //                                {
        //                                    strAmountUnits = row[rmEnglish.GetString("AmountUnit")].ToString();
        //                                }
        //                                else
        //                                {
        //                                    strAmountUnits = string.Empty;
        //                                }

        //                                if (dt.Columns.Contains(rmChinese.GetString("Specification")) && !row.IsNull(rmChinese.GetString("Specification")))
        //                                {
        //                                    strSpecification = row[rmChinese.GetString("Specification")].ToString();
        //                                }
        //                                else if (dt.Columns.Contains(rmEnglish.GetString("Specification")) && !row.IsNull(rmEnglish.GetString("Specification")))
        //                                {
        //                                    strSpecification = row[rmEnglish.GetString("Specification")].ToString();
        //                                }
        //                                else
        //                                {
        //                                    errorlist.Add("Specification");
        //                                }

        //                                if (dt.Columns.Contains(rmChinese.GetString("Category")) && !row.IsNull(rmChinese.GetString("Category")))
        //                                {
        //                                    strCategory = row[rmChinese.GetString("Category")].ToString();
        //                                }
        //                                else if (dt.Columns.Contains(rmEnglish.GetString("Category")) && !row.IsNull(rmEnglish.GetString("Category")))
        //                                {
        //                                    strCategory = row[rmEnglish.GetString("Category")].ToString();
        //                                }
        //                                else
        //                                {
        //                                    errorlist.Add("Category");
        //                                }

        //                                if ((dt.Columns.Contains(rmChinese.GetString("IsVendorLT")) && !row.IsNull(rmChinese.GetString("IsVendorLT")) && row[rmChinese.GetString("IsVendorLT")].GetType() != typeof(bool))
        //                                    || (dt.Columns.Contains(rmEnglish.GetString("IsVendorLT")) && !row.IsNull(rmEnglish.GetString("IsVendorLT")) && row[rmEnglish.GetString("IsVendorLT")].GetType() != typeof(bool)))
        //                                {
        //                                    errorlist.Add("IsVendorLT");
        //                                }

        //                                if ((dt.Columns.Contains(rmChinese.GetString("IsLabLT")) && !row.IsNull(rmChinese.GetString("IsLabLT")) && row[rmChinese.GetString("IsLabLT")].GetType() != typeof(bool))
        //                                    || (dt.Columns.Contains(rmEnglish.GetString("IsLabLT")) && !row.IsNull(rmEnglish.GetString("IsLabLT")) && row[rmEnglish.GetString("IsLabLT")].GetType() != typeof(bool)))
        //                                {
        //                                    errorlist.Add("IsLabLT");
        //                                }

        //                                if ((dt.Columns.Contains(rmChinese.GetString("IsToxic")) && !row.IsNull(rmChinese.GetString("IsToxic")) && row[rmChinese.GetString("IsToxic")].GetType() != typeof(bool))
        //                                   || (dt.Columns.Contains(rmEnglish.GetString("IsToxic")) && !row.IsNull(rmEnglish.GetString("IsToxic")) && row[rmEnglish.GetString("IsToxic")].GetType() != typeof(bool)))
        //                                {
        //                                    errorlist.Add("IsToxic");
        //                                }

        //                                if ((dt.Columns.Contains(rmChinese.GetString("RetireDate")) && !row.IsNull(rmChinese.GetString("RetireDate")) &&
        //                                    !DateTime.TryParseExact(row[rmChinese.GetString("RetireDate")].ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dateTime) && row[rmChinese.GetString("RetireDate")].GetType() != typeof(DateTime))
        //                                   || (dt.Columns.Contains(rmEnglish.GetString("RetireDate")) && !row.IsNull(rmEnglish.GetString("RetireDate")) &&
        //                                   !DateTime.TryParseExact(row[rmEnglish.GetString("RetireDate")].ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dateTime) && row[rmEnglish.GetString("RetireDate")].GetType() != typeof(DateTime)))
        //                                {
        //                                    errorlist.Add("RetireDate");
        //                                }

        //                                if ((dt.Columns.Contains(rmChinese.GetString("ExpiryDate")) && !row.IsNull(rmChinese.GetString("ExpiryDate")) && !DateTime.TryParseExact(row[rmChinese.GetString("ExpiryDate")].ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dateTime) && row[rmChinese.GetString("ExpiryDate")].GetType() != typeof(DateTime))
        //                                  || (dt.Columns.Contains(rmEnglish.GetString("ExpiryDate")) && !row.IsNull(rmEnglish.GetString("ExpiryDate")) && !DateTime.TryParseExact(row[rmEnglish.GetString("ExpiryDate")].ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dateTime) && row[rmEnglish.GetString("ExpiryDate")].GetType() != typeof(DateTime)))
        //                                {
        //                                    errorlist.Add("ExpiryDate");
        //                                }

        //                                if (errorlist.Count == 0)
        //                                {
        //                                    Items findItem = ObjectSpace.FindObject<Items>(CriteriaOperator.Parse("[items]='" + strItemName + "' And [Vendor.Vendor] ='" + strVendorName + "' And [VendorCatName] ='" + strVendorCatName + "' And [Amount] ='" + strAmount + "' And [AmountUnit.UnitName] ='" + strAmountUnits + "' AND [Specification]='" + strSpecification + "' AND [Category.category] = '" + strCategory + "'"));
        //                                    if (findItem == null)
        //                                    {
        //                                        string strVendorlt = string.Empty;
        //                                        string strStorage = string.Empty;
        //                                        intstockqty = 0;
        //                                        Nullable<DateTime> dateExpiry = null;
        //                                        Items items = ObjectSpace.CreateObject<Items>();
        //                                        items.items = strItemName;
        //                                        items.VendorCatName = strVendorCatName;
        //                                        items.Specification = strSpecification;
        //                                        items.Amount = strAmount;
        //                                        if (strVendorName != string.Empty)
        //                                        {
        //                                            Vendors vendor = ObjectSpace.FindObject<Vendors>(CriteriaOperator.Parse("[Vendor]='" + strVendorName + "'"));
        //                                            if (vendor != null)
        //                                            {
        //                                                items.Vendor = vendor;
        //                                            }
        //                                            else
        //                                            {
        //                                                Vendors createVendor = ObjectSpace.CreateObject<Vendors>();
        //                                                createVendor.Vendor = strVendorName;
        //                                                items.Vendor = createVendor;
        //                                                packageunits.vendors.Add(createVendor);
        //                                            }
        //                                        }
        //                                        if (strAmountUnits != string.Empty)
        //                                        {
        //                                            Unit amountUnit = ObjectSpace.FindObject<Unit>(CriteriaOperator.Parse("[UnitName]='" + strAmountUnits + "'"));
        //                                            if (amountUnit != null)
        //                                            {
        //                                                items.AmountUnit = amountUnit;
        //                                            }
        //                                            else
        //                                            {
        //                                                Unit createAmountunit = ObjectSpace.CreateObject<Unit>();
        //                                                createAmountunit.UnitName = strAmountUnits;
        //                                                items.AmountUnit = createAmountunit;
        //                                            }
        //                                        }
        //                                        if (strCategory != string.Empty)
        //                                        {
        //                                            Category category = ObjectSpace.FindObject<Category>(CriteriaOperator.Parse("[category]='" + strCategory + "'"));
        //                                            if (category != null)
        //                                            {
        //                                                items.Category = category;
        //                                            }
        //                                            else
        //                                            {
        //                                                Category createCategory = ObjectSpace.CreateObject<Category>();
        //                                                createCategory.category = strCategory;
        //                                                items.Category = createCategory;
        //                                            }
        //                                        }

        //                                        if (dt.Columns.Contains(rmChinese.GetString("StockQty")) && !row.IsNull(rmChinese.GetString("StockQty")))
        //                                        {
        //                                            intstockqty = Convert.ToInt32(row[rmChinese.GetString("StockQty")]);
        //                                        }
        //                                        else if (dt.Columns.Contains(rmEnglish.GetString("StockQty")) && !row.IsNull(rmEnglish.GetString("StockQty")))
        //                                        {
        //                                            intstockqty = Convert.ToInt32(row[rmEnglish.GetString("StockQty")]);
        //                                        }
        //                                        items.StockQty = intstockqty;

        //                                        if (dt.Columns.Contains(rmChinese.GetString("Vendorlt")) && !row.IsNull(rmChinese.GetString("Vendorlt")))
        //                                        {
        //                                            strVendorlt = row[rmChinese.GetString("Vendorlt")].ToString();
        //                                        }
        //                                        else if (dt.Columns.Contains(rmEnglish.GetString("Vendorlt")) && !row.IsNull(rmEnglish.GetString("Vendorlt")))
        //                                        {
        //                                            strVendorlt = row[rmEnglish.GetString("Vendorlt")].ToString();
        //                                        }

        //                                        if (dt.Columns.Contains(rmChinese.GetString("Storage")) && !row.IsNull(rmChinese.GetString("Storage")))
        //                                        {
        //                                            strStorage = row[rmChinese.GetString("Storage")].ToString();
        //                                        }
        //                                        else if (dt.Columns.Contains(rmEnglish.GetString("Storage")) && !row.IsNull(rmEnglish.GetString("Storage")))
        //                                        {
        //                                            strStorage = row[rmEnglish.GetString("Storage")].ToString();
        //                                        }

        //                                        if (dt.Columns.Contains(rmChinese.GetString("ExpiryDate")) && !row.IsNull(rmChinese.GetString("ExpiryDate")))
        //                                        {
        //                                            if (row[rmChinese.GetString("ExpiryDate")].GetType() == typeof(DateTime))
        //                                            {
        //                                                dateExpiry = Convert.ToDateTime(row[rmChinese.GetString("ExpiryDate")]);
        //                                            }
        //                                            else if (row[rmChinese.GetString("ExpiryDate")].GetType() == typeof(string))
        //                                            {
        //                                                string strdate = row[rmChinese.GetString("ExpiryDate")].ToString();
        //                                                if (strdate != string.Empty)
        //                                                {
        //                                                    dateExpiry = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
        //                                                }
        //                                            }
        //                                        }
        //                                        else if (dt.Columns.Contains(rmEnglish.GetString("ExpiryDate")) && !row.IsNull(rmEnglish.GetString("ExpiryDate")))
        //                                        {
        //                                            if (row[rmEnglish.GetString("ExpiryDate")].GetType() == typeof(DateTime))
        //                                            {
        //                                                dateExpiry = Convert.ToDateTime(row[rmEnglish.GetString("ExpiryDate")]);
        //                                            }
        //                                            else if (row[rmEnglish.GetString("ExpiryDate")].GetType() == typeof(string))
        //                                            {
        //                                                string strdate = row[rmEnglish.GetString("ExpiryDate")].ToString();
        //                                                if (strdate != string.Empty)
        //                                                {
        //                                                    dateExpiry = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
        //                                                }
        //                                            }
        //                                        }

        //                                        itemsltno.Items.Add(items.Oid + ";" + strStorage + ";" + dateExpiry + ";" + intstockqty.ToString() + ";" + strVendorlt);
        //                                        processItems(items, dt, row, rmChinese, rmEnglish);
        //                                        ((ListView)View).CollectionSource.Add(items);
        //                                    }
        //                                    else
        //                                    {
        //                                        string strStorage = string.Empty;
        //                                        string strVendorlt = string.Empty;
        //                                        intstockqty = 0;
        //                                        Nullable<DateTime> dateExpiry = null;
        //                                        int stockQty = findItem.StockQty;
        //                                        if (dt.Columns.Contains(rmChinese.GetString("StockQty")) && !row.IsNull(rmChinese.GetString("StockQty")))
        //                                        {
        //                                            intstockqty = Convert.ToInt32(row[rmChinese.GetString("StockQty")]);
        //                                            stockQty += Convert.ToInt32(row[rmChinese.GetString("StockQty")]);
        //                                        }
        //                                        else if (dt.Columns.Contains(rmEnglish.GetString("StockQty")) && !row.IsNull(rmEnglish.GetString("StockQty")))
        //                                        {
        //                                            intstockqty = Convert.ToInt32(row[rmEnglish.GetString("StockQty")]);
        //                                            stockQty += Convert.ToInt32(row[rmEnglish.GetString("StockQty")]);
        //                                        }
        //                                        findItem.StockQty = stockQty;

        //                                        if (dt.Columns.Contains(rmChinese.GetString("Vendorlt")) && !row.IsNull(rmChinese.GetString("Vendorlt")))
        //                                        {
        //                                            strVendorlt = row[rmChinese.GetString("Vendorlt")].ToString();
        //                                        }
        //                                        else if (dt.Columns.Contains(rmEnglish.GetString("Vendorlt")) && !row.IsNull(rmEnglish.GetString("Vendorlt")))
        //                                        {
        //                                            strVendorlt = row[rmEnglish.GetString("Vendorlt")].ToString();
        //                                        }

        //                                        if (dt.Columns.Contains(rmChinese.GetString("Storage")) && !row.IsNull(rmChinese.GetString("Storage")))
        //                                        {
        //                                            strStorage = row[rmChinese.GetString("Storage")].ToString();
        //                                        }
        //                                        else if (dt.Columns.Contains(rmEnglish.GetString("Storage")) && !row.IsNull(rmEnglish.GetString("Storage")))
        //                                        {
        //                                            strStorage = row[rmEnglish.GetString("Storage")].ToString();
        //                                        }

        //                                        if (dt.Columns.Contains(rmChinese.GetString("ExpiryDate")) && !row.IsNull(rmChinese.GetString("ExpiryDate")))
        //                                        {
        //                                            if (row[rmChinese.GetString("ExpiryDate")].GetType() == typeof(DateTime))
        //                                            {
        //                                                dateExpiry = Convert.ToDateTime(row[rmChinese.GetString("ExpiryDate")]);
        //                                            }
        //                                            else if (row[rmChinese.GetString("ExpiryDate")].GetType() == typeof(string))
        //                                            {
        //                                                string strdate = row[rmChinese.GetString("ExpiryDate")].ToString();
        //                                                if (strdate != string.Empty)
        //                                                {
        //                                                    dateExpiry = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
        //                                                }
        //                                            }
        //                                        }
        //                                        else if (dt.Columns.Contains(rmEnglish.GetString("ExpiryDate")) && !row.IsNull(rmEnglish.GetString("ExpiryDate")))
        //                                        {
        //                                            if (row[rmEnglish.GetString("ExpiryDate")].GetType() == typeof(DateTime))
        //                                            {
        //                                                dateExpiry = Convert.ToDateTime(row[rmEnglish.GetString("ExpiryDate")]);
        //                                            }
        //                                            else if (row[rmEnglish.GetString("ExpiryDate")].GetType() == typeof(string))
        //                                            {
        //                                                string strdate = row[rmEnglish.GetString("ExpiryDate")].ToString();
        //                                                if (strdate != string.Empty)
        //                                                {
        //                                                    dateExpiry = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
        //                                                }
        //                                            }
        //                                        }

        //                                        itemsltno.Items.Add(findItem.Oid + ";" + strStorage + ";" + dateExpiry + ";" + intstockqty.ToString() + ";" + strVendorlt);
        //                                        processItems(findItem, dt, row, rmChinese, rmEnglish);
        //                                        ((ListView)View).CollectionSource.Add(findItem);
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    var error = "Error in columns - " + string.Join(", ", errorlist) + " of row number - " + (dt.Rows.IndexOf(row) + 1);
        //                                    Application.ShowViewStrategy.ShowMessage(error, InformationType.Error, 6000, InformationPosition.Top);
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    conn.Close();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, 3000, InformationPosition.Top);
        //    }
        //}
        //private Items processItems(Items items, DataTable dt, DataRow row, ResourceManager rmChinese, ResourceManager rmEnglish)
        //{
        //    string strUnit = string.Empty;
        //    string strVendorName = string.Empty;
        //    string strGrade = string.Empty;
        //    string strItemDescription = string.Empty;
        //    string strComment = string.Empty;
        //    double unitPrice = 0;
        //    int ItemUnit = 0;
        //    int alertQty = 0;
        //    bool isVendorLT = false;
        //    bool isLabLT = false;
        //    bool isToxic = false;

        //    if (dt.Columns.Contains(rmChinese.GetString("PackUnits")) && !row.IsNull(rmChinese.GetString("PackUnits")))
        //    {
        //        strUnit = row[rmChinese.GetString("PackUnits")].ToString();
        //    }
        //    else if (dt.Columns.Contains(rmEnglish.GetString("PackUnits")) && !row.IsNull(rmEnglish.GetString("PackUnits")))
        //    {
        //        strUnit = row[rmEnglish.GetString("PackUnits")].ToString();
        //    }
        //    if (strUnit != string.Empty)
        //    {
        //        Packageunits unit = ObjectSpace.FindObject<Packageunits>(CriteriaOperator.Parse("[Option]='" + strUnit + "'"));
        //        if (unit != null)
        //        {
        //            items.Unit = unit;
        //        }
        //        else
        //        {
        //            Packageunits createUnit = ObjectSpace.CreateObject<Packageunits>();
        //            createUnit.Option = strUnit;
        //            items.Unit = createUnit;
        //            packageunits.packageunit.Add(createUnit);
        //        }
        //    }

        //    if (dt.Columns.Contains(rmChinese.GetString("UnitPrice")) && !row.IsNull(rmChinese.GetString("UnitPrice")))
        //    {
        //        unitPrice = Convert.ToDouble(row[rmChinese.GetString("UnitPrice")]);
        //    }
        //    else if (dt.Columns.Contains(rmEnglish.GetString("UnitPrice")) && !row.IsNull(rmEnglish.GetString("UnitPrice")))
        //    {
        //        unitPrice = Convert.ToDouble(row[rmEnglish.GetString("UnitPrice")]);
        //    }
        //    items.UnitPrice = unitPrice;

        //    if (dt.Columns.Contains(rmChinese.GetString("Item/Unit")) && !row.IsNull(rmChinese.GetString("Item/Unit")))
        //    {
        //        ItemUnit = Convert.ToInt32(row[rmChinese.GetString("Item/Unit")]);
        //    }
        //    else if (dt.Columns.Contains(rmEnglish.GetString("Item/Unit")) && !row.IsNull(rmEnglish.GetString("Item/Unit")))
        //    {
        //        ItemUnit = Convert.ToInt32(row[rmEnglish.GetString("Item/Unit")]);
        //    }
        //    items.ItemUnit = ItemUnit;

        //    if (dt.Columns.Contains(rmChinese.GetString("VendorItemName")) && !row.IsNull(rmChinese.GetString("VendorItemName")))
        //    {
        //        strVendorName = row[rmChinese.GetString("VendorItemName")].ToString();
        //    }
        //    else if (dt.Columns.Contains(rmEnglish.GetString("VendorItemName")) && !row.IsNull(rmEnglish.GetString("VendorItemName")))
        //    {
        //        strVendorName = row[rmEnglish.GetString("VendorItemName")].ToString();
        //    }
        //    items.VendorItemName = strVendorName;

        //    if (dt.Columns.Contains(rmChinese.GetString("Grade")) && !row.IsNull(rmChinese.GetString("Grade")))
        //    {
        //        strGrade = row[rmChinese.GetString("Grade")].ToString();
        //    }
        //    else if (dt.Columns.Contains(rmEnglish.GetString("Grade")) && !row.IsNull(rmEnglish.GetString("Grade")))
        //    {
        //        strGrade = row[rmEnglish.GetString("Grade")].ToString();
        //    }
        //    if (strGrade != string.Empty)
        //    {
        //        Grades grades = ObjectSpace.FindObject<Grades>(CriteriaOperator.Parse("[Grade]='" + strGrade + "'"));
        //        if (grades != null)
        //        {
        //            items.Grade = grades;
        //        }
        //        else
        //        {
        //            Grades createGrades = ObjectSpace.CreateObject<Grades>();
        //            createGrades.Grade = strGrade;
        //            items.Grade = createGrades;
        //        }
        //    }

        //    if (dt.Columns.Contains(rmChinese.GetString("AlertQty")) && !row.IsNull(rmChinese.GetString("AlertQty")))
        //    {
        //        alertQty = Convert.ToInt32(row[rmChinese.GetString("AlertQty")]);
        //    }
        //    else if (dt.Columns.Contains(rmEnglish.GetString("AlertQty")) && !row.IsNull(rmEnglish.GetString("AlertQty")))
        //    {
        //        alertQty = Convert.ToInt32(row[rmEnglish.GetString("AlertQty")]);
        //    }
        //    items.AlertQty = alertQty;

        //    if (dt.Columns.Contains(rmChinese.GetString("IsVendorLT")) && !row.IsNull(rmChinese.GetString("IsVendorLT")))
        //    {
        //        isVendorLT = Convert.ToBoolean(row[rmChinese.GetString("IsVendorLT")]);
        //    }
        //    else if (dt.Columns.Contains(rmEnglish.GetString("IsVendorLT")) && !row.IsNull(rmEnglish.GetString("IsVendorLT")))
        //    {
        //        isVendorLT = Convert.ToBoolean(row[rmEnglish.GetString("IsVendorLT")]);
        //    }
        //    items.IsVendorLT = isVendorLT;

        //    if (dt.Columns.Contains(rmChinese.GetString("IsLabLT")) && !row.IsNull(rmChinese.GetString("IsLabLT")))
        //    {
        //        isLabLT = Convert.ToBoolean(row[rmChinese.GetString("IsLabLT")]);
        //    }
        //    else if (dt.Columns.Contains(rmEnglish.GetString("IsLabLT")) && !row.IsNull(rmEnglish.GetString("IsLabLT")))
        //    {
        //        isLabLT = Convert.ToBoolean(row[rmEnglish.GetString("IsLabLT")]);
        //    }
        //    items.IsLabLT = isLabLT;

        //    if (dt.Columns.Contains(rmChinese.GetString("IsToxic")) && !row.IsNull(rmChinese.GetString("IsToxic")))
        //    {
        //        isToxic = Convert.ToBoolean(row[rmChinese.GetString("IsToxic")]);
        //    }
        //    else if (dt.Columns.Contains(rmEnglish.GetString("IsToxic")) && !row.IsNull(rmEnglish.GetString("IsToxic")))
        //    {
        //        isToxic = Convert.ToBoolean(row[rmEnglish.GetString("IsToxic")]);
        //    }
        //    items.IsToxic = isToxic;

        //    if (dt.Columns.Contains(rmChinese.GetString("RetireDate")) && !row.IsNull(rmChinese.GetString("RetireDate")))
        //    {
        //        if (row[rmChinese.GetString("RetireDate")].GetType() == typeof(DateTime))
        //        {
        //            items.RetireDate = Convert.ToDateTime(row[rmChinese.GetString("RetireDate")]);
        //        }
        //        else if (row[rmChinese.GetString("RetireDate")].GetType() == typeof(string))
        //        {
        //            string strdate = row[rmChinese.GetString("RetireDate")].ToString();
        //            if (strdate != string.Empty)
        //            {
        //                string[] strdateonly = strdate.Split(' ');
        //                items.RetireDate = DateTime.ParseExact(strdateonly[0], "MM/dd/yyyy", null);
        //            }
        //        }
        //    }
        //    else if (dt.Columns.Contains(rmEnglish.GetString("RetireDate")) && !row.IsNull(rmEnglish.GetString("RetireDate")))
        //    {
        //        if (row[rmEnglish.GetString("RetireDate")].GetType() == typeof(DateTime))
        //        {
        //            items.RetireDate = Convert.ToDateTime(row[rmEnglish.GetString("RetireDate")]);
        //        }
        //        else if (row[rmEnglish.GetString("RetireDate")].GetType() == typeof(string))
        //        {
        //            string strdate = row[rmEnglish.GetString("RetireDate")].ToString();
        //            if (strdate != string.Empty)
        //            {
        //                string[] strdateonly = strdate.Split(' ');
        //                items.RetireDate = DateTime.ParseExact(strdateonly[0], "MM/dd/yyyy", null);
        //            }
        //        }
        //    }

        //    if (dt.Columns.Contains(rmChinese.GetString("Item Description")) && !row.IsNull(rmChinese.GetString("Item Description")))
        //    {
        //        strItemDescription = row[rmChinese.GetString("Item Description")].ToString();
        //    }
        //    else if (dt.Columns.Contains(rmEnglish.GetString("Item Description")) && !row.IsNull(rmEnglish.GetString("Item Description")))
        //    {
        //        strItemDescription = row[rmEnglish.GetString("Item Description")].ToString();
        //    }
        //    items.ItemDescription = strItemDescription;

        //    if (dt.Columns.Contains(rmChinese.GetString("Comment")) && !row.IsNull(rmChinese.GetString("Comment")))
        //    {
        //        strComment = row[rmChinese.GetString("Comment")].ToString();
        //    }
        //    else if (dt.Columns.Contains(rmEnglish.GetString("Comment")) && !row.IsNull(rmEnglish.GetString("Comment")))
        //    {
        //        strComment = row[rmEnglish.GetString("Comment")].ToString();
        //    }
        //    items.Comment = strComment;

        //    return items;
        //}
        //public void ProcessAction(string parameter)
        //{
        //    try
        //    {
        //        if (parameter == "0")
        //        {
        //            ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
        //            ObjectSpace.CommitChanges();

        //            foreach (Items item in ((ListView)View).CollectionSource.List)
        //            {
        //                if (string.IsNullOrEmpty(item.ItemCode))
        //                {
        //                    CriteriaOperator ItemCode = CriteriaOperator.Parse("Max(ItemCode)");
        //                    string NewItemCode = (Convert.ToInt32(((XPObjectSpace)((ListView)View).ObjectSpace).Session.Evaluate(typeof(Items), ItemCode, null)) + 1).ToString();
        //                    if (NewItemCode.Length == 1)
        //                    {
        //                        NewItemCode = "00" + NewItemCode;
        //                    }
        //                    else if (NewItemCode.Length == 2)
        //                    {
        //                        NewItemCode = "0" + NewItemCode;
        //                    }
        //                    item.ItemCode = NewItemCode;
        //                    ObjectSpace.CommitChanges();
        //                }
        //            }

        //            IList<Vendors> emptyvendors = ObjectSpace.GetObjects<Vendors>(CriteriaOperator.Parse("IsNullOrEmpty([Vendorcode])"));
        //            foreach (Vendors vendor in packageunits.vendors)
        //            {
        //                CriteriaOperator Vendorcode = CriteriaOperator.Parse("Max(Vendorcode)");
        //                string NewVendorCode = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(Vendors), Vendorcode, null)) + 1).ToString();
        //                if (NewVendorCode.Length == 1)
        //                {
        //                    NewVendorCode = "00" + NewVendorCode;
        //                }
        //                else if (NewVendorCode.Length == 2)
        //                {
        //                    NewVendorCode = "0" + NewVendorCode;
        //                }
        //                vendor.Vendorcode = NewVendorCode;
        //                ObjectSpace.CommitChanges();
        //            }

        //            foreach (Packageunits unit in packageunits.packageunit)
        //            {
        //                CriteriaOperator sort = CriteriaOperator.Parse("Max(Sort)");
        //                int newSort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(Packageunits), sort, null)) + 1);
        //                unit.Sort = newSort;
        //                ObjectSpace.CommitChanges();
        //            }

        //            IObjectSpace Space = Application.CreateObjectSpace(typeof(Requisition));
        //            CriteriaOperator sam = CriteriaOperator.Parse("Max(SUBSTRING(ReceiveID, 2))");
        //            string temprc = (Convert.ToInt32(((XPObjectSpace)Space).Session.Evaluate(typeof(Requisition), sam, null)) + 1).ToString();
        //            var curedate = DateTime.Now.ToString("yyMMdd");
        //            if (temprc != "1")
        //            {
        //                var predate = temprc.Substring(0, 6);
        //                if (predate == curedate)
        //                {
        //                    temprc = "RC" + temprc;
        //                }
        //                else
        //                {
        //                    temprc = "RC" + curedate + "01";
        //                }
        //            }
        //            else
        //            {
        //                temprc = "RC" + curedate + "01";
        //            }
        //            var receivedate = DateTime.Now;
        //            foreach (string distributionItems in itemsltno.Items)
        //            {
        //                string[] distributionItem = distributionItems.Split(';');
        //                IObjectSpace os = Application.CreateObjectSpace();
        //                Items item = os.FindObject<Items>(CriteriaOperator.Parse("[Oid]='" + distributionItem[0] + "'"));
        //                if (item != null)
        //                {
        //                    Requisition newobj = os.CreateObject<Requisition>();
        //                    newobj.Item = os.GetObject<Items>(item);
        //                    if (item.Vendor != null)
        //                    {
        //                        newobj.Vendor = newobj.Item.Vendor;
        //                    }
        //                    if (item.Manufacturer != null)
        //                    {
        //                        newobj.Manufacturer = newobj.Item.Manufacturer;
        //                    }
        //                    if (item.UnitPrice != 0)
        //                    {
        //                        newobj.UnitPrice = newobj.Item.UnitPrice;
        //                    }
        //                    if (item.VendorCatName != null)
        //                    {
        //                        newobj.Catalog = newobj.Item.VendorCatName;
        //                    }
        //                    int totqty = 0;
        //                    if (distributionItem[3] != null && distributionItem[3].Length > 0)
        //                    {
        //                        newobj.TotalItems = Convert.ToInt32(distributionItem[3].ToString());
        //                        totqty = Convert.ToInt32(distributionItem[3].ToString());
        //                        newobj.itemreceived = totqty + " of " + totqty;
        //                    }
        //                    newobj.ReceiveID = temprc;
        //                    newobj.Status = Requisition.TaskStatus.Received;
        //                    if (totqty != 0)
        //                    {
        //                        for (int i = 1; i <= totqty; i++)
        //                        {
        //                            Distribution distribution = os.CreateObject<Distribution>();
        //                            distribution.Item = item;
        //                            distribution.Vendor = item.Vendor;
        //                            if (!string.IsNullOrEmpty(distributionItem[2]))
        //                            {
        //                                distribution.ExpiryDate = Convert.ToDateTime(distributionItem[2]);
        //                            }

        //                            CriteriaOperator ltcriteria = CriteriaOperator.Parse("Max(SUBSTRING(LT, 2))");
        //                            string templt = (Convert.ToInt32(((XPObjectSpace)os).Session.Evaluate(typeof(Distribution), ltcriteria, null)) + 1).ToString();
        //                            var curdate = DateTime.Now.ToString("yy");
        //                            if (templt != "1")
        //                            {
        //                                var predate = templt.Substring(0, 2);
        //                                if (predate == curdate)
        //                                {
        //                                    templt = "LT" + templt;
        //                                }
        //                                else
        //                                {
        //                                    templt = "LT" + curdate + "0001";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                templt = "LT" + curdate + "0001";
        //                            }
        //                            distribution.LT = templt;
        //                            distribution.TotalItems = totqty;
        //                            distribution.DistributionDate = receivedate;
        //                            distribution.DistributedBy = os.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
        //                            distribution.VendorLT = distributionItem[4].ToString();
        //                            distribution.ReceiveID = temprc;
        //                            distribution.ReceiveCount = temprc + "-" + i;
        //                            distribution.itemreceived = i + " of " + totqty;
        //                            distribution.OrderQty = totqty;
        //                            distribution.GivenBy = ((XPObjectSpace)(os)).Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
        //                            distribution.ReceivedBy = os.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
        //                            distribution.ReceiveDate = receivedate;
        //                            distribution.NumItemCode = itemsltno.Items.Count;

        //                            if (!string.IsNullOrEmpty(distributionItem[1]))
        //                            {
        //                                ICMStorage storage = os.FindObject<ICMStorage>(CriteriaOperator.Parse("[storage]='" + distributionItem[1] + "'"));
        //                                if (storage != null)
        //                                {
        //                                    distribution.Storage = storage;
        //                                }
        //                                else
        //                                {
        //                                    ICMStorage createdStorage = os.CreateObject<ICMStorage>();
        //                                    createdStorage.storage = distributionItem[1];
        //                                    createdStorage.Location = "N/A";
        //                                    distribution.Storage = createdStorage;
        //                                }
        //                            }
        //                            distribution.Status = Distribution.LTStatus.PendingConsume;
        //                            if (distribution.ExpiryDate != null && distribution.ExpiryDate <= DateTime.Now.AddDays(7))
        //                            {
        //                                IObjectSpace space = Application.CreateObjectSpace(typeof(ICMAlert));
        //                                ICMAlert objdisp = space.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Expiry Alert - " + distribution.LT));
        //                                if (objdisp == null)
        //                                {
        //                                    ICMAlert obj1 = space.CreateObject<ICMAlert>();
        //                                    obj1.Subject = "Expiry Alert - " + distribution.LT;
        //                                    obj1.StartDate = DateTime.Now;
        //                                    obj1.DueDate = DateTime.Now.AddDays(7);
        //                                    obj1.RemindIn = TimeSpan.FromMinutes(5);
        //                                    obj1.Description = "Nice";
        //                                    space.CommitChanges();
        //                                }
        //                                if (distribution.ExpiryDate <= DateTime.Now)
        //                                {
        //                                    distribution.Status = Distribution.LTStatus.PendingDispose;
        //                                }
        //                            }
        //                            os.CommitChanges();
        //                        }
        //                        if (item.StockQty > item.AlertQty)
        //                        {
        //                            IObjectSpace objspace = Application.CreateObjectSpace();
        //                            IList<ICMAlert> alertlist = objspace.GetObjects<ICMAlert>(CriteriaOperator.Parse("[Subject] = ?", "Low Stock - " + item.items + "(" + item.ItemCode + ")"));
        //                            if (alertlist != null)
        //                            {
        //                                foreach (ICMAlert objitem in alertlist)
        //                                {
        //                                    objitem.AlarmTime = null;
        //                                    objitem.RemindIn = null;
        //                                }
        //                                objspace.CommitChanges();
        //                            }
        //                        }
        //                        else
        //                        {
        //                            IObjectSpace objspace1 = Application.CreateObjectSpace();
        //                            ICMAlert objdisp1 = objspace1.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Low Stock - " + item.items + "(" + item.ItemCode + ")"));
        //                            if (objdisp1 == null)
        //                            {
        //                                IObjectSpace space = Application.CreateObjectSpace(typeof(ICMAlert));
        //                                ICMAlert obj1 = space.CreateObject<ICMAlert>();
        //                                obj1.Subject = "Low Stock - " + item.items + "(" + item.ItemCode + ")";
        //                                obj1.StartDate = DateTime.Now;
        //                                obj1.DueDate = DateTime.Now.AddDays(7);
        //                                obj1.RemindIn = TimeSpan.FromMinutes(5);
        //                                obj1.Description = "Nice";
        //                                space.CommitChanges();
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            NotificationsModule module = this.Application.Modules.FindModule<NotificationsModule>();
        //            module.ShowNotificationsWindow = false;
        //            module.NotificationsService.Refresh();
        //            ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
        //            foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
        //            {
        //                if (parent.Id == "ICM")
        //                {
        //                    foreach (ChoiceActionItem child in parent.Items)
        //                    {
        //                        if (child.Id == "Operations")
        //                        {
        //                            foreach (ChoiceActionItem subchild in child.Items)
        //                            {
        //                                if (subchild.Id == "Receive Order")
        //                                {
        //                                    IObjectSpace objectSpace = Application.CreateObjectSpace();
        //                                    CollectionSource cs = new CollectionSource(objectSpace, typeof(Requisition));
        //                                    cs.Criteria["FilterPOID"] = CriteriaOperator.Parse("[Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived'");
        //                                    List<string> listpoid = new List<string>();
        //                                    foreach (Requisition reqobjvendor in cs.List)
        //                                    {
        //                                        if (!listpoid.Contains(reqobjvendor.POID.POID))
        //                                        {
        //                                            listpoid.Add(reqobjvendor.POID.POID);
        //                                        }
        //                                    }
        //                                    var count = listpoid.Count;
        //                                    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
        //                                    if (count > 0)
        //                                    {
        //                                        subchild.Caption = cap[0] + " (" + count + ")";
        //                                    }
        //                                    else
        //                                    {
        //                                        subchild.Caption = cap[0];
        //                                    }
        //                                }
        //                                else if (subchild.Id == "DistributionItem")
        //                                {
        //                                    IObjectSpace objectSpace = Application.CreateObjectSpace();
        //                                    var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("[LT] Is Null"));
        //                                    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
        //                                    int intReceive = 0;
        //                                    CriteriaOperator criteria = CriteriaOperator.Parse("[LT] Is Null");
        //                                    IList<Distribution> req = ObjectSpace.GetObjects<Distribution>(criteria);
        //                                    string[] ReceiveID = new string[req.Count];
        //                                    foreach (Distribution item in req)
        //                                    {
        //                                        if (!ReceiveID.Contains(item.ReceiveID))
        //                                        {
        //                                            ReceiveID[intReceive] = item.ReceiveID;
        //                                            intReceive = intReceive + 1;
        //                                        }
        //                                    }
        //                                    if (count > 0)
        //                                    {
        //                                        subchild.Caption = cap[0] + " (" + intReceive + ")";
        //                                    }
        //                                    else
        //                                    {
        //                                        subchild.Caption = cap[0];
        //                                    }
        //                                }
        //                                else if (subchild.Id == "DisposalItems")
        //                                {
        //                                    IObjectSpace objectSpace = Application.CreateObjectSpace();
        //                                    //var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] = 'PendingDispose' OR [Status] = 'PendingConsume') And [ExpiryDate] <= ?", DateTime.Now));
        //                                    var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] = 3 OR [Status] = 1) And [ExpiryDate] <= ?", DateTime.Now));
        //                                    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
        //                                    if (count > 0)
        //                                    {
        //                                        subchild.Caption = cap[0] + " (" + count + ")";
        //                                    }
        //                                    else
        //                                    {
        //                                        subchild.Caption = cap[0];
        //                                    }
        //                                }
        //                                else if (subchild.Id == "VendorReagentCertificate")
        //                                {
        //                                    IObjectSpace objectSpace = Application.CreateObjectSpace();
        //                                    var count = objectSpace.GetObjectsCount(typeof(VendorReagentCertificate), CriteriaOperator.Parse("[LoadedDate] IS NULL AND [LoadedBy] IS NULL"));
        //                                    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
        //                                    if (count > 0)
        //                                    {
        //                                        subchild.Caption = cap[0] + " (" + count + ")";
        //                                    }
        //                                    else
        //                                    {
        //                                        subchild.Caption = cap[0];
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        else if (child.Id == "Alert")
        //                        {
        //                            foreach (ChoiceActionItem subchild in child.Items)
        //                            {
        //                                if (subchild.Id == "Stock Alert")
        //                                {
        //                                    IObjectSpace objectSpace = Application.CreateObjectSpace();
        //                                    var count = objectSpace.GetObjectsCount(typeof(Items), CriteriaOperator.Parse("[StockQty] <= [AlertQty]"));
        //                                    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
        //                                    if (count > 0)
        //                                    {
        //                                        subchild.Caption = cap[0] + " (" + count + ")";
        //                                    }
        //                                    else
        //                                    {
        //                                        subchild.Caption = cap[0];
        //                                    }
        //                                }
        //                                else if (subchild.Id == "Expiration Alert")
        //                                {
        //                                    DateTime TodayDate = DateTime.Now;
        //                                    TodayDate = TodayDate.AddDays(7);
        //                                    IObjectSpace objectSpace = Application.CreateObjectSpace();
        //                                    var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] == 3 OR [Status] == 1) And [ExpiryDate] <= ?", TodayDate));
        //                                    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
        //                                    if (count > 0)
        //                                    {
        //                                        subchild.Caption = cap[0] + " (" + count + ")";
        //                                    }
        //                                    else
        //                                    {
        //                                        subchild.Caption = cap[0];
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            IObjectSpace savedListViewObjectSpace = Application.CreateObjectSpace();
        //            CollectionSource collectionSource = new CollectionSource(savedListViewObjectSpace, typeof(Items));
        //            ListView itemListview = Application.CreateListView("Items_ListView", collectionSource, true);
        //            Frame.SetView(itemListview);
        //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages", "ItemsSaved"), InformationType.Success, 3000, InformationPosition.Top);
        //        }
        //        else
        //        {
        //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "shippingnull"), InformationType.Error, timer.Seconds, InformationPosition.Top);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Application.ShowViewStrategy.ShowMessage(ex.Message);
        //    }
        //} 
        #endregion

        // private void ImportFileAction_Execute(object sender, DialogControllerAcceptingEventArgs e)
        private void ImportFileAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {

                ResourceManager rmEnglish = new ResourceManager("Resources.LocalizeResourcesEnglish", Assembly.Load("App_GlobalResources"));
                ResourceManager rmChinese = new ResourceManager("Resources.LocalizeResourcesChinese", Assembly.Load("App_GlobalResources"));
                //ItemsFileUpload itemsFile = (ItemsFileUpload)e.CurrentObject;
                ItemsFileUpload itemsFile = (ItemsFileUpload)e.PopupWindowViewCurrentObject;
                if (itemsFile.InputFile != null && itemsFile.InputFile.FileName != null)
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
                    if (dt != null && dt.Rows.Count > 1)
                    {
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
                        if (itemsltno.Items == null)
                        {
                            itemsltno.Items = new List<string>();
                        }

                        //objectspace change
                        IObjectSpace cloneitemObjectSpace = Application.CreateObjectSpace();
                        CollectionSource source = new CollectionSource(cloneitemObjectSpace, typeof(Items));
                        source.Criteria["filter"] = CriteriaOperator.Parse("IsNullOrEmpty([ItemCode])");
                        ListView cloneItemListView = Application.CreateListView("Items_ListView_Copy", source, true);
                        Frame.SetView(cloneItemListView);

                        List<Items> lstItems = new List<Items>();
                        foreach (DataRow row in dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(c => c is DBNull)))
                        {
                            var isEmpty = row.ItemArray.All(c => c is DBNull);
                            if (!isEmpty)
                            {
                                List<string> errorlist = new List<string>();
                                DateTime dateTime;
                                /*Item Name */
                                if (dt.Columns.Contains(rmChinese.GetString("ItemName")) && !row.IsNull(rmChinese.GetString("ItemName")))
                                {
                                    strItemName = row[rmChinese.GetString("ItemName")].ToString().Trim();
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("ItemName")) && !row.IsNull(rmEnglish.GetString("ItemName")))
                                {
                                    strItemName = row[rmEnglish.GetString("ItemName")].ToString().Trim();
                                }
                                else
                                {
                                    errorlist.Add("Item Name");
                                }
                                if (strItemName.Contains("'"))
                                {
                                    if (strItemName.EndsWith("'"))
                                    {
                                        strItemName = strItemName.Replace("'", "'+'''");
                                    }
                                    else
                                    if (strItemName.StartsWith("'"))
                                    {
                                        strItemName = strItemName.Replace("'", "'''+'");
                                    }
                                    else
                                    {
                                        strItemName = strItemName.Replace("'", "'+''''+'");
                                    }
                                }

                                /*Vendor */
                                if (dt.Columns.Contains(rmChinese.GetString("Vendor")) && !row.IsNull(rmChinese.GetString("Vendor")))
                                {
                                    strVendorName = row[rmChinese.GetString("Vendor")].ToString().Trim();
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("Vendor")) && !row.IsNull(rmEnglish.GetString("Vendor")))
                                {
                                    strVendorName = row[rmEnglish.GetString("Vendor")].ToString().Trim();
                                }
                                else
                                {
                                    errorlist.Add("Vendor");
                                }
                                if (strVendorName.Contains("'"))
                                {
                                    if (strVendorName.EndsWith("'"))
                                    {
                                        strVendorName = strVendorName.Replace("'", "'+'''");
                                    }
                                    else
                                    if (strVendorName.StartsWith("'"))
                                    {
                                        strVendorName = strVendorName.Replace("'", "'''+'");
                                    }
                                    else
                                    {
                                        strVendorName = strVendorName.Replace("'", "'+''''+'");
                                    }
                                }

                                /*Specification */
                                if (dt.Columns.Contains(rmChinese.GetString("Specification")) && !row.IsNull(rmChinese.GetString("Specification")))
                                {
                                    strSpecification = row[rmChinese.GetString("Specification")].ToString().Trim();
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("Specification")) && !row.IsNull(rmEnglish.GetString("Specification")))
                                {
                                    strSpecification = row[rmEnglish.GetString("Specification")].ToString().Trim();
                                }
                                else
                                {
                                    errorlist.Add("Specification");
                                }
                                if (strSpecification.Contains("'"))
                                {
                                    if (strSpecification.EndsWith("'"))
                                    {
                                        strSpecification = strSpecification.Replace("'", "'+'''");
                                    }
                                    else
                                    if (strSpecification.StartsWith("'"))
                                    {
                                        strSpecification = strSpecification.Replace("'", "'''+'");
                                    }
                                    else
                                    {
                                        strSpecification = strSpecification.Replace("'", "'+''''+'");
                                    }
                                }

                                /*Catalog#*/
                                if (dt.Columns.Contains(rmChinese.GetString("Catalog#")) && !row.IsNull(rmChinese.GetString("Catalog#")))
                                {
                                    strVendorCatName = row[rmChinese.GetString("Catalog#")].ToString().Trim();
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("Catalog#")) && !row.IsNull(rmEnglish.GetString("Catalog#")))
                                {
                                    strVendorCatName = row[rmEnglish.GetString("Catalog#")].ToString().Trim();
                                }
                                else
                                {
                                    errorlist.Add("Catalog#");
                                }

                                /*PackUnit*/
                                if (dt.Columns.Contains(rmChinese.GetString("PackUnits")) && !row.IsNull(rmChinese.GetString("PackUnits")))
                                {
                                    strPackUnits = row[rmChinese.GetString("PackUnits")].ToString().Trim();
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("PackUnits")) && !row.IsNull(rmEnglish.GetString("PackUnits")))
                                {
                                    strPackUnits = row[rmEnglish.GetString("PackUnits")].ToString().Trim();
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(strSpecification))
                                    {
                                        if (strSpecification.ToLower().Contains("case"))
                                        {
                                            strPackUnits = "Case";
                                        }
                                        else
                                        if (strSpecification.ToLower().Contains("roll"))
                                        {
                                            strPackUnits = "Roll";
                                        }
                                        else
                                        if (strSpecification.ToLower().Contains("box"))
                                        {
                                            strPackUnits = "Box";
                                        }
                                        else
                                        {
                                            strPackUnits = "Pack";
                                        }
                                    }
                                    else
                                    {
                                        strPackUnits = "Pack";
                                    }
                                    //errorlist.Add("PackUnits");
                                }

                                ///*ItemUnit */
                                //if (dt.Columns.Contains(rmChinese.GetString("ItemUnit")) && !row.IsNull(rmChinese.GetString("ItemUnit")))
                                //{
                                //    strItemUnit = row[rmChinese.GetString("ItemUnit")].ToString();
                                //}
                                //else if (dt.Columns.Contains(rmEnglish.GetString("ItemUnit")) && !row.IsNull(rmEnglish.GetString("ItemUnit")))
                                //{
                                //    strItemUnit = row[rmEnglish.GetString("ItemUnit")].ToString();
                                //}
                                //else
                                //{
                                //    //errorlist.Add("ItemUnit");
                                //    strItemUnit = "1";
                                //}

                                ///*UnitPrice */
                                //if (dt.Columns.Contains(rmChinese.GetString("UnitPrice")) && !row.IsNull(rmChinese.GetString("UnitPrice")))
                                //{
                                //    strUnitPrice = row[rmChinese.GetString("UnitPrice")].ToString();
                                //}
                                //else if (dt.Columns.Contains(rmEnglish.GetString("UnitPrice")) && !row.IsNull(rmEnglish.GetString("UnitPrice")))
                                //{
                                //    strUnitPrice = row[rmEnglish.GetString("UnitPrice")].ToString();
                                //}
                                //else
                                //{
                                //    //errorlist.Add("UnitPrice");
                                //    strUnitPrice = "1";
                                //}

                                ///*StockQty */
                                //if (dt.Columns.Contains(rmChinese.GetString("StockQty")) && !row.IsNull(rmChinese.GetString("StockQty")))
                                //{
                                //    strStockQty = row[rmChinese.GetString("StockQty")].ToString();
                                //}
                                //else if (dt.Columns.Contains(rmEnglish.GetString("StockQty")) && !row.IsNull(rmEnglish.GetString("StockQty")))
                                //{
                                //    strStockQty = row[rmEnglish.GetString("StockQty")].ToString();
                                //}
                                //else
                                //{
                                //    errorlist.Add("StockQty");
                                //}


                                //if (dt.Columns.Contains(rmChinese.GetString("Grade")) && !row.IsNull(rmChinese.GetString("Grade")))
                                //{
                                //    strgrade = row[rmChinese.GetString("Grade")].ToString();
                                //}
                                //else if (dt.Columns.Contains(rmEnglish.GetString("Grade")) && !row.IsNull(rmEnglish.GetString("Grade")))
                                //{
                                //    strgrade = row[rmEnglish.GetString("Grade")].ToString();
                                //}
                                //else
                                //{
                                //    strgrade = string.Empty;
                                //}

                                /*Department*/
                                //if (dt.Columns.Contains(rmChinese.GetString("Department")) && !row.IsNull(rmChinese.GetString("Department")))
                                //{
                                //    strdepartment = row[rmChinese.GetString("Department")].ToString();
                                //}
                                //else if (dt.Columns.Contains(rmEnglish.GetString("Department")) && !row.IsNull(rmEnglish.GetString("Department")))
                                //{
                                //    strdepartment = row[rmEnglish.GetString("Department")].ToString();
                                //}
                                //else
                                //{
                                //    strdepartment = string.Empty;
                                //}


                                ///*Comment */
                                //if (dt.Columns.Contains(rmChinese.GetString("Comment")) && !row.IsNull(rmChinese.GetString("Comment")))
                                //{
                                //    strcomment = row[rmChinese.GetString("Comment")].ToString();
                                //}
                                //else if (dt.Columns.Contains(rmEnglish.GetString("Comment")) && !row.IsNull(rmEnglish.GetString("Comment")))
                                //{
                                //    strcomment = row[rmEnglish.GetString("Comment")].ToString();
                                //}
                                //else
                                //{
                                //    strcomment = string.Empty;
                                //}

                                /*Manufacture */
                                if (dt.Columns.Contains(rmChinese.GetString("Manufacturer")) && !row.IsNull(rmChinese.GetString("Manufacturer")))
                                {
                                    strmanufacture = row[rmChinese.GetString("Manufacturer")].ToString().Trim();
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("Manufacturer")) && !row.IsNull(rmEnglish.GetString("Manufacturer")))
                                {
                                    strmanufacture = row[rmEnglish.GetString("Manufacturer")].ToString().Trim();
                                }
                                else
                                {
                                    strmanufacture = string.Empty;
                                }
                                if (strmanufacture.Contains("'"))
                                {
                                    if (strmanufacture.EndsWith("'"))
                                    {
                                        strmanufacture = strmanufacture.Replace("'", "'+'''");
                                    }
                                    else
                                    if (strmanufacture.StartsWith("'"))
                                    {
                                        strmanufacture = strmanufacture.Replace("'", "'''+'");
                                    }
                                    else
                                    {
                                        strmanufacture = strmanufacture.Replace("'", "'+''''+'");
                                    }
                                }


                                //if (dt.Columns.Contains(rmChinese.GetString("MfritemName")) && !row.IsNull(rmChinese.GetString("MfritemName")))
                                //{
                                //    strMfritemName = row[rmChinese.GetString("MfritemName")].ToString();
                                //}
                                //else if (dt.Columns.Contains(rmEnglish.GetString("MfritemName")) && !row.IsNull(rmEnglish.GetString("MfritemName")))
                                //{
                                //    strMfritemName = row[rmEnglish.GetString("MfritemName")].ToString();
                                //}
                                //else
                                //{
                                //    strMfritemName = string.Empty;
                                //}

                                //if (dt.Columns.Contains(rmChinese.GetString("ItemDescription")) && !row.IsNull(rmChinese.GetString("ItemDescription")))
                                //{
                                //    strItemDescription = row[rmChinese.GetString("ItemDescription")].ToString();
                                //}
                                //else if (dt.Columns.Contains(rmEnglish.GetString("ItemDescription")) && !row.IsNull(rmEnglish.GetString("ItemDescription")))
                                //{
                                //    strItemDescription = row[rmEnglish.GetString("ItemDescription")].ToString();
                                //}
                                //else
                                //{
                                //    strItemDescription = string.Empty;
                                //}


                                //if (dt.Columns.Contains(rmChinese.GetString("MfrcatNum")) && !row.IsNull(rmChinese.GetString("MfrcatNum")))
                                //{
                                //    strMfrcatNum = row[rmChinese.GetString("MfrcatNum")].ToString();
                                //}
                                //else if (dt.Columns.Contains(rmEnglish.GetString("MfrcatNum")) && !row.IsNull(rmEnglish.GetString("MfrcatNum")))
                                //{
                                //    strMfrcatNum = row[rmEnglish.GetString("MfrcatNum")].ToString();
                                //}
                                //else
                                //{
                                //    strMfrcatNum = string.Empty;
                                //}


                                //if (dt.Columns.Contains(rmChinese.GetString("VendorItemName")) && !row.IsNull(rmChinese.GetString("VendorItemName")))
                                //{
                                //    strVendorItemName = row[rmChinese.GetString("VendorItemName")].ToString();
                                //}
                                //else if (dt.Columns.Contains(rmEnglish.GetString("VendorItemName")) && !row.IsNull(rmEnglish.GetString("VendorItemName")))
                                //{
                                //    strVendorItemName = row[rmEnglish.GetString("VendorItemName")].ToString();
                                //}
                                //else
                                //{
                                //    strVendorItemName = string.Empty;
                                //}

                                //if (dt.Columns.Contains(rmChinese.GetString("VendorCatalog")) && !row.IsNull(rmChinese.GetString("VendorCatalog")))
                                //{
                                //    strVendorCatalog = row[rmChinese.GetString("VendorCatalog")].ToString();
                                //}
                                //else if (dt.Columns.Contains(rmEnglish.GetString("VendorCatalog")) && !row.IsNull(rmEnglish.GetString("VendorCatalog")))
                                //{
                                //    strVendorCatalog  = row[rmEnglish.GetString("VendorCatalog")].ToString();
                                //}
                                //else
                                //{
                                //    strVendorCatalog = string.Empty;
                                //}

                                //if (dt.Columns.Contains(rmChinese.GetString("VendorCatName")) && !row.IsNull(rmChinese.GetString("VendorCatName")))
                                //{
                                //    strVendorCatalogName = row[rmChinese.GetString("VendorCatName")].ToString();
                                //}
                                //else if (dt.Columns.Contains(rmEnglish.GetString("VendorCatName")) && !row.IsNull(rmEnglish.GetString("VendorCatName")))
                                //{
                                //    strVendorCatalogName = row[rmEnglish.GetString("VendorCatName")].ToString();
                                //}
                                //else
                                //{
                                //    errorlist.Add("strVendorCatalogName");
                                //}

                                ///*AmountUnit */
                                //if (dt.Columns.Contains(rmChinese.GetString("AmountUnit")) && !row.IsNull(rmChinese.GetString("AmountUnit")))
                                //{
                                //    strAmountUnits = row[rmChinese.GetString("AmountUnit")].ToString();
                                //}
                                //else if (dt.Columns.Contains(rmEnglish.GetString("AmountUnit")) && !row.IsNull(rmEnglish.GetString("AmountUnit")))
                                //{
                                //    strAmountUnits = row[rmEnglish.GetString("AmountUnit")].ToString();
                                //}
                                //else
                                //{
                                //    strAmountUnits = string.Empty;
                                //}

                                /*Category */

                                if (dt.Columns.Contains(rmChinese.GetString("Category")) && !row.IsNull(rmChinese.GetString("Category")))
                                {
                                    strCategory = row[rmChinese.GetString("Category")].ToString().Trim();
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("Category")) && !row.IsNull(rmEnglish.GetString("Category")))
                                {
                                    strCategory = row[rmEnglish.GetString("Category")].ToString().Trim();
                                }
                                else
                                {
                                    strCategory = string.Empty;
                                }


                                ///*Grade */
                                //if (dt.Columns.Contains(rmChinese.GetString("Grade")) && !row.IsNull(rmChinese.GetString("Grade")))
                                //{
                                //    strgrade = row[rmChinese.GetString("Grade")].ToString();
                                //}
                                //else if (dt.Columns.Contains(rmEnglish.GetString("Grade")) && !row.IsNull(rmEnglish.GetString("Grade")))
                                //{
                                //    strgrade = row[rmEnglish.GetString("Grade")].ToString();
                                //}
                                //else
                                //{
                                //    strgrade = string.Empty;
                                //}



                                ////if ((dt.Columns.Contains(rmChinese.GetString("IsVendorLT")) && !row.IsNull(rmChinese.GetString("IsVendorLT")) && row[rmChinese.GetString("IsVendorLT")].GetType() != typeof(bool))
                                ////    || (dt.Columns.Contains(rmEnglish.GetString("IsVendorLT")) && !row.IsNull(rmEnglish.GetString("IsVendorLT")) && row[rmEnglish.GetString("IsVendorLT")].GetType() != typeof(bool)))
                                ////{
                                ////    errorlist.Add("IsVendorLT");
                                ////}

                                ////if ((dt.Columns.Contains(rmChinese.GetString("IsLabLT")) && !row.IsNull(rmChinese.GetString("IsLabLT")) && row[rmChinese.GetString("IsLabLT")].GetType() != typeof(bool))
                                ////    || (dt.Columns.Contains(rmEnglish.GetString("IsLabLT")) && !row.IsNull(rmEnglish.GetString("IsLabLT")) && row[rmEnglish.GetString("IsLabLT")].GetType() != typeof(bool)))
                                ////{
                                ////    errorlist.Add("IsLabLT");
                                ////}

                                ////if ((dt.Columns.Contains(rmChinese.GetString("IsToxic")) && !row.IsNull(rmChinese.GetString("IsToxic")) && row[rmChinese.GetString("IsToxic")].GetType() != typeof(bool))
                                ////   || (dt.Columns.Contains(rmEnglish.GetString("IsToxic")) && !row.IsNull(rmEnglish.GetString("IsToxic")) && row[rmEnglish.GetString("IsToxic")].GetType() != typeof(bool)))
                                ////{
                                ////    errorlist.Add("IsToxic");
                                ////}

                                //if ((dt.Columns.Contains(rmChinese.GetString("RetireDate")) && !row.IsNull(rmChinese.GetString("RetireDate")) &&
                                //    !DateTime.TryParseExact(row[rmChinese.GetString("RetireDate")].ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dateTime) && row[rmChinese.GetString("RetireDate")].GetType() != typeof(DateTime))
                                //   || (dt.Columns.Contains(rmEnglish.GetString("RetireDate")) && !row.IsNull(rmEnglish.GetString("RetireDate")) &&
                                //   !DateTime.TryParseExact(row[rmEnglish.GetString("RetireDate")].ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dateTime) && row[rmEnglish.GetString("RetireDate")].GetType() != typeof(DateTime)))
                                //{
                                //    errorlist.Add("RetireDate");
                                //}

                                /*ExpiryDate*/
                                //var obj2 = dt.Columns.Contains(rmEnglish.GetString("ExpiryDate"));
                                //var obj3 = row.IsNull(rmEnglish.GetString("ExpiryDate"));

                                //if ((dt.Columns.Contains(rmChinese.GetString("ExpiryDate")) && !row.IsNull(rmChinese.GetString("ExpiryDate")) &&
                                //    !DateTime.TryParseExact(row[rmChinese.GetString("ExpiryDate")].ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dateTime) && row[rmChinese.GetString("ExpiryDate")].GetType() != typeof(DateTime))
                                //  || (dt.Columns.Contains(rmEnglish.GetString("ExpiryDate")) && !row.IsNull(rmEnglish.GetString("ExpiryDate")) &&
                                //  !DateTime.TryParseExact(row[rmEnglish.GetString("ExpiryDate")].ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dateTime) && row[rmEnglish.GetString("ExpiryDate")].GetType() != typeof(DateTime)))
                                //{
                                //    errorlist.Add("ExpiryDate");
                                //}

                                if (errorlist.Count == 0)
                                {
                                    Items findItem = ObjectSpace.FindObject<Items>(CriteriaOperator.Parse("[items]='" + strItemName + "' And [Vendor.Vendor] ='" + strVendorName + "' And [VendorCatName] ='" + strVendorCatName + "'  AND [Specification] = '" + strSpecification + "'"));
                                    if (findItem == null)
                                    {
                                        if (lstItems.FirstOrDefault(i => i.items == strItemName && i.Vendor != null && i.Vendor.Vendor == strVendorName && i.VendorCatName == strVendorCatName && i.Specification == strSpecification) == null)
                                        {
                                            string strVendorlt = string.Empty;
                                            string strStorage = string.Empty;
                                            doubleunitprice = 1;
                                            doubleAmount = 0;
                                            intstockqty = 0;
                                            intitemunit = 1;
                                            intalert = 0;
                                            intdayremain = 0;
                                            Nullable<DateTime> dateExpiry = null;
                                            Nullable<DateTime> dateRetire = null;
                                            bool isToxic = false;
                                            bool isVendorLT = false;
                                            bool isLabLT = false;
                                            bool isReSale = false;

                                            Items items = ObjectSpace.CreateObject<Items>();
                                            items.items = strItemName;
                                            items.Specification = strSpecification;
                                            items.Comment = strcomment;
                                            items.VendorCatName = strVendorCatName;
                                            //items.bolExportedItems = true;
                                            // items.ItemUnit = Convert.ToInt16(strItemUnit);
                                            // items.UnitPrice = Convert.ToDouble(strUnitPrice);                                  

                                            if (strVendorName != string.Empty)
                                            {
                                                Vendors vendor = ObjectSpace.FindObject<Vendors>(CriteriaOperator.Parse("[Vendor]='" + strVendorName + "'"));
                                                if (vendor != null)
                                                {
                                                    items.Vendor = vendor;
                                                }
                                                else
                                                {
                                                    Vendors createVendor = ObjectSpace.CreateObject<Vendors>();
                                                    createVendor.Vendor = strVendorName;
                                                    items.Vendor = createVendor;
                                                    //packageunits.vendors.Add(createVendor);
                                                }
                                            }

                                            if (strgrade != string.Empty)
                                            {
                                                Grades grade = ObjectSpace.FindObject<Grades>(CriteriaOperator.Parse("[Grade]='" + strgrade + "'"));
                                                if (grade != null)
                                                {
                                                    items.Grade = grade;
                                                }
                                                else
                                                {
                                                    Grades createGrade = ObjectSpace.CreateObject<Grades>();
                                                    createGrade.Grade = strgrade;
                                                    items.Grade = createGrade;
                                                }
                                            }


                                            //if (strdepartment != string.Empty)
                                            //{
                                            //    Department dept = ObjectSpace.FindObject<Department>(CriteriaOperator.Parse("[Name]='" + strdepartment + "'"));
                                            //    if (dept != null)
                                            //    {
                                            //        items.Department = dept;
                                            //    }
                                            //    else
                                            //    {
                                            //        Department createDept = ObjectSpace.CreateObject<Department>();
                                            //        createDept.Name = strdepartment;
                                            //        items.Department = createDept;

                                            //    }
                                            //}

                                            if (strmanufacture != string.Empty)
                                            {
                                                Manufacturer manuf = ObjectSpace.FindObject<Manufacturer>(CriteriaOperator.Parse("[ManufacturerName]='" + strmanufacture + "'"));
                                                if (manuf != null)
                                                {
                                                    items.Manufacturer = manuf;
                                                }
                                                else
                                                {
                                                    Manufacturer createmanuf = ObjectSpace.CreateObject<Manufacturer>();
                                                    createmanuf.ManufacturerName = strmanufacture;
                                                    items.Manufacturer = createmanuf;
                                                }
                                            }

                                            if (strAmountUnits != string.Empty)
                                            {
                                                Unit amountUnit = ObjectSpace.FindObject<Unit>(CriteriaOperator.Parse("[UnitName]='" + strAmountUnits + "'"));
                                                if (amountUnit != null)
                                                {
                                                    items.AmountUnit = amountUnit;
                                                }
                                                else
                                                {
                                                    Unit createAmountunit = ObjectSpace.CreateObject<Unit>();
                                                    createAmountunit.UnitName = strAmountUnits;
                                                    items.AmountUnit = createAmountunit;
                                                }
                                            }

                                            if (strPackUnits != string.Empty)
                                            {
                                                Packageunits packageunit = ObjectSpace.FindObject<Packageunits>(CriteriaOperator.Parse("[Option]='" + strPackUnits + "'"));
                                                if (packageunit != null)
                                                {
                                                    items.Unit = packageunit;
                                                }
                                                else
                                                {
                                                    Packageunits createpackageunit = ObjectSpace.CreateObject<Packageunits>();
                                                    createpackageunit.Option = strPackUnits;
                                                    items.Unit = createpackageunit;
                                                }
                                            }

                                            //if (strCategory != string.Empty)
                                            //{
                                            //    ICMCategory category = ObjectSpace.FindObject<ICMCategory>(CriteriaOperator.Parse("[category]='" + strCategory + "'"));
                                            //    if (category != null)
                                            //    {
                                            //        items.Category = category;
                                            //    }
                                            //    else
                                            //    {
                                            //        ICMCategory createCategory = ObjectSpace.CreateObject<ICMCategory>();
                                            //        createCategory.category = strCategory;
                                            //        items.Category = createCategory;
                                            //    }
                                            //}
                                            if (strCategory != string.Empty)
                                            {
                                                Category category = ObjectSpace.FindObject<Category>(CriteriaOperator.Parse("[category]='" + strCategory + "'"));
                                                if (category != null)
                                                {
                                                    items.Category = category;
                                                }
                                                else
                                                {
                                                    Category createCategory = ObjectSpace.CreateObject<Category>();
                                                    createCategory.category = strCategory;
                                                    items.Category = createCategory;
                                                }
                                            }

                                            //if (dt.Columns.Contains(rmChinese.GetString("IsToxic")) && !row.IsNull(rmChinese.GetString("IsToxic")))
                                            //{
                                            //    isToxic = Convert.ToBoolean(row[rmChinese.GetString("IsToxic")]);
                                            //}
                                            //else if (dt.Columns.Contains(rmEnglish.GetString("IsToxic")) && !row.IsNull(rmEnglish.GetString("IsToxic"))
                                            //{
                                            //    isToxic = Convert.ToBoolean(row[rmEnglish.GetString("IsToxic")]);
                                            //}


                                            //if (dt.Columns.Contains(rmChinese.GetString("IsLabLT")) && !row.IsNull(rmChinese.GetString("IsLabLT")))
                                            //{
                                            //    isLabLT = Convert.ToBoolean(row[rmChinese.GetString("IsLabLT")]);
                                            //}
                                            //else if (dt.Columns.Contains(rmEnglish.GetString("IsLabLT")) && !row.IsNull(rmEnglish.GetString("IsLabLT")))
                                            //{
                                            //    isLabLT = Convert.ToBoolean(row[rmEnglish.GetString("IsLabLT")]);
                                            //}
                                            //items.IsLabLT = isLabLT;

                                            //if (dt.Columns.Contains(rmChinese.GetString("IsVendorLT")) && !row.IsNull(rmChinese.GetString("IsVendorLT")))
                                            //{
                                            //    isVendorLT = Convert.ToBoolean(row[rmChinese.GetString("IsVendorLT")]);
                                            //}
                                            //else if (dt.Columns.Contains(rmEnglish.GetString("IsVendorLT")) && !row.IsNull(rmEnglish.GetString("IsVendorLT")))
                                            //{
                                            //    isVendorLT = Convert.ToBoolean(row[rmEnglish.GetString("IsVendorLT")]);
                                            //}
                                            //items.IsVendorLT = isVendorLT;

                                            if (dt.Columns.Contains(rmChinese.GetString("IsResale")) && !row.IsNull(rmChinese.GetString("IsResale")))
                                            {
                                                isReSale = Convert.ToBoolean(row[rmChinese.GetString("IsResale")]);
                                            }
                                            else if (dt.Columns.Contains(rmEnglish.GetString("IsResale")) && !row.IsNull(rmEnglish.GetString("IsResale")))
                                            {
                                                isReSale = Convert.ToBoolean(row[rmEnglish.GetString("IsResale")]);
                                            }
                                            items.bolResaleItems = isReSale;

                                            if (dt.Columns.Contains(rmChinese.GetString("StockQty")) && !row.IsNull(rmChinese.GetString("StockQty")))
                                            {
                                                intstockqty = Convert.ToInt32(row[rmChinese.GetString("StockQty")]);
                                            }
                                            else if (dt.Columns.Contains(rmEnglish.GetString("StockQty")) && !row.IsNull(rmEnglish.GetString("StockQty")))
                                            {
                                                intstockqty = Convert.ToInt32(row[rmEnglish.GetString("StockQty")]);
                                            }
                                            items.StockQty = intstockqty;

                                            if (dt.Columns.Contains(rmChinese.GetString("AlertQty")) && !row.IsNull(rmChinese.GetString("AlertQty")))
                                            {
                                                intalert = Convert.ToInt32(row[rmChinese.GetString("AlertQty")]);
                                            }
                                            else if (dt.Columns.Contains(rmEnglish.GetString("AlertQty")) && !row.IsNull(rmEnglish.GetString("AlertQty")))
                                            {
                                                intalert = Convert.ToInt32(row[rmEnglish.GetString("AlertQty")]);
                                            }
                                            items.AlertQty = intalert;

                                            //if (dt.Columns.Contains(rmChinese.GetString("RemainderDaysInAdvance")) && !row.IsNull(rmChinese.GetString("RemainderDaysInAdvance")))
                                            //{
                                            //    intdayremain = Convert.ToInt32(row[rmChinese.GetString("RemainderDaysInAdvance")]);
                                            //}
                                            //else if (dt.Columns.Contains(rmEnglish.GetString("RemainderDaysInAdvance")) && !row.IsNull(rmEnglish.GetString("RemainderDaysInAdvance")))
                                            //{
                                            //    intdayremain = Convert.ToInt32(row[rmEnglish.GetString("RemainderDaysInAdvance")]);
                                            //}
                                            //items.ReminderDaysInAdvance = intstockqty;


                                            if (dt.Columns.Contains(rmChinese.GetString("Item/Unit")) && !row.IsNull(rmChinese.GetString("Item/Unit")))
                                            {
                                                intitemunit = Convert.ToInt32(row[rmChinese.GetString("Item/Unit")]);
                                            }
                                            else if (dt.Columns.Contains(rmEnglish.GetString("Item/Unit")) && !row.IsNull(rmEnglish.GetString("Item/Unit")))
                                            {
                                                intitemunit = Convert.ToInt32(row[rmEnglish.GetString("Item/Unit")]);
                                            }
                                            else
                                            {
                                                intitemunit = 1;
                                            }
                                            items.ItemUnit = intitemunit;

                                            if (dt.Columns.Contains(rmChinese.GetString("Amount")) && !row.IsNull(rmChinese.GetString("Amount")))
                                            {
                                                doubleAmount = Convert.ToDouble(row[rmChinese.GetString("Amount")]);
                                            }
                                            else if (dt.Columns.Contains(rmEnglish.GetString("Amount")) && !row.IsNull(rmEnglish.GetString("Amount")))
                                            {
                                                doubleAmount = Convert.ToDouble(row[rmEnglish.GetString("Amount")]);
                                            }
                                            items.Amount = Convert.ToString(doubleAmount);


                                            if (dt.Columns.Contains(rmChinese.GetString("UnitPrice")) && !row.IsNull(rmChinese.GetString("UnitPrice")))
                                            {
                                                string strUnitPrice = string.Empty;
                                                strUnitPrice = Convert.ToString(row[rmChinese.GetString("UnitPrice")]);
                                                if (strUnitPrice.Contains("$"))
                                                {
                                                    strUnitPrice = strUnitPrice.Replace("$", "");
                                                }

                                                doubleunitprice = Convert.ToDouble(strUnitPrice);

                                                if (doubleunitprice < 1)
                                                {
                                                    doubleunitprice = 1;
                                                }
                                            }
                                            else if (dt.Columns.Contains(rmEnglish.GetString("UnitPrice")) && !row.IsNull(rmEnglish.GetString("UnitPrice")))
                                            {
                                                string strUnitPrice = string.Empty;
                                                strUnitPrice = Convert.ToString(row[rmEnglish.GetString("UnitPrice")]);
                                                if (strUnitPrice.Contains("$"))
                                                {
                                                    strUnitPrice = strUnitPrice.Replace("$", "");
                                                }

                                                doubleunitprice = Convert.ToDouble(strUnitPrice);

                                                if (doubleunitprice < 1)
                                                {
                                                    doubleunitprice = 1;
                                                }
                                            }
                                            else
                                            {
                                                doubleunitprice = Convert.ToDouble(1);
                                            }
                                            items.UnitPrice = doubleunitprice;

                                            //if (dt.Columns.Contains(rmChinese.GetString("Vendorlt")) && !row.IsNull(rmChinese.GetString("Vendorlt")))
                                            //{
                                            //    strVendorlt = row[rmChinese.GetString("Vendorlt")].ToString();
                                            //}
                                            //else if (dt.Columns.Contains(rmEnglish.GetString("Vendorlt")) && !row.IsNull(rmEnglish.GetString("Vendorlt")))
                                            //{
                                            //    strVendorlt = row[rmEnglish.GetString("Vendorlt")].ToString();
                                            //}

                                            if (dt.Columns.Contains(rmChinese.GetString("Storage")) && !row.IsNull(rmChinese.GetString("Storage")))
                                            {
                                                strStorage = row[rmChinese.GetString("Storage")].ToString();
                                            }
                                            else if (dt.Columns.Contains(rmEnglish.GetString("Storage")) && !row.IsNull(rmEnglish.GetString("Storage")))
                                            {
                                                strStorage = row[rmEnglish.GetString("Storage")].ToString();
                                            }

                                            if (dt.Columns.Contains(rmChinese.GetString("RetireDate")) && !row.IsNull(rmChinese.GetString("RetireDate")))
                                            {
                                                if (row[rmChinese.GetString("RetireDate")].GetType() == typeof(DateTime))
                                                {
                                                    items.RetireDate = Convert.ToDateTime(row[rmChinese.GetString("RetireDate")]);
                                                }
                                                else if (row[rmChinese.GetString("RetireDate")].GetType() == typeof(string))
                                                {
                                                    string strdate = row[rmChinese.GetString("RetireDate")].ToString();
                                                    if (strdate != string.Empty)
                                                    {
                                                        string[] strdateonly = strdate.Split(' ');
                                                        items.RetireDate = DateTime.ParseExact(strdateonly[0], "MM/dd/yyyy", null);
                                                    }
                                                }
                                            }
                                            else if (dt.Columns.Contains(rmEnglish.GetString("RetireDate")) && !row.IsNull(rmEnglish.GetString("RetireDate")))
                                            {
                                                if (row[rmEnglish.GetString("RetireDate")].GetType() == typeof(DateTime))
                                                {
                                                    items.RetireDate = Convert.ToDateTime(row[rmEnglish.GetString("RetireDate")]);
                                                }
                                                else if (row[rmEnglish.GetString("RetireDate")].GetType() == typeof(string))
                                                {
                                                    string strdate = row[rmEnglish.GetString("RetireDate")].ToString();
                                                    if (strdate != string.Empty)
                                                    {
                                                        string[] strdateonly = strdate.Split(' ');
                                                        items.RetireDate = DateTime.ParseExact(strdateonly[0], "MM/dd/yyyy", null);
                                                    }
                                                }
                                            }


                                            //if (dt.Columns.Contains(rmChinese.GetString("ExpiryDate")) && !row.IsNull(rmChinese.GetString("ExpiryDate")))
                                            //{
                                            //    if (row[rmChinese.GetString("ExpiryDate")].GetType() == typeof(DateTime))
                                            //    {
                                            //        dateExpiry = Convert.ToDateTime(row[rmChinese.GetString("ExpiryDate")]);
                                            //    }
                                            //    else if (row[rmChinese.GetString("ExpiryDate")].GetType() == typeof(string))
                                            //    {
                                            //        string strdate = row[rmChinese.GetString("ExpiryDate")].ToString();
                                            //        if (strdate != string.Empty)
                                            //        {
                                            //            dateExpiry = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
                                            //        }
                                            //    }
                                            //}
                                            //else if (dt.Columns.Contains(rmEnglish.GetString("ExpiryDate")) && !row.IsNull(rmEnglish.GetString("ExpiryDate")))
                                            //{
                                            //    if (row[rmEnglish.GetString("ExpiryDate")].GetType() == typeof(DateTime))
                                            //    {
                                            //        dateExpiry = Convert.ToDateTime(row[rmEnglish.GetString("ExpiryDate")]);
                                            //    }
                                            //    else if (row[rmEnglish.GetString("ExpiryDate")].GetType() == typeof(string))
                                            //    {
                                            //        string strdate = row[rmEnglish.GetString("ExpiryDate")].ToString();
                                            //        if (strdate != string.Empty)
                                            //        {
                                            //            dateExpiry = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
                                            //        }
                                            //    }
                                            //}

                                            //CriteriaOperator ItemCode = CriteriaOperator.Parse("Max(ItemCode)");
                                            //string NewItemCode = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(Items), ItemCode, null)) + 1).ToString();
                                            //if (NewItemCode.Length == 1)
                                            //{
                                            //    NewItemCode = "00" + NewItemCode;
                                            //}
                                            //else if (NewItemCode.Length == 2)
                                            //{
                                            //    NewItemCode = "0" + NewItemCode;
                                            //}
                                            //items.ItemCode = NewItemCode;
                                            //ObjectSpace.CommitChanges();
                                            //ObjectSpace.Refresh();

                                            if (itemsltno.Items != null)
                                            {
                                                if (intstockqty > 0)
                                                {
                                                    string stritem = items.Oid + ";" + strStorage + ";" + intstockqty.ToString();
                                                    itemsltno.Items.Add(stritem);
                                                }
                                            }

                                            // itemsltno.Items.Add(items.Oid + ";" + strStorage + ";" + intstockqty.ToString() + ";");
                                            processItems(items, dt, row, rmChinese, rmEnglish);
                                            ((ListView)View).CollectionSource.Add(items);
                                            lstItems.Add(items);

                                            //itemsltno.Items.Add(items.Oid + ";" + strStorage + ";" + dateExpiry + ";" + intstockqty.ToString() + ";" + strVendorlt);

                                            // processItems(items, dt, row, rmChinese, rmEnglish);
                                            //((ListView)View).CollectionSource.Add(items);

                                            /*Creating Lot Number */

                                            //if (intstockqty > 0)
                                            //{
                                            //    IObjectSpace Space = Application.CreateObjectSpace(typeof(Requisition));
                                            //    CriteriaOperator sam = CriteriaOperator.Parse("Max(SUBSTRING(ReceiveID, 2))");
                                            //    string temprc = (Convert.ToInt32(((XPObjectSpace)Space).Session.Evaluate(typeof(Requisition), sam, null)) + 1).ToString();
                                            //    var curedate = DateTime.Now.ToString("yyMMdd");
                                            //    if (temprc != "1")
                                            //    {
                                            //        var predate = temprc.Substring(0, 6);
                                            //        if (predate == curedate)
                                            //        {
                                            //            temprc = "RC" + temprc;
                                            //        }
                                            //        else
                                            //        {
                                            //            temprc = "RC" + curedate + "01";
                                            //        }
                                            //    }
                                            //    else
                                            //    {
                                            //        temprc = "RC" + curedate + "01";
                                            //    }
                                            //    //strRequistionID = temprc;

                                            //    var receivedate = DateTime.Now;
                                            //    IObjectSpace os = Application.CreateObjectSpace();
                                            //    Items item = os.FindObject<Items>(CriteriaOperator.Parse("[Oid]='" + items.Oid + "'"));
                                            //    if (item != null)
                                            //    {
                                            //        Requisition newobj = os.CreateObject<Requisition>();
                                            //        newobj.Item = os.GetObject<Items>(item);
                                            //        if (item.Vendor != null)
                                            //        {
                                            //            newobj.Vendor = newobj.Item.Vendor;
                                            //        }
                                            //        if (item.Manufacturer != null)
                                            //        {
                                            //            newobj.Manufacturer = newobj.Item.Manufacturer;
                                            //        }
                                            //        if (item.UnitPrice != 0)
                                            //        {
                                            //            newobj.UnitPrice = newobj.Item.UnitPrice;
                                            //        }
                                            //        if (item.VendorCatName != null)
                                            //        {
                                            //            newobj.Catalog = newobj.Item.VendorCatName;
                                            //        }
                                            //        int totqty = 0;
                                            //        //if (distributionItem[3] != null && distributionItem[3].Length > 0)
                                            //        //{
                                            //        newobj.TotalItems = intstockqty;
                                            //        totqty = intstockqty;
                                            //        newobj.ItemReceived = totqty + " of " + totqty;
                                            //        //}
                                            //        newobj.ReceiveID = temprc;
                                            //        newobj.Status = Requisition.TaskStatus.Received;

                                            //        if (totqty != 0)
                                            //        {
                                            //            CriteriaOperator ltcriteria = CriteriaOperator.Parse("Max(SUBSTRING(LT, 2))");
                                            //            //string templt = (Convert.ToInt32(((XPObjectSpace)os).Session.Evaluate(typeof(Distribution), ltcriteria, null)) + 1).ToString();
                                            //            string templt = (((XPObjectSpace)os).Session.Evaluate(typeof(Distribution), ltcriteria, null)).ToString();
                                            //            string[] strPrevlt = templt.Split('_');
                                            //            templt = (Convert.ToUInt32(strPrevlt[0]) + 1).ToString();
                                            //            var curdate = DateTime.Now.ToString("yy");
                                            //            if (templt != "1")
                                            //            {
                                            //                var predate = templt.Substring(0, 2);
                                            //                if (predate == curdate)
                                            //                {
                                            //                    templt = "LT" + templt;
                                            //                }
                                            //                else
                                            //                {
                                            //                    templt = "LT" + curdate + "0001";
                                            //                }
                                            //            }
                                            //            else
                                            //            {
                                            //                templt = "LT" + curdate + "0001";
                                            //            }


                                            //            for (int i = 1; i <= totqty; i++)
                                            //            {
                                            //                Distribution distribution = os.CreateObject<Distribution>();
                                            //                distribution.Item = item;
                                            //                distribution.Vendor = item.Vendor;
                                            //                // if (!string.IsNullOrEmpty(dateExpiry.ToString()))
                                            //                // {
                                            //                //distribution.ExpiryDate = Convert.ToDateTime(dateExpiry);
                                            //                //}
                                            //                //CriteriaOperator ltcriteria = CriteriaOperator.Parse("Max(SUBSTRING(LT, 2))");
                                            //                //string templt = (Convert.ToInt32(((XPObjectSpace)os).Session.Evaluate(typeof(Distribution), ltcriteria, null)) + 1).ToString();
                                            //                //var curdate = DateTime.Now.ToString("yy");
                                            //                //if (templt != "1")
                                            //                //{
                                            //                //    var predate = templt.Substring(0, 2);
                                            //                //    if (predate == curdate)
                                            //                //    {
                                            //                //        templt = "LT" + templt;
                                            //                //    }
                                            //                //    else
                                            //                //    {
                                            //                //        templt = "LT" + curdate + "0001";
                                            //                //    }
                                            //                //}
                                            //                //else
                                            //                //{
                                            //                //    templt = "LT" + curdate + "0001";
                                            //                //}

                                            //                if (totqty == 1)
                                            //                {
                                            //                    distribution.LT = templt;
                                            //                }
                                            //                else if (totqty > 0 && i < 10)
                                            //                {
                                            //                    //distribution.LT = templt +"_" +string.Format("{0:00}",i);
                                            //                    distribution.LT = templt + "_0" + i.ToString();
                                            //                }
                                            //                else
                                            //                {
                                            //                    //distribution.LT = templt + "_0" + string.Format("{0:00}", i);
                                            //                    distribution.LT = templt + "_" + i.ToString();
                                            //                }
                                            //                distribution.TotalItems = totqty;
                                            //                distribution.DistributionDate = DateTime.Now;
                                            //                distribution.DistributedBy = os.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                            //                distribution.VendorLT = strVendorlt;
                                            //                //distribution.ReceiveID = temprc;
                                            //                //// distribution.ReceiveCount = temprc + "-" + i;
                                            //                distribution.ItemReceived = i + " of " + totqty;
                                            //                distribution.OrderQty = totqty;
                                            //                distribution.GivenBy = ((XPObjectSpace)(os)).Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                            //                distribution.ReceivedBy = os.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                            //                distribution.ReceiveDate = DateTime.Now;
                                            //                //distribution.NumItemCode = itemsltno.Items.Count;

                                            //                if (!string.IsNullOrEmpty(strStorage))
                                            //                {
                                            //                    ICMStorage storage = os.FindObject<ICMStorage>(CriteriaOperator.Parse("[storage]='" + strStorage + "'"));
                                            //                    if (storage != null)
                                            //                    {
                                            //                        ////  distribution.Storage = storage;
                                            //                    }
                                            //                    else
                                            //                    {
                                            //                        ICMStorage createdStorage = os.CreateObject<ICMStorage>();
                                            //                        createdStorage.storage = strStorage;
                                            //                        //createdStorage.Location = "N/A";
                                            //                        ////  distribution.Storage = createdStorage;
                                            //                    }
                                            //                }
                                            //                distribution.Status = Distribution.LTStatus.PendingConsume;
                                            //                if (distribution.ExpiryDate != null && distribution.ExpiryDate <= DateTime.Now.AddDays(7))
                                            //                {
                                            //                    IObjectSpace space = Application.CreateObjectSpace(typeof(ICMAlert));
                                            //                    ICMAlert objdisp = space.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Expiry Alert - " + distribution.LT));
                                            //                    if (objdisp == null)
                                            //                    {
                                            //                        ICMAlert obj1 = space.CreateObject<ICMAlert>();
                                            //                        obj1.Subject = "Expiry Alert - " + distribution.LT;
                                            //                        obj1.StartDate = DateTime.Now;
                                            //                        obj1.DueDate = DateTime.Now.AddDays(7);
                                            //                        obj1.RemindIn = TimeSpan.FromMinutes(5);
                                            //                        obj1.Description = "Nice";
                                            //                        space.CommitChanges();
                                            //                    }
                                            //                    if (distribution.ExpiryDate <= DateTime.Now)
                                            //                    {
                                            //                        distribution.Status = Distribution.LTStatus.PendingDispose;
                                            //                    }
                                            //                }
                                            //                os.CommitChanges();
                                            //            }
                                            //            if (item.StockQty > item.AlertQty)
                                            //            {
                                            //                IObjectSpace objspace = Application.CreateObjectSpace();
                                            //                IList<ICMAlert> alertlist = objspace.GetObjects<ICMAlert>(CriteriaOperator.Parse("[Subject] = ?", "Low Stock - " + item.items + "(" + item.ItemCode + ")"));
                                            //                if (alertlist != null)
                                            //                {
                                            //                    foreach (ICMAlert objitem in alertlist)
                                            //                    {
                                            //                        objitem.AlarmTime = null;
                                            //                        objitem.RemindIn = null;
                                            //                    }
                                            //                    objspace.CommitChanges();
                                            //                }
                                            //            }
                                            //            else
                                            //            {
                                            //                IObjectSpace objspace1 = Application.CreateObjectSpace();
                                            //                ICMAlert objdisp1 = objspace1.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Low Stock - " + item.items + "(" + item.ItemCode + ")"));
                                            //                if (objdisp1 == null)
                                            //                {
                                            //                    IObjectSpace space = Application.CreateObjectSpace(typeof(ICMAlert));
                                            //                    ICMAlert obj1 = space.CreateObject<ICMAlert>();
                                            //                    obj1.Subject = "Low Stock - " + item.items + "(" + item.ItemCode + ")";
                                            //                    obj1.StartDate = DateTime.Now;
                                            //                    obj1.DueDate = DateTime.Now.AddDays(7);
                                            //                    obj1.RemindIn = TimeSpan.FromMinutes(5);
                                            //                    obj1.Description = "Nice";
                                            //                    space.CommitChanges();
                                            //                }
                                            //            }
                                            //        }
                                            //    }
                                            //} 
                                        }

                                    }
                                    else
                                    {

                                        string strStorage = string.Empty;
                                        string strVendorlt = string.Empty;
                                        intstockqty = 0;
                                        doubleunitprice = 0;
                                        doubleAmount = 0;
                                        intitemunit = 0;
                                        intalert = 0;
                                        intdayremain = 0;
                                        Nullable<DateTime> dateExpiry = null;
                                        Nullable<DateTime> dateRetire = null;
                                        int itemunit = findItem.ItemUnit;
                                        int stockQty = findItem.StockQty;
                                        int alert = findItem.AlertQty;
                                        //int dayremain = findItem.ReminderDaysInAdvance;
                                        double unitprice = findItem.UnitPrice;
                                        double amount = Convert.ToDouble(findItem.Amount);

                                        if (dt.Columns.Contains(rmChinese.GetString("StockQty")) && !row.IsNull(rmChinese.GetString("StockQty")))
                                        {
                                            intstockqty = Convert.ToInt32(row[rmChinese.GetString("StockQty")]);
                                            stockQty += Convert.ToInt32(row[rmChinese.GetString("StockQty")]);
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("StockQty")) && !row.IsNull(rmEnglish.GetString("StockQty")))
                                        {
                                            intstockqty = Convert.ToInt32(row[rmEnglish.GetString("StockQty")]);
                                            stockQty += Convert.ToInt32(row[rmEnglish.GetString("StockQty")]);
                                        }
                                        findItem.StockQty = stockQty;


                                        if (dt.Columns.Contains(rmChinese.GetString("Item/Unit")) && !row.IsNull(rmChinese.GetString("Item/Unit")))
                                        {
                                            intitemunit = Convert.ToInt32(row[rmChinese.GetString("Item/Unit")]);
                                            itemunit += Convert.ToInt32(row[rmChinese.GetString("Item/Unit")]);
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("Item/Unit")) && !row.IsNull(rmEnglish.GetString("Item/Unit")))
                                        {
                                            intitemunit = Convert.ToInt32(row[rmEnglish.GetString("Item/Unit")]);
                                            itemunit += Convert.ToInt32(row[rmEnglish.GetString("Item/Unit")]);
                                        }
                                        findItem.StockQty = stockQty;

                                        if (dt.Columns.Contains(rmChinese.GetString("Amount")) && !row.IsNull(rmChinese.GetString("Amount")))
                                        {
                                            doubleAmount = Convert.ToDouble(row[rmChinese.GetString("Amount")]);
                                            amount += Convert.ToDouble(row[rmChinese.GetString("Amount")]);
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("Amount")) && !row.IsNull(rmEnglish.GetString("Amount")))
                                        {
                                            doubleAmount = Convert.ToDouble(row[rmEnglish.GetString("Amount")]);
                                            amount += Convert.ToDouble(row[rmEnglish.GetString("Amount")]);
                                        }
                                        findItem.StockQty = stockQty;

                                        if (dt.Columns.Contains(rmChinese.GetString("AlertQty")) && !row.IsNull(rmChinese.GetString("AlertQty")))
                                        {
                                            intalert = Convert.ToInt32(row[rmChinese.GetString("AlertQty")]);
                                            alert += Convert.ToInt32(row[rmChinese.GetString("AlertQty")]);
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("AlertQty")) && !row.IsNull(rmEnglish.GetString("AlertQty")))
                                        {
                                            intalert = Convert.ToInt32(row[rmEnglish.GetString("AlertQty")]);
                                            alert += Convert.ToInt32(row[rmEnglish.GetString("AlertQty")]);
                                        }
                                        findItem.StockQty = stockQty;

                                        if (dt.Columns.Contains(rmChinese.GetString("RemainderDaysInAdvance")) && !row.IsNull(rmChinese.GetString("RemainderDaysInAdvance")))
                                        {
                                            intdayremain = Convert.ToInt32(row[rmChinese.GetString("RemainderDaysInAdvance")]);
                                            //dayremain += Convert.ToInt32(row[rmChinese.GetString("RemainderDaysInAdvance")]);
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("RemainderDaysInAdvance")) && !row.IsNull(rmEnglish.GetString("RemainderDaysInAdvance")))
                                        {
                                            intdayremain = Convert.ToInt32(row[rmEnglish.GetString("RemainderDaysInAdvance")]);
                                            // dayremain += Convert.ToInt32(row[rmEnglish.GetString("RemainderDaysInAdvance")]);
                                        }
                                        findItem.StockQty = stockQty;

                                        if (dt.Columns.Contains(rmChinese.GetString("UnitPrice")) && !row.IsNull(rmChinese.GetString("UnitPrice")))
                                        {
                                            //doubleunitprice = Convert.ToDouble(row[rmChinese.GetString("UnitPrice")]);

                                            string strUnitPrice = string.Empty;
                                            strUnitPrice = Convert.ToString(row[rmChinese.GetString("UnitPrice")]);
                                            if (strUnitPrice.Contains("$"))
                                            {
                                                strUnitPrice = strUnitPrice.Replace("$", "");
                                            }

                                            doubleunitprice = Convert.ToDouble(strUnitPrice);

                                            if (doubleunitprice < 1)
                                            {
                                                doubleunitprice = 1;
                                            }

                                            //unitprice += Convert.ToDouble(row[rmChinese.GetString("UnitPrice")]);
                                            unitprice += doubleunitprice;
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("UnitPrice")) && !row.IsNull(rmEnglish.GetString("UnitPrice")))
                                        {
                                            //doubleunitprice = Convert.ToDouble(row[rmEnglish.GetString("UnitPrice")]);
                                            //unitprice += Convert.ToDouble(row[rmChinese.GetString("UnitPrice")]);

                                            string strUnitPrice = string.Empty;
                                            strUnitPrice = Convert.ToString(row[rmEnglish.GetString("UnitPrice")]);
                                            if (strUnitPrice.Contains("$"))
                                            {
                                                strUnitPrice = strUnitPrice.Replace("$", "");
                                            }

                                            doubleunitprice = Convert.ToDouble(strUnitPrice);
                                            if (doubleunitprice < 1)
                                            {
                                                doubleunitprice = 1;
                                            }
                                            //unitprice += Convert.ToDouble(row[rmChinese.GetString("UnitPrice")]);
                                            unitprice += doubleunitprice;
                                        }
                                        findItem.UnitPrice = unitprice;

                                        if (dt.Columns.Contains(rmChinese.GetString("Vendorlt")) && !row.IsNull(rmChinese.GetString("Vendorlt")))
                                        {
                                            strVendorlt = row[rmChinese.GetString("Vendorlt")].ToString();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("Vendorlt")) && !row.IsNull(rmEnglish.GetString("Vendorlt")))
                                        {
                                            strVendorlt = row[rmEnglish.GetString("Vendorlt")].ToString();
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("Storage")) && !row.IsNull(rmChinese.GetString("Storage")))
                                        {
                                            strStorage = row[rmChinese.GetString("Storage")].ToString();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("Storage")) && !row.IsNull(rmEnglish.GetString("Storage")))
                                        {
                                            strStorage = row[rmEnglish.GetString("Storage")].ToString();
                                        }
                                        //dateRetire

                                        if (dt.Columns.Contains(rmChinese.GetString("RetireDate")) && !row.IsNull(rmChinese.GetString("RetireDate")))
                                        {
                                            if (row[rmChinese.GetString("RetireDate")].GetType() == typeof(DateTime))
                                            {
                                                findItem.RetireDate = Convert.ToDateTime(row[rmChinese.GetString("RetireDate")]);
                                            }
                                            else if (row[rmChinese.GetString("RetireDate")].GetType() == typeof(string))
                                            {
                                                string strdate = row[rmChinese.GetString("RetireDate")].ToString();
                                                if (strdate != string.Empty)
                                                {
                                                    string[] strdateonly = strdate.Split(' ');
                                                    findItem.RetireDate = DateTime.ParseExact(strdateonly[0], "MM/dd/yyyy", null);
                                                }
                                            }
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("RetireDate")) && !row.IsNull(rmEnglish.GetString("RetireDate")))
                                        {
                                            if (row[rmEnglish.GetString("RetireDate")].GetType() == typeof(DateTime))
                                            {
                                                findItem.RetireDate = Convert.ToDateTime(row[rmEnglish.GetString("RetireDate")]);
                                            }
                                            else if (row[rmEnglish.GetString("RetireDate")].GetType() == typeof(string))
                                            {
                                                string strdate = row[rmEnglish.GetString("RetireDate")].ToString();
                                                if (strdate != string.Empty)
                                                {
                                                    string[] strdateonly = strdate.Split(' ');
                                                    findItem.RetireDate = DateTime.ParseExact(strdateonly[0], "MM/dd/yyyy", null);
                                                }
                                            }
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("ExpiryDate")) && !row.IsNull(rmChinese.GetString("ExpiryDate")))
                                        {
                                            if (row[rmChinese.GetString("ExpiryDate")].GetType() == typeof(DateTime))
                                            {
                                                dateExpiry = Convert.ToDateTime(row[rmChinese.GetString("ExpiryDate")]);
                                            }
                                            else if (row[rmChinese.GetString("ExpiryDate")].GetType() == typeof(string))
                                            {
                                                string strdate = row[rmChinese.GetString("ExpiryDate")].ToString();
                                                if (strdate != string.Empty)
                                                {
                                                    dateExpiry = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
                                                }
                                            }
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("ExpiryDate")) && !row.IsNull(rmEnglish.GetString("ExpiryDate")))
                                        {
                                            if (row[rmEnglish.GetString("ExpiryDate")].GetType() == typeof(DateTime))
                                            {
                                                dateExpiry = Convert.ToDateTime(row[rmEnglish.GetString("ExpiryDate")]);
                                            }
                                            else if (row[rmEnglish.GetString("ExpiryDate")].GetType() == typeof(string))
                                            {
                                                string strdate = row[rmEnglish.GetString("ExpiryDate")].ToString();
                                                if (strdate != string.Empty)
                                                {
                                                    dateExpiry = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
                                                }
                                            }
                                        }

                                        if (intstockqty > 0)
                                        {
                                            itemsltno.Items.Add(findItem.Oid + ";" + strStorage + ";" + intstockqty.ToString());
                                        }
                                        processItems(findItem, dt, row, rmChinese, rmEnglish);
                                        ((ListView)View).CollectionSource.Add(findItem);
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
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "AddExcelItem"), InformationType.Warning, 4000, InformationPosition.Top);
                    }


                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "uploadfile"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    return;
                }
            }


            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            //try
            //{
            //    ResourceManager rmEnglish = new ResourceManager("Resources.LocalizeResourcesEnglish", Assembly.Load("App_GlobalResources"));
            //    ResourceManager rmChinese = new ResourceManager("Resources.LocalizeResourcesChinese", Assembly.Load("App_GlobalResources"));
            //    ItemsFileUpload itemsFile = (ItemsFileUpload)e.PopupWindowViewCurrentObject;
            //    if (itemsFile.InputFile != null)
            //    {
            //        string strFileName = itemsFile.InputFile.FileName;
            //        string strFilePath = HttpContext.Current.Server.MapPath(@"~\TestSpreadSheet\");
            //        string strLocalPath = HttpContext.Current.Server.MapPath(@"~\TestSpreadSheet\" + strFileName);
            //        if (Directory.Exists(strFilePath) == false)
            //        {
            //            Directory.CreateDirectory(strFilePath);
            //        }
            //        byte[] file = itemsFile.InputFile.Content;
            //        File.WriteAllBytes(strLocalPath, file);
            //        string fileExtension = Path.GetExtension(itemsFile.InputFile.FileName);
            //        string connectionString = string.Empty;
            //        if (fileExtension == ".xlsx")
            //        {
            //            connectionString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0 Xml;", strLocalPath);
            //        }
            //        else if (fileExtension == ".xls")
            //        {
            //            connectionString = String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;", strLocalPath);
            //        }
            //        //objectspace change
            //        IObjectSpace cloneitemObjectSpace = Application.CreateObjectSpace();
            //        CollectionSource source = new CollectionSource(cloneitemObjectSpace, typeof(Items));
            //        source.Criteria["filter"] = CriteriaOperator.Parse("IsNullOrEmpty([ItemCode])");
            //        ListView cloneItemListView = Application.CreateListView("Items_ListView_Copy", source, true);
            //        Frame.SetView(cloneItemListView);

            //        if (connectionString != string.Empty)
            //        {
            //            using (var conn = new OleDbConnection(connectionString))
            //            {
            //                conn.Open();
            //                List<string> sheets = new List<string>();
            //                OleDbDataAdapter oleda = new OleDbDataAdapter();
            //                DataTable sheetNameTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //                foreach (DataRow drSheet in sheetNameTable.Rows)
            //                {
            //                    if (drSheet["TABLE_NAME"].ToString().Contains("$"))
            //                    {
            //                        string s = drSheet["TABLE_NAME"].ToString();
            //                        sheets.Add(s.StartsWith("'") ? s.Substring(1, s.Length - 3) : s.Substring(0, s.Length - 1));
            //                    }
            //                }

            //                var cmd = conn.CreateCommand();
            //                cmd.CommandText = String.Format(
            //                    @"SELECT * FROM [{0}]", sheets[0] + "$"
            //                    );

            //                //variable name change
            //                packageunits.packageunit = new List<Packageunits>();
            //                packageunits.vendors = new List<Vendors>();
            //                itemsltno.Items = new List<string>();
            //                oleda = new OleDbDataAdapter(cmd);
            //                using (dt = new DataTable())
            //                {
            //                    oleda.Fill(dt);
            //                    foreach (DataRow row in dt.Rows)
            //                    {
            //                        var isEmpty = row.ItemArray.All(c => c is DBNull);
            //                        if (!isEmpty)
            //                        {
            //                            List<string> errorlist = new List<string>();
            //                            DateTime dateTime;
            //                            if (dt.Columns.Contains(rmChinese.GetString("ItemName")) && !row.IsNull(rmChinese.GetString("ItemName")))
            //                            {
            //                                strItemName = row[rmChinese.GetString("ItemName")].ToString();
            //                            }
            //                            else if (dt.Columns.Contains(rmEnglish.GetString("ItemName")) && !row.IsNull(rmEnglish.GetString("ItemName")))
            //                            {
            //                                strItemName = row[rmEnglish.GetString("ItemName")].ToString();
            //                            }
            //                            else
            //                            {
            //                                errorlist.Add("ItemName");
            //                            }

            //                            if (dt.Columns.Contains(rmChinese.GetString("Vendor")) && !row.IsNull(rmChinese.GetString("Vendor")))
            //                            {
            //                                strVendorName = row[rmChinese.GetString("Vendor")].ToString();
            //                            }
            //                            else if (dt.Columns.Contains(rmEnglish.GetString("Vendor")) && !row.IsNull(rmEnglish.GetString("Vendor")))
            //                            {
            //                                strVendorName = row[rmEnglish.GetString("Vendor")].ToString();
            //                            }
            //                            else
            //                            {
            //                                errorlist.Add("Vendor");
            //                            }

            //                            if (dt.Columns.Contains(rmChinese.GetString("Catalog#")) && !row.IsNull(rmChinese.GetString("Catalog#")))
            //                            {
            //                                strVendorCatName = row[rmChinese.GetString("Catalog#")].ToString();
            //                            }
            //                            else if (dt.Columns.Contains(rmEnglish.GetString("Catalog#")) && !row.IsNull(rmEnglish.GetString("Catalog#")))
            //                            {
            //                                strVendorCatName = row[rmEnglish.GetString("Catalog#")].ToString();
            //                            }
            //                            else
            //                            {
            //                                strVendorCatName = "N/A";
            //                            }

            //                            if (dt.Columns.Contains(rmChinese.GetString("Amount")) && !row.IsNull(rmChinese.GetString("Amount")))
            //                            {
            //                                strAmount = row[rmChinese.GetString("Amount")].ToString();
            //                            }
            //                            else if (dt.Columns.Contains(rmEnglish.GetString("Amount")) && !row.IsNull(rmEnglish.GetString("Amount")))
            //                            {
            //                                strAmount = row[rmEnglish.GetString("Amount")].ToString();
            //                            }
            //                            else
            //                            {
            //                                strAmount = string.Empty;
            //                            }

            //                            if (dt.Columns.Contains(rmChinese.GetString("AmountUnit")) && !row.IsNull(rmChinese.GetString("AmountUnit")))
            //                            {
            //                                strAmountUnits = row[rmChinese.GetString("AmountUnit")].ToString();
            //                            }
            //                            else if (dt.Columns.Contains(rmEnglish.GetString("AmountUnit")) && !row.IsNull(rmEnglish.GetString("AmountUnit")))
            //                            {
            //                                strAmountUnits = row[rmEnglish.GetString("AmountUnit")].ToString();
            //                            }
            //                            else
            //                            {
            //                                strAmountUnits = string.Empty;
            //                            }

            //                            if (dt.Columns.Contains(rmChinese.GetString("Specification")) && !row.IsNull(rmChinese.GetString("Specification")))
            //                            {
            //                                strSpecification = row[rmChinese.GetString("Specification")].ToString();
            //                            }
            //                            else if (dt.Columns.Contains(rmEnglish.GetString("Specification")) && !row.IsNull(rmEnglish.GetString("Specification")))
            //                            {
            //                                strSpecification = row[rmEnglish.GetString("Specification")].ToString();
            //                            }
            //                            else
            //                            {
            //                                errorlist.Add("Specification");
            //                            }

            //                            if (dt.Columns.Contains(rmChinese.GetString("Category")) && !row.IsNull(rmChinese.GetString("Category")))
            //                            {
            //                                strCategory = row[rmChinese.GetString("Category")].ToString();
            //                            }
            //                            else if (dt.Columns.Contains(rmEnglish.GetString("Category")) && !row.IsNull(rmEnglish.GetString("Category")))
            //                            {
            //                                strCategory = row[rmEnglish.GetString("Category")].ToString();
            //                            }
            //                            else
            //                            {
            //                                errorlist.Add("Category");
            //                            }

            //                            if ((dt.Columns.Contains(rmChinese.GetString("IsVendorLT")) && !row.IsNull(rmChinese.GetString("IsVendorLT")) && row[rmChinese.GetString("IsVendorLT")].GetType() != typeof(bool))
            //                                || (dt.Columns.Contains(rmEnglish.GetString("IsVendorLT")) && !row.IsNull(rmEnglish.GetString("IsVendorLT")) && row[rmEnglish.GetString("IsVendorLT")].GetType() != typeof(bool)))
            //                            {
            //                                errorlist.Add("IsVendorLT");
            //                            }

            //                            if ((dt.Columns.Contains(rmChinese.GetString("IsLabLT")) && !row.IsNull(rmChinese.GetString("IsLabLT")) && row[rmChinese.GetString("IsLabLT")].GetType() != typeof(bool))
            //                                || (dt.Columns.Contains(rmEnglish.GetString("IsLabLT")) && !row.IsNull(rmEnglish.GetString("IsLabLT")) && row[rmEnglish.GetString("IsLabLT")].GetType() != typeof(bool)))
            //                            {
            //                                errorlist.Add("IsLabLT");
            //                            }

            //                            if ((dt.Columns.Contains(rmChinese.GetString("IsToxic")) && !row.IsNull(rmChinese.GetString("IsToxic")) && row[rmChinese.GetString("IsToxic")].GetType() != typeof(bool))
            //                               || (dt.Columns.Contains(rmEnglish.GetString("IsToxic")) && !row.IsNull(rmEnglish.GetString("IsToxic")) && row[rmEnglish.GetString("IsToxic")].GetType() != typeof(bool)))
            //                            {
            //                                errorlist.Add("IsToxic");
            //                            }

            //                            if ((dt.Columns.Contains(rmChinese.GetString("RetireDate")) && !row.IsNull(rmChinese.GetString("RetireDate")) &&
            //                                !DateTime.TryParseExact(row[rmChinese.GetString("RetireDate")].ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dateTime) && row[rmChinese.GetString("RetireDate")].GetType() != typeof(DateTime))
            //                               || (dt.Columns.Contains(rmEnglish.GetString("RetireDate")) && !row.IsNull(rmEnglish.GetString("RetireDate")) &&
            //                               !DateTime.TryParseExact(row[rmEnglish.GetString("RetireDate")].ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dateTime) && row[rmEnglish.GetString("RetireDate")].GetType() != typeof(DateTime)))
            //                            {
            //                                errorlist.Add("RetireDate");
            //                            }

            //                            if ((dt.Columns.Contains(rmChinese.GetString("ExpiryDate")) && !row.IsNull(rmChinese.GetString("ExpiryDate")) && !DateTime.TryParseExact(row[rmChinese.GetString("ExpiryDate")].ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dateTime) && row[rmChinese.GetString("ExpiryDate")].GetType() != typeof(DateTime))
            //                              || (dt.Columns.Contains(rmEnglish.GetString("ExpiryDate")) && !row.IsNull(rmEnglish.GetString("ExpiryDate")) && !DateTime.TryParseExact(row[rmEnglish.GetString("ExpiryDate")].ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dateTime) && row[rmEnglish.GetString("ExpiryDate")].GetType() != typeof(DateTime)))
            //                            {
            //                                errorlist.Add("ExpiryDate");
            //                            }

            //                            if (errorlist.Count == 0)
            //                            {
            //                                Items findItem = ObjectSpace.FindObject<Items>(CriteriaOperator.Parse("[items]='" + strItemName + "' And [Vendor.Vendor] ='" + strVendorName + "' And [VendorCatName] ='" + strVendorCatName + "' And [Amount] ='" + strAmount + "' And [AmountUnit.UnitName] ='" + strAmountUnits + "' AND [Specification]='" + strSpecification + "' AND [Category.category] = '" + strCategory + "'"));
            //                                if (findItem == null)
            //                                {
            //                                    string strVendorlt = string.Empty;
            //                                    string strStorage = string.Empty;
            //                                    intstockqty = 0;
            //                                    Nullable<DateTime> dateExpiry = null;
            //                                    Items items = ObjectSpace.CreateObject<Items>();
            //                                    items.items = strItemName;
            //                                    items.VendorCatName = strVendorCatName;
            //                                    items.Specification = strSpecification;
            //                                    items.Amount = strAmount;
            //                                    if (strVendorName != string.Empty)
            //                                    {
            //                                        Vendors vendor = ObjectSpace.FindObject<Vendors>(CriteriaOperator.Parse("[Vendor]='" + strVendorName + "'"));
            //                                        if (vendor != null)
            //                                        {
            //                                            items.Vendor = vendor;
            //                                        }
            //                                        else
            //                                        {
            //                                            Vendors createVendor = ObjectSpace.CreateObject<Vendors>();
            //                                            createVendor.Vendor = strVendorName;
            //                                            items.Vendor = createVendor;
            //                                            packageunits.vendors.Add(createVendor);
            //                                        }
            //                                    }
            //                                    if (strAmountUnits != string.Empty)
            //                                    {
            //                                        Unit amountUnit = ObjectSpace.FindObject<Unit>(CriteriaOperator.Parse("[UnitName]='" + strAmountUnits + "'"));
            //                                        if (amountUnit != null)
            //                                        {
            //                                            items.AmountUnit = amountUnit;
            //                                        }
            //                                        else
            //                                        {
            //                                            Unit createAmountunit = ObjectSpace.CreateObject<Unit>();
            //                                            createAmountunit.UnitName = strAmountUnits;
            //                                            items.AmountUnit = createAmountunit;
            //                                        }
            //                                    }
            //                                    if (strCategory != string.Empty)
            //                                    {
            //                                        ICMCategory category = ObjectSpace.FindObject<ICMCategory>(CriteriaOperator.Parse("[category]='" + strCategory + "'"));
            //                                        if (category != null)
            //                                        {
            //                                            items.Category = category;
            //                                        }
            //                                        else
            //                                        {
            //                                            ICMCategory createCategory = ObjectSpace.CreateObject<ICMCategory>();
            //                                            createCategory.category = strCategory;
            //                                            items.Category = createCategory;
            //                                        }
            //                                    }

            //                                    if (dt.Columns.Contains(rmChinese.GetString("StockQty")) && !row.IsNull(rmChinese.GetString("StockQty")))
            //                                    {
            //                                        intstockqty = Convert.ToInt32(row[rmChinese.GetString("StockQty")]);
            //                                    }
            //                                    else if (dt.Columns.Contains(rmEnglish.GetString("StockQty")) && !row.IsNull(rmEnglish.GetString("StockQty")))
            //                                    {
            //                                        intstockqty = Convert.ToInt32(row[rmEnglish.GetString("StockQty")]);
            //                                    }
            //                                    items.StockQty = intstockqty;

            //                                    if (dt.Columns.Contains(rmChinese.GetString("Vendorlt")) && !row.IsNull(rmChinese.GetString("Vendorlt")))
            //                                    {
            //                                        strVendorlt = row[rmChinese.GetString("Vendorlt")].ToString();
            //                                    }
            //                                    else if (dt.Columns.Contains(rmEnglish.GetString("Vendorlt")) && !row.IsNull(rmEnglish.GetString("Vendorlt")))
            //                                    {
            //                                        strVendorlt = row[rmEnglish.GetString("Vendorlt")].ToString();
            //                                    }

            //                                    if (dt.Columns.Contains(rmChinese.GetString("Storage")) && !row.IsNull(rmChinese.GetString("Storage")))
            //                                    {
            //                                        strStorage = row[rmChinese.GetString("Storage")].ToString();
            //                                    }
            //                                    else if (dt.Columns.Contains(rmEnglish.GetString("Storage")) && !row.IsNull(rmEnglish.GetString("Storage")))
            //                                    {
            //                                        strStorage = row[rmEnglish.GetString("Storage")].ToString();
            //                                    }

            //                                    if (dt.Columns.Contains(rmChinese.GetString("ExpiryDate")) && !row.IsNull(rmChinese.GetString("ExpiryDate")))
            //                                    {
            //                                        if (row[rmChinese.GetString("ExpiryDate")].GetType() == typeof(DateTime))
            //                                        {
            //                                            dateExpiry = Convert.ToDateTime(row[rmChinese.GetString("ExpiryDate")]);
            //                                        }
            //                                        else if (row[rmChinese.GetString("ExpiryDate")].GetType() == typeof(string))
            //                                        {
            //                                            string strdate = row[rmChinese.GetString("ExpiryDate")].ToString();
            //                                            if (strdate != string.Empty)
            //                                            {
            //                                                dateExpiry = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
            //                                            }
            //                                        }
            //                                    }
            //                                    else if (dt.Columns.Contains(rmEnglish.GetString("ExpiryDate")) && !row.IsNull(rmEnglish.GetString("ExpiryDate")))
            //                                    {
            //                                        if (row[rmEnglish.GetString("ExpiryDate")].GetType() == typeof(DateTime))
            //                                        {
            //                                            dateExpiry = Convert.ToDateTime(row[rmEnglish.GetString("ExpiryDate")]);
            //                                        }
            //                                        else if (row[rmEnglish.GetString("ExpiryDate")].GetType() == typeof(string))
            //                                        {
            //                                            string strdate = row[rmEnglish.GetString("ExpiryDate")].ToString();
            //                                            if (strdate != string.Empty)
            //                                            {
            //                                                dateExpiry = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
            //                                            }
            //                                        }
            //                                    }

            //                                    itemsltno.Items.Add(items.Oid + ";" + strStorage + ";" + dateExpiry + ";" + intstockqty.ToString() + ";" + strVendorlt);
            //                                    processItems(items, dt, row, rmChinese, rmEnglish);
            //                                    ((ListView)View).CollectionSource.Add(items);
            //                                }
            //                                else
            //                                {
            //                                    string strStorage = string.Empty;
            //                                    string strVendorlt = string.Empty;
            //                                    intstockqty = 0;
            //                                    Nullable<DateTime> dateExpiry = null;
            //                                    int stockQty = findItem.StockQty;
            //                                    if (dt.Columns.Contains(rmChinese.GetString("StockQty")) && !row.IsNull(rmChinese.GetString("StockQty")))
            //                                    {
            //                                        intstockqty = Convert.ToInt32(row[rmChinese.GetString("StockQty")]);
            //                                        stockQty += Convert.ToInt32(row[rmChinese.GetString("StockQty")]);
            //                                    }
            //                                    else if (dt.Columns.Contains(rmEnglish.GetString("StockQty")) && !row.IsNull(rmEnglish.GetString("StockQty")))
            //                                    {
            //                                        intstockqty = Convert.ToInt32(row[rmEnglish.GetString("StockQty")]);
            //                                        stockQty += Convert.ToInt32(row[rmEnglish.GetString("StockQty")]);
            //                                    }
            //                                    findItem.StockQty = stockQty;

            //                                    if (dt.Columns.Contains(rmChinese.GetString("Vendorlt")) && !row.IsNull(rmChinese.GetString("Vendorlt")))
            //                                    {
            //                                        strVendorlt = row[rmChinese.GetString("Vendorlt")].ToString();
            //                                    }
            //                                    else if (dt.Columns.Contains(rmEnglish.GetString("Vendorlt")) && !row.IsNull(rmEnglish.GetString("Vendorlt")))
            //                                    {
            //                                        strVendorlt = row[rmEnglish.GetString("Vendorlt")].ToString();
            //                                    }

            //                                    if (dt.Columns.Contains(rmChinese.GetString("Storage")) && !row.IsNull(rmChinese.GetString("Storage")))
            //                                    {
            //                                        strStorage = row[rmChinese.GetString("Storage")].ToString();
            //                                    }
            //                                    else if (dt.Columns.Contains(rmEnglish.GetString("Storage")) && !row.IsNull(rmEnglish.GetString("Storage")))
            //                                    {
            //                                        strStorage = row[rmEnglish.GetString("Storage")].ToString();
            //                                    }

            //                                    if (dt.Columns.Contains(rmChinese.GetString("ExpiryDate")) && !row.IsNull(rmChinese.GetString("ExpiryDate")))
            //                                    {
            //                                        if (row[rmChinese.GetString("ExpiryDate")].GetType() == typeof(DateTime))
            //                                        {
            //                                            dateExpiry = Convert.ToDateTime(row[rmChinese.GetString("ExpiryDate")]);
            //                                        }
            //                                        else if (row[rmChinese.GetString("ExpiryDate")].GetType() == typeof(string))
            //                                        {
            //                                            string strdate = row[rmChinese.GetString("ExpiryDate")].ToString();
            //                                            if (strdate != string.Empty)
            //                                            {
            //                                                dateExpiry = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
            //                                            }
            //                                        }
            //                                    }
            //                                    else if (dt.Columns.Contains(rmEnglish.GetString("ExpiryDate")) && !row.IsNull(rmEnglish.GetString("ExpiryDate")))
            //                                    {
            //                                        if (row[rmEnglish.GetString("ExpiryDate")].GetType() == typeof(DateTime))
            //                                        {
            //                                            dateExpiry = Convert.ToDateTime(row[rmEnglish.GetString("ExpiryDate")]);
            //                                        }
            //                                        else if (row[rmEnglish.GetString("ExpiryDate")].GetType() == typeof(string))
            //                                        {
            //                                            string strdate = row[rmEnglish.GetString("ExpiryDate")].ToString();
            //                                            if (strdate != string.Empty)
            //                                            {
            //                                                dateExpiry = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
            //                                            }
            //                                        }
            //                                    }

            //                                    itemsltno.Items.Add(findItem.Oid + ";" + strStorage + ";" + dateExpiry + ";" + intstockqty.ToString() + ";" + strVendorlt);
            //                                    processItems(findItem, dt, row, rmChinese, rmEnglish);
            //                                    ((ListView)View).CollectionSource.Add(findItem);
            //                                }
            //                            }
            //                            else
            //                            {
            //                                var error = "Error in columns - " + string.Join(", ", errorlist) + " of row number - " + (dt.Rows.IndexOf(row) + 1);
            //                                Application.ShowViewStrategy.ShowMessage(error, InformationType.Error, 6000, InformationPosition.Top);
            //                                break;
            //                            }
            //                        }
            //                    }
            //                }
            //                conn.Close();
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, 3000, InformationPosition.Top);
            //}
        }

        private Items processItems(Items items, DataTable dt, DataRow row, ResourceManager rmChinese, ResourceManager rmEnglish)
        {
            try
            {
                string strUnit = string.Empty;
                string strVendorName = string.Empty;
                string strGrade = string.Empty;
                string strItemDescription = string.Empty;
                string strComment = string.Empty;
                string strDepartment = string.Empty;
                double unitPrice = 0;
                int ItemUnit = 0;
                int alertQty = 0;
                bool isVendorLT = false;
                bool isLabLT = false;
                bool isToxic = false;

                if (dt.Columns.Contains(rmChinese.GetString("PackUnits")) && !row.IsNull(rmChinese.GetString("PackUnits")))
                {
                    strUnit = row[rmChinese.GetString("PackUnits")].ToString();
                }
                else if (dt.Columns.Contains(rmEnglish.GetString("PackUnits")) && !row.IsNull(rmEnglish.GetString("PackUnits")))
                {
                    strUnit = row[rmEnglish.GetString("PackUnits")].ToString();
                }
                if (strUnit != string.Empty)
                {
                    Packageunits unit = ObjectSpace.FindObject<Packageunits>(CriteriaOperator.Parse("[Option]='" + strUnit + "'"));
                    if (unit != null)
                    {
                        items.Unit = unit;
                    }
                    else
                    {
                        Packageunits createUnit = ObjectSpace.CreateObject<Packageunits>();
                        createUnit.Option = strUnit;
                        items.Unit = createUnit;
                        if (packageunits.packageunit == null)
                        {
                            packageunits.packageunit = new List<Packageunits>();
                        }
                        packageunits.packageunit.Add(createUnit);
                    }
                }

                if (dt.Columns.Contains(rmChinese.GetString("UnitPrice")) && !row.IsNull(rmChinese.GetString("UnitPrice")))
                {
                    string strUnitPrice = string.Empty;
                    strUnitPrice = Convert.ToString(row[rmChinese.GetString("UnitPrice")]);
                    if (strUnitPrice.Contains("$"))
                    {
                        strUnitPrice = strUnitPrice.Replace("$", "");
                    }
                    //unitPrice = Convert.ToDouble(row[rmChinese.GetString("UnitPrice")]);
                    unitPrice = Convert.ToDouble(strUnitPrice);

                    if (unitPrice < 1)
                    {
                        unitPrice = 1;
                    }
                }
                else if (dt.Columns.Contains(rmEnglish.GetString("UnitPrice")) && !row.IsNull(rmEnglish.GetString("UnitPrice")))
                {
                    string strUnitPrice = string.Empty;
                    strUnitPrice = Convert.ToString(row[rmEnglish.GetString("UnitPrice")]);
                    if (strUnitPrice.Contains("$"))
                    {
                        strUnitPrice = strUnitPrice.Replace("$", "");
                    }
                    //unitPrice = Convert.ToDouble(row[rmChinese.GetString("UnitPrice")]);
                    unitPrice = Convert.ToDouble(strUnitPrice);
                    if (unitPrice < 1)
                    {
                        unitPrice = 1;
                    }
                    // unitPrice = Convert.ToDouble(row[rmEnglish.GetString("UnitPrice")]);
                }
                items.UnitPrice = unitPrice;

                if (dt.Columns.Contains(rmChinese.GetString("Item/Unit")) && !row.IsNull(rmChinese.GetString("Item/Unit")))
                {
                    ItemUnit = Convert.ToInt32(row[rmChinese.GetString("Item/Unit")]);
                }
                else if (dt.Columns.Contains(rmEnglish.GetString("Item/Unit")) && !row.IsNull(rmEnglish.GetString("Item/Unit")))
                {
                    ItemUnit = Convert.ToInt32(row[rmEnglish.GetString("Item/Unit")]);
                }
                items.ItemUnit = ItemUnit;

                //if (dt.Columns.Contains(rmChinese.GetString("VendorItemName")) && !row.IsNull(rmChinese.GetString("VendorItemName")))
                //{
                //    strVendorName = row[rmChinese.GetString("VendorItemName")].ToString();
                //}
                //else if (dt.Columns.Contains(rmEnglish.GetString("VendorItemName")) && !row.IsNull(rmEnglish.GetString("VendorItemName")))
                //{
                //    strVendorName = row[rmEnglish.GetString("VendorItemName")].ToString();
                //}
                //items.VendorItemName = strVendorName;

                if (dt.Columns.Contains(rmChinese.GetString("Grade")) && !row.IsNull(rmChinese.GetString("Grade")))
                {
                    strGrade = row[rmChinese.GetString("Grade")].ToString();
                }
                else if (dt.Columns.Contains(rmEnglish.GetString("Grade")) && !row.IsNull(rmEnglish.GetString("Grade")))
                {
                    strGrade = row[rmEnglish.GetString("Grade")].ToString();
                }
                if (strGrade != string.Empty)
                {
                    Grades grades = ObjectSpace.FindObject<Grades>(CriteriaOperator.Parse("[Grade]='" + strGrade + "'"));
                    if (grades != null)
                    {
                        items.Grade = grades;
                    }
                    else
                    {
                        Grades createGrades = ObjectSpace.CreateObject<Grades>();
                        createGrades.Grade = strGrade;
                        items.Grade = createGrades;
                    }
                }

                if (dt.Columns.Contains(rmChinese.GetString("AlertQty")) && !row.IsNull(rmChinese.GetString("AlertQty")))
                {
                    alertQty = Convert.ToInt32(row[rmChinese.GetString("AlertQty")]);
                }
                else if (dt.Columns.Contains(rmEnglish.GetString("AlertQty")) && !row.IsNull(rmEnglish.GetString("AlertQty")))
                {
                    alertQty = Convert.ToInt32(row[rmEnglish.GetString("AlertQty")]);
                }
                items.AlertQty = alertQty;

                //if (dt.Columns.Contains(rmChinese.GetString("IsVendorLT")) && !row.IsNull(rmChinese.GetString("IsVendorLT")))
                //{
                //    isVendorLT = Convert.ToBoolean(row[rmChinese.GetString("IsVendorLT")]);
                //}
                //else if (dt.Columns.Contains(rmEnglish.GetString("IsVendorLT")) && !row.IsNull(rmEnglish.GetString("IsVendorLT")))
                //{
                //    isVendorLT = Convert.ToBoolean(row[rmEnglish.GetString("IsVendorLT")]);
                //}
                //items.IsVendorLT = isVendorLT;

                //if (dt.Columns.Contains(rmChinese.GetString("IsLabLT")) && !row.IsNull(rmChinese.GetString("IsLabLT")))
                //{
                //    isLabLT = Convert.ToBoolean(row[rmChinese.GetString("IsLabLT")]);
                //}
                //else if (dt.Columns.Contains(rmEnglish.GetString("IsLabLT")) && !row.IsNull(rmEnglish.GetString("IsLabLT")))
                //{
                //    isLabLT = Convert.ToBoolean(row[rmEnglish.GetString("IsLabLT")]);
                //}
                //items.IsLabLT = isLabLT;

                //if (dt.Columns.Contains(rmChinese.GetString("IsToxic")) && !row.IsNull(rmChinese.GetString("IsToxic")))
                //{
                //    isToxic = Convert.ToBoolean(row[rmChinese.GetString("IsToxic")]);
                //}
                //else if (dt.Columns.Contains(rmEnglish.GetString("IsToxic")) && !row.IsNull(rmEnglish.GetString("IsToxic")))
                //{
                //    isToxic = Convert.ToBoolean(row[rmEnglish.GetString("IsToxic")]);
                //}
                //items.IsToxic = isToxic;

                if (dt.Columns.Contains(rmChinese.GetString("RetireDate")) && !row.IsNull(rmChinese.GetString("RetireDate")))
                {
                    if (row[rmChinese.GetString("RetireDate")].GetType() == typeof(DateTime))
                    {
                        items.RetireDate = Convert.ToDateTime(row[rmChinese.GetString("RetireDate")]);
                    }
                    else if (row[rmChinese.GetString("RetireDate")].GetType() == typeof(string))
                    {
                        string strdate = row[rmChinese.GetString("RetireDate")].ToString();
                        if (strdate != string.Empty)
                        {
                            string[] strdateonly = strdate.Split(' ');
                            items.RetireDate = DateTime.ParseExact(strdateonly[0], "MM/dd/yyyy", null);
                        }
                    }
                }
                else if (dt.Columns.Contains(rmEnglish.GetString("RetireDate")) && !row.IsNull(rmEnglish.GetString("RetireDate")))
                {
                    if (row[rmEnglish.GetString("RetireDate")].GetType() == typeof(DateTime))
                    {
                        items.RetireDate = Convert.ToDateTime(row[rmEnglish.GetString("RetireDate")]);
                    }
                    else if (row[rmEnglish.GetString("RetireDate")].GetType() == typeof(string))
                    {
                        string strdate = row[rmEnglish.GetString("RetireDate")].ToString();
                        if (strdate != string.Empty)
                        {
                            string[] strdateonly = strdate.Split(' ');
                            items.RetireDate = DateTime.ParseExact(strdateonly[0], "MM/dd/yyyy", null);
                        }
                    }
                }

                //if (dt.Columns.Contains(rmChinese.GetString("Item Description")) && !row.IsNull(rmChinese.GetString("Item Description")))
                //{
                //    strItemDescription = row[rmChinese.GetString("Item Description")].ToString();
                //}
                //else if (dt.Columns.Contains(rmEnglish.GetString("Item Description")) && !row.IsNull(rmEnglish.GetString("Item Description")))
                //{
                //    strItemDescription = row[rmEnglish.GetString("Item Description")].ToString();
                //}
                //items.ItemDescription = strItemDescription;

                if (dt.Columns.Contains(rmChinese.GetString("Comment")) && !row.IsNull(rmChinese.GetString("Comment")))
                {
                    strComment = row[rmChinese.GetString("Comment")].ToString();
                }
                else if (dt.Columns.Contains(rmEnglish.GetString("Comment")) && !row.IsNull(rmEnglish.GetString("Comment")))
                {
                    strComment = row[rmEnglish.GetString("Comment")].ToString();
                }
                items.Comment = strComment;

                return items;
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, 3000, InformationPosition.Top);
                return null;
            }
        }

        public void ProcessAction(string parameter)
        {
            try
            {
                //string[] paramsplit = parameter.Split('|');
                if (parameter == "0")
                {

                    ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                    ObjectSpace.CommitChanges();

                    foreach (Items item in ((ListView)View).CollectionSource.List.Cast<Items>().ToList())
                    {
                        if (string.IsNullOrEmpty(item.ItemCode))
                        {
                            CriteriaOperator ItemCode = CriteriaOperator.Parse("Max(ItemCode)");
                            string NewItemCode = (Convert.ToInt32(((XPObjectSpace)((ListView)View).ObjectSpace).Session.Evaluate(typeof(Items), ItemCode, null)) + 1).ToString();
                            if (NewItemCode.Length == 1)
                            {
                                NewItemCode = "00" + NewItemCode;
                            }
                            else if (NewItemCode.Length == 2)
                            {
                                NewItemCode = "0" + NewItemCode;
                            }
                            item.ItemCode = NewItemCode;
                            ObjectSpace.CommitChanges();


                        }
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages", "ItemsImported"), InformationType.Success, 3000, InformationPosition.Top);
                    }

                    IList<Vendors> emptyvendors = ObjectSpace.GetObjects<Vendors>(CriteriaOperator.Parse("IsNullOrEmpty([Vendorcode])"));
                    if (packageunits.vendors != null)
                    {
                        foreach (Vendors vendor in packageunits.vendors)
                        {
                            CriteriaOperator Vendorcode = CriteriaOperator.Parse("Max(Vendorcode)");
                            string NewVendorCode = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(Vendors), Vendorcode, null)) + 1).ToString();
                            if (NewVendorCode.Length == 1)
                            {
                                NewVendorCode = "00" + NewVendorCode;
                            }
                            else if (NewVendorCode.Length == 2)
                            {
                                NewVendorCode = "0" + NewVendorCode;
                            }
                            vendor.Vendorcode = NewVendorCode;
                            ObjectSpace.CommitChanges();
                        }
                    }

                    if (packageunits.packageunit != null)
                    {
                        foreach (Packageunits unit in packageunits.packageunit)
                        {
                            CriteriaOperator sort = CriteriaOperator.Parse("Max(Sort)");
                            int newSort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(Packageunits), sort, null)) + 1);
                            //unit.Sort = newSort;
                            ObjectSpace.CommitChanges();
                        }
                    }

                    IObjectSpace Space = Application.CreateObjectSpace(typeof(Requisition));
                    CriteriaOperator sam = CriteriaOperator.Parse("Max(SUBSTRING(ReceiveID, 2))");
                    string temprc = (Convert.ToInt32(((XPObjectSpace)Space).Session.Evaluate(typeof(Requisition), sam, null)) + 1).ToString();
                    var curedate = DateTime.Now.ToString("yyMMdd");
                    if (temprc != "1")
                    {
                        var predate = temprc.Substring(0, 6);
                        if (predate == curedate)
                        {
                            temprc = "RC" + temprc;
                        }
                        else
                        {
                            temprc = "RC" + curedate + "01";
                        }
                    }
                    else
                    {
                        temprc = "RC" + curedate + "01";
                    }
                    var receivedate = DateTime.Now;

                    if (itemsltno.Items != null)
                    {
                        //List<Distribution> lstDistributions = View.SelectedObjects.Cast<Distribution>().ToList();
                        List<Distribution> lstDistributions = new List<Distribution>();
                        foreach (string distributionItems in itemsltno.Items)
                        {
                            string[] distributionItem = distributionItems.Split(';');
                            IObjectSpace os = Application.CreateObjectSpace();
                            Items item = os.FindObject<Items>(CriteriaOperator.Parse("[Oid]='" + distributionItem[0] + "'"));
                            if (item != null)
                            {
                                Requisition newobj = os.CreateObject<Requisition>();
                                newobj.Item = os.GetObject<Items>(item);
                                if (item.Vendor != null)
                                {
                                    newobj.Vendor = newobj.Item.Vendor;
                                }
                                if (item.Manufacturer != null)
                                {
                                    newobj.Manufacturer = newobj.Item.Manufacturer;
                                }
                                if (item.UnitPrice != 0)
                                {
                                    newobj.UnitPrice = newobj.Item.UnitPrice;
                                }
                                if (item.VendorCatName != null)
                                {
                                    newobj.Catalog = newobj.Item.VendorCatName;
                                }
                                int totqty = 0;
                                if (distributionItem[2] != null && distributionItem[2].Length > 0)
                                {
                                    newobj.TotalItems = Convert.ToInt32(distributionItem[2].ToString());
                                    totqty = Convert.ToInt32(distributionItem[2].ToString());
                                    newobj.itemreceived = totqty + " of " + totqty;
                                }
                                newobj.ReceiveID = temprc;
                                newobj.Status = Requisition.TaskStatus.Received;

                                if (totqty != 0)
                                {
                                    for (int i = 1; i <= totqty; i++)
                                    {
                                        Distribution distribution = os.CreateObject<Distribution>();
                                        distribution.Item = item;
                                        distribution.Vendor = item.Vendor;

                                        //if (!string.IsNullOrEmpty(distributionItem[2]))
                                        //{
                                        //    distribution.ExpiryDate = Convert.ToDateTime(distributionItem[2]);
                                        //}

                                        //CriteriaOperator ltcriteria = CriteriaOperator.Parse("Max(SUBSTRING(LT, 2))");
                                        //string templt = (Convert.ToInt32(((XPObjectSpace)os).Session.Evaluate(typeof(Distribution), ltcriteria, null)) + 1).ToString();
                                        //var curdate = DateTime.Now.ToString("yy");
                                        //if (templt != "1")
                                        //{
                                        //    var predate = templt.Substring(0, 2);
                                        //    if (predate == curdate)
                                        //    {
                                        //        templt = "LT" + templt;
                                        //    }
                                        //    else
                                        //    {
                                        //        templt = "LT" + curdate + "0001";
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    templt = "LT" + curdate + "0001";
                                        //}
                                        //distribution.LT = templt;
                                        distribution.TotalItems = totqty;
                                        distribution.DistributionDate = receivedate;
                                        distribution.DistributedBy = os.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                        //distribution.VendorLT = distributionItem[4].ToString();
                                        distribution.ReceiveID = temprc;
                                        //// distribution.ReceiveCount = temprc + "-" + i;
                                        distribution.itemreceived = i + " of " + totqty;
                                        distribution.OrderQty = totqty;
                                        distribution.GivenBy = ((XPObjectSpace)(os)).Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                        distribution.ReceivedBy = os.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                        distribution.ReceiveDate = receivedate;
                                        distribution.NumItemCode = itemsltno.Items.Count;

                                        if (!string.IsNullOrEmpty(distributionItem[1]))
                                        {
                                            ICMStorage storage = os.FindObject<ICMStorage>(CriteriaOperator.Parse("[storage]='" + distributionItem[1] + "'"));
                                            if (storage != null)
                                            {
                                                distribution.Storage = storage;
                                            }
                                            else
                                            {
                                                ICMStorage createdStorage = os.CreateObject<ICMStorage>();
                                                createdStorage.storage = distributionItem[1];
                                                //createdStorage.Location = "N/A";
                                                distribution.Storage = createdStorage;
                                            }
                                        }

                                        distribution.Status = Distribution.LTStatus.PendingConsume;
                                        lstDistributions.Add(distribution);
                                        //if (distribution.ExpiryDate != null && distribution.ExpiryDate <= DateTime.Now.AddDays(7))
                                        //{
                                        //    IObjectSpace space = Application.CreateObjectSpace(typeof(ICMAlert));
                                        //    ICMAlert objdisp = space.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Expiry Alert - " + distribution.LT));
                                        //    if (objdisp == null)
                                        //    {
                                        //        ICMAlert obj1 = space.CreateObject<ICMAlert>();
                                        //        obj1.Subject = "Expiry Alert - " + distribution.LT;
                                        //        obj1.StartDate = DateTime.Now;
                                        //        obj1.DueDate = DateTime.Now.AddDays(7);
                                        //        obj1.RemindIn = TimeSpan.FromMinutes(5);
                                        //        obj1.Description = "Nice";
                                        //        space.CommitChanges();
                                        //    }
                                        //    if (distribution.ExpiryDate <= DateTime.Now)
                                        //    {
                                        //        distribution.Status = Distribution.LTStatus.PendingDispose;
                                        //    }
                                        //}
                                        os.CommitChanges();

                                    }

                                    if (item.StockQty > item.AlertQty)
                                    {
                                        IObjectSpace objspace = Application.CreateObjectSpace();
                                        IList<ICMAlert> alertlist = objspace.GetObjects<ICMAlert>(CriteriaOperator.Parse("[Subject] = ?", "Low Stock - " + item.items + "(" + item.ItemCode + ")"));
                                        if (alertlist != null)
                                        {
                                            foreach (ICMAlert objitem in alertlist)
                                            {
                                                objitem.AlarmTime = null;
                                                objitem.RemindIn = null;
                                            }
                                            objspace.CommitChanges();
                                        }
                                    }
                                    else
                                    {
                                        IObjectSpace objspace1 = Application.CreateObjectSpace();
                                        ICMAlert objdisp1 = objspace1.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Low Stock - " + item.items + "(" + item.ItemCode + ")"));
                                        if (objdisp1 == null)
                                        {
                                            IObjectSpace space = Application.CreateObjectSpace(typeof(ICMAlert));
                                            ICMAlert obj1 = space.CreateObject<ICMAlert>();
                                            obj1.Subject = "Low Stock - " + item.items + "(" + item.ItemCode + ")";
                                            obj1.StartDate = DateTime.Now;
                                            obj1.DueDate = DateTime.Now.AddDays(7);
                                            obj1.RemindIn = TimeSpan.FromMinutes(5);
                                            obj1.Description = "Nice";
                                            space.CommitChanges();
                                        }
                                    }
                                }

                            }
                        }

                        if (lstDistributions != null && lstDistributions.Count > 0)
                        {
                            List<Tuple<string, string, string, bool>> lstLt = new List<Tuple<string, string, string, bool>>();
                            IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(Distribution));
                            string strLTNo = string.Empty;
                            string strLTNowithVendorLT = string.Empty;
                            foreach (Distribution objDistribution in lstDistributions)
                            {
                                //if (objdis.rgMode == ENMode.Enter.ToString())
                                Distribution obj = objectSpace1.GetObject<Distribution>(objDistribution);
                                if (string.IsNullOrEmpty(obj.LT))
                                {
                                    Tuple<string, string, string, bool> tupDistribution;
                                    string templt = string.Empty;
                                    if (obj.Item != null)
                                    {
                                        string curdate = DateTime.Now.ToString("yy");
                                        if ((obj.Item.IsVendorLT && lstLt.FirstOrDefault(i => i.Item1 == obj.Item.ItemCode && i.Item4 == obj.Item.IsVendorLT && i.Item3 == obj.VendorLT) == null) ||
                                            (!obj.Item.IsVendorLT && lstLt.FirstOrDefault(i => i.Item1 == obj.Item.ItemCode && i.Item4 == false) == null))
                                        {
                                            bool HasSequence = false;
                                            if ((obj.Item.IsVendorLT && lstDistributions.FirstOrDefault(i => i.Item.ItemCode == obj.Item.ItemCode && i.Item.IsVendorLT == true && i.VendorLT == obj.VendorLT && i.Oid != obj.Oid) != null) ||
                                                (!obj.Item.IsVendorLT && lstDistributions.FirstOrDefault(i => i.Item.ItemCode == obj.Item.ItemCode && i.Item.IsVendorLT == false && i.Oid != obj.Oid) != null))
                                            {
                                                HasSequence = true;
                                            }
                                            CriteriaOperator LTExpressionWithoutSequence = CriteriaOperator.Parse("Max(SUBSTRING(LT, 2))");
                                            CriteriaOperator LTCriteriaWithoutSequence = CriteriaOperator.Parse("StartsWith([LT], ?) And Not Contains([LT], '_')", "LT" + curdate);

                                            CriteriaOperator LTExpressionWithSequence = CriteriaOperator.Parse("Max(SUBSTRING(LT, 2, 6))");
                                            CriteriaOperator LTCriteriaWithSequence = CriteriaOperator.Parse("StartsWith([LT], ?) And Contains([LT], '_')", "LT" + curdate);

                                            int templtwithoutsequence = Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Distribution), LTExpressionWithoutSequence, LTCriteriaWithoutSequence)) + 1;
                                            int templtwithsequence = Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Distribution), LTExpressionWithSequence, LTCriteriaWithSequence)) + 1;
                                            if (templtwithoutsequence == 1 && templtwithsequence == 1)
                                            {
                                                if (!obj.Item.IsVendorLT && string.IsNullOrEmpty(strLTNo))
                                                {
                                                    strLTNo = "LT" + curdate + "0001";
                                                }
                                                else
                                                if (!string.IsNullOrEmpty(obj.VendorLT) && string.IsNullOrEmpty(strLTNowithVendorLT))
                                                {
                                                    strLTNowithVendorLT = "LT" + curdate + "0001";
                                                }
                                            }
                                            else if (templtwithoutsequence > templtwithsequence)
                                            {
                                                if (string.IsNullOrEmpty(obj.VendorLT))
                                                {
                                                    strLTNo = "LT" + templtwithoutsequence;
                                                }
                                                else
                                                if (!string.IsNullOrEmpty(obj.VendorLT) /*&& string.IsNullOrEmpty(strLTNowithVendorLT)*/)
                                                {
                                                    strLTNowithVendorLT = "LT" + templtwithoutsequence;
                                                }
                                            }
                                            else if (templtwithoutsequence < templtwithsequence)
                                            {
                                                if (string.IsNullOrEmpty(obj.VendorLT))
                                                {
                                                    strLTNo = "LT" + templtwithsequence;
                                                }
                                                else
                                                if (!string.IsNullOrEmpty(obj.VendorLT)/* && string.IsNullOrEmpty(strLTNowithVendorLT)*/)
                                                {
                                                    strLTNowithVendorLT = "LT" + templtwithsequence;
                                                }
                                            }

                                            if (!HasSequence)
                                            {
                                                if (string.IsNullOrEmpty(obj.VendorLT))
                                                {
                                                    templt = strLTNo;
                                                }
                                                else
                                                {
                                                    templt = strLTNowithVendorLT;
                                                }
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(obj.VendorLT))
                                                {
                                                    templt = strLTNo + "_01";
                                                }
                                                else
                                                {
                                                    templt = strLTNowithVendorLT + "_01";
                                                }
                                            }
                                            tupDistribution = new Tuple<string, string, string, bool>(obj.Item.ItemCode, templt, obj.VendorLT, obj.Item.IsVendorLT);
                                            lstLt.Add(tupDistribution);
                                        }
                                        else
                                        {
                                            int index = -1;
                                            if (!obj.Item.IsVendorLT)
                                            {
                                                index = lstLt.FindIndex(i => i.Item1 == obj.Item.ItemCode && i.Item4 == false);
                                            }
                                            else
                                            {
                                                index = lstLt.FindIndex(i => i.Item1 == obj.Item.ItemCode && i.Item3 == obj.VendorLT);
                                            }
                                            if (index >= 0)
                                            {
                                                string[] strPrevlt = lstLt[index].Item2.Split('_');
                                                if ((Convert.ToInt32(strPrevlt[1]) + 1) < 10)
                                                {
                                                    templt = strPrevlt[0] + "_0" + (Convert.ToInt32(strPrevlt[1]) + 1);
                                                }
                                                else
                                                {
                                                    templt = strPrevlt[0] + "_" + (Convert.ToInt32(strPrevlt[1]) + 1);
                                                }
                                                lstLt[index] = new Tuple<string, string, string, bool>(obj.Item.ItemCode, templt, obj.VendorLT, obj.Item.IsVendorLT);
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(templt))
                                    {
                                        obj.LT = templt;
                                        objectSpace1.CommitChanges();

                                    }
                                }
                            }
                        }

                    }
                    NotificationsModule module = this.Application.Modules.FindModule<NotificationsModule>();
                    module.ShowNotificationsWindow = false;
                    module.NotificationsService.Refresh();

                    ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                    foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                    {
                        if (parent.Id == "InventoryManagement")
                        {
                            foreach (ChoiceActionItem child in parent.Items)
                            {
                                if (child.Id == "Operations")
                                {
                                    foreach (ChoiceActionItem subchild in child.Items)
                                    {
                                        if (subchild.Id == "Receiving")
                                        {
                                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            CollectionSource cs = new CollectionSource(objectSpace, typeof(Requisition));
                                            cs.Criteria["FilterPOID"] = CriteriaOperator.Parse("[Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived'");
                                            List<string> listpoid = new List<string>();
                                            foreach (Requisition reqobjvendor in cs.List)
                                            {
                                                if (!listpoid.Contains(reqobjvendor.POID.POID))
                                                {
                                                    listpoid.Add(reqobjvendor.POID.POID);
                                                }
                                            }
                                            var count = listpoid.Count;
                                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                            if (count > 0)
                                            {
                                                subchild.Caption = cap[0] + " (" + count + ")";
                                            }
                                            else
                                            {
                                                subchild.Caption = cap[0];
                                            }
                                        }
                                        else if (subchild.Id == "Distribution")
                                        {
                                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("[LT] Is Null"));
                                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                            int intReceive = 0;
                                            CriteriaOperator criteria = CriteriaOperator.Parse("[LT] Is Null");
                                            IList<Distribution> req = ObjectSpace.GetObjects<Distribution>(criteria);
                                            string[] ReceiveID = new string[req.Count];
                                            foreach (Distribution item in req)
                                            {
                                                if (!ReceiveID.Contains(item.ReceiveID))
                                                {
                                                    ReceiveID[intReceive] = item.ReceiveID;
                                                    intReceive = intReceive + 1;
                                                }
                                            }
                                            if (count > 0)
                                            {
                                                subchild.Caption = cap[0] + " (" + intReceive + ")";
                                            }
                                            else
                                            {
                                                subchild.Caption = cap[0];
                                            }
                                        }
                                        else if (subchild.Id == "Disposal")
                                        {
                                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            //var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] = 'PendingDispose' OR [Status] = 'PendingConsume') And [ExpiryDate] <= ?", DateTime.Now));
                                            var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] = 3 OR [Status] = 1) And [ExpiryDate] < ?", DateTime.Today));
                                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                            if (count > 0)
                                            {
                                                subchild.Caption = cap[0] + " (" + count + ")";
                                            }
                                            else
                                            {
                                                subchild.Caption = cap[0];
                                            }
                                        }
                                        else if (subchild.Id == "VendorReagentCertificate")
                                        {
                                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            var count = objectSpace.GetObjectsCount(typeof(VendorReagentCertificate), CriteriaOperator.Parse("[LoadedDate] IS NULL AND [LoadedBy] IS NULL"));
                                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                            if (count > 0)
                                            {
                                                subchild.Caption = cap[0] + " (" + count + ")";
                                            }
                                            else
                                            {
                                                subchild.Caption = cap[0];
                                            }
                                        }
                                    }
                                }
                                else if (child.Id == "Alert")
                                {
                                    foreach (ChoiceActionItem subchild in child.Items)
                                    {
                                        if (subchild.Id == "StockAlert")
                                        {
                                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            var count = objectSpace.GetObjectsCount(typeof(Items), CriteriaOperator.Parse("[StockQty] <= [AlertQty]"));
                                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                            if (count > 0)
                                            {
                                                subchild.Caption = cap[0] + " (" + count + ")";
                                            }
                                            else
                                            {
                                                subchild.Caption = cap[0];
                                            }
                                        }
                                        else if (subchild.Id == "Expiration")
                                        {
                                            DateTime TodayDate = DateTime.Now;
                                            TodayDate = TodayDate.AddDays(7);
                                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] == 3 OR [Status] == 1) And [ExpiryDate] <= ?", TodayDate));
                                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                            if (count > 0)
                                            {
                                                subchild.Caption = cap[0] + " (" + count + ")";
                                            }
                                            else
                                            {
                                                subchild.Caption = cap[0];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    IObjectSpace savedListViewObjectSpace = Application.CreateObjectSpace();
                    CollectionSource collectionSource = new CollectionSource(savedListViewObjectSpace, typeof(Items));
                    ListView itemListview = Application.CreateListView("Items_ListView", collectionSource, true);
                    Frame.SetView(itemListview);
                    // Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages", "ItemsImported"), InformationType.Success, 3000, InformationPosition.Top);

                }
                //else if(paramsplit.Count() ==2)
                //{
                //    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                //    if (editor != null && editor.Grid != null)
                //    {
                //        object currentOid = editor.Grid.GetRowValues(int.Parse(paramsplit[1]), "Oid");
                //        ItemRequistionListInfo.ItemOid = new Guid(currentOid.ToString());
                //        CollectionSource cs = new CollectionSource(Application.CreateObjectSpace(), typeof(Items));
                //        ListView lv = Application.CreateListView("Items_ListView_Copy_Requisition", cs, false);
                //        ShowViewParameters showViewParameters = new ShowViewParameters();
                //        showViewParameters.CreatedView = lv;
                //        showViewParameters.Context = TemplateContext.PopupWindow;
                //        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                //        DialogController dc = Application.CreateController<DialogController>();
                //        dc.SaveOnAccept = false;
                //        dc.CloseOnCurrentObjectProcessing = false;
                //        dc.Accepting += Dc_Accepting;
                //        showViewParameters.Controllers.Add(dc);
                //        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                //    }
                //}
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "shippingnull"), InformationType.Error, timer.Seconds, InformationPosition.Top);
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
            //try
            //{

            //        IObjectSpace os = Application.CreateObjectSpace();
            //        if (View.Id == "Requisition_ListView")
            //        {
            //        Requisition objRequistion = os.GetObjectByKey<Requisition>(ItemRequistionListInfo.ItemOid);
            //        if (objRequistion != null)
            //        {
            //            List<Items> lstDepartmentOids = e.AcceptActionArgs.SelectedObjects.Cast<Items>().ToList();
            //            if (lstDepartmentOids != null && lstDepartmentOids.Count > 0)
            //            {
            //                foreach (Items objDepartment in lstDepartmentOids)
            //                {
            //                    Items obj = os.GetObject<Items>(objDepartment);
            //                    objRequistion.Item = obj;
            //                    objRequistion.Vendor = obj.Vendor;
            //                    objRequistion.Catalog = obj.VendorCatName;


            //                }
            //            }

            //            os.CommitChanges();
            //            os.Dispose();
            //            ObjectSpace.Refresh();
            //        }


            //    }

            //}
            //catch(Exception ex)
            //{
            //    Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            //    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            //}
        }

        private void removeItemsAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Items currentItem = (Items)e.CurrentObject;

                currentItem.Delete();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void exportitemsstock_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                string strLocalFile = HttpContext.Current.Server.MapPath(@"~\itemtemplate.xlsx");
                if (File.Exists(strLocalFile) == true)
                {
                    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=itemtemplate.xlsx");
                    HttpContext.Current.Response.BinaryWrite(File.ReadAllBytes(strLocalFile));
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
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
