using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Module.Controllers.ICM.CopyToAllCell
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DistributionCopyToAllCallViewController : ViewController
    {
        ASPxGridListEditor GridListEditor;
        MessageTimer timer = new MessageTimer();
        public DistributionCopyToAllCallViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "Distribution_ListView;" + "Requisition_ListViewEntermode;" + "Distribution_ListView_StockQtyEdit;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                // Access and customize the target View control.
                if (View.Id == "Distribution_ListView" || View.Id == "Requisition_ListViewEntermode")
                {
                    GridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (GridListEditor != null)
                    {
                        GridListEditor.Grid.FillContextMenuItems += Grid_FillContextMenuItems;
                        GridListEditor.Grid.SettingsContextMenu.Enabled = true;
                        GridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                        GridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                        {                        
                            sessionStorage.setItem('DistributionFocusedColumn', null); 
                            //if((e.cellInfo.column.name.indexOf('Command') !== -1) || (e.cellInfo.column.name == 'Edit'))
                            if(e.cellInfo.column.name.indexOf('Command') !== -1)
                            {                              
                                e.cancel = true;
                            }
                            else if (e.cellInfo.column.fieldName == 'ExpPrice')
                            {
                                e.cancel = true;
                            }    
                            else if (e.cellInfo.column.fieldName == 'Item.Specification' ||e.cellInfo.column.fieldName == 'Item.Size' ||e.cellInfo.column.fieldName == 'Catalog' ||e.cellInfo.column.fieldName == 'Vendor.Vendor'
                            ||e.cellInfo.column.fieldName == 'Item.Unit.Option'||e.cellInfo.column.fieldName == 'grade'||e.cellInfo.column.fieldName == 'Item.StockQty')
                            {
                                e.cancel = true;
                            }                        
                            else
                            {
                                var fieldName = e.cellInfo.column.fieldName;                       
                                sessionStorage.setItem('DistributionFocusedColumn', fieldName);  
                            }
                                          
                        }";
                        if (View.Id == "Requisition_ListViewEntermode")
                        {
                            GridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                                {                      
                                var FocusedColumn = sessionStorage.getItem('DistributionFocusedColumn');                                
                                var oid;
                                var text;
                                if(FocusedColumn.includes('.'))
                                {                                       
                                    oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                    text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;                                                     
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        {                                                                                 
                                            s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);                                                                                 
                                         }
                                     }        
                                 }
                                 else
                                 {                                                             
                                    var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        {   
                                                if(FocusedColumn=='OrderQty')
                                                {
                                                  var unprice = s.batchEditApi.GetCellValue(i,'UnitPrice');  
                                                    if(unprice != null){
                                                    var tempprice = CopyValue * unprice; 
                                                    tempprice = Math.round(tempprice * 100) / 100; 
                                                    s.batchEditApi.SetCellValue(i,'ExpPrice',tempprice); 
                                                   }
                                                }                                          
                                            s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                        }
                                    }                            
                                 }
                             e.processOnServer = false;
                        }";
                        }
                        else
                        {
                            GridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                        { 
                            if (s.IsRowSelectedOnPage(e.elementIndex))  
                            { 
                                var FocusedColumn = sessionStorage.getItem('DistributionFocusedColumn');                                
                                var oid;
                                var text;
                                if(FocusedColumn.includes('.'))
                                {                                       
                                    oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                    text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;                                                     
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        { 
                                            if (s.IsRowSelectedOnPage(i)) 
                                            {
                                               if(FocusedColumn=='Storage.Oid')
                                               {
                                                    s.batchEditApi.SetCellValue(i,'givento.Oid',null,null,false);
                                                    s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);
                                               }
                                               else if(FocusedColumn=='givento.Oid')
                                               {
                                                    s.batchEditApi.SetCellValue(i,'Storage.Oid',null,null,false);
                                                    s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);
                                               }
                                               else
                                               {
                                                    s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);
                                               }
                                           
                                            }
                                         }
                                     }        
                                 }
                                 else
                                 {                                                             
                                    var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
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
                        }
                    }
                }
                else if (View.Id == "Distribution_ListView_StockQtyEdit")
                {
                    GridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (GridListEditor != null)
                    {
                        GridListEditor.Grid.FillContextMenuItems += Grid_FillContextMenuItems;
                        GridListEditor.Grid.SettingsContextMenu.Enabled = true;
                        GridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                        GridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s, e) 
                        {
                            var fieldName = e.cellInfo.column.fieldName;
                            sessionStorage.setItem('ItemFocusedColumn', fieldName);
                        }";
                        GridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                        { 
                            if (s.IsRowSelectedOnPage(e.elementIndex))  
                            { 
                                var FocusedColumn = sessionStorage.getItem('ItemFocusedColumn');                                
                                var oid;
                                var text;
                                if(FocusedColumn.includes('.'))
                                {                                       
                                    oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                    text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText; 
                                    if (e.item.name =='CopyToAllCell' )
                                    {
                                       if (FocusedColumn=='VendorLT' || FocusedColumn=='ExpiryDate' || FocusedColumn=='Storage.Oid')
	                                   {
		                                 for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        { 
                                            if (s.IsRowSelectedOnPage(i)) 
                                            {
                                               s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);   
                                            }
                                         } 
	                                  }
                                     }        
                                 }
                                 else
                                 {                                                             
                                    var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn); 
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        if (FocusedColumn=='VendorLT' || FocusedColumn=='ExpiryDate' || FocusedColumn=='Storage.Oid')
                                        {
                                        for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        { 
                                            if (s.IsRowSelectedOnPage(i)) 
                                            {
                                                s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                            }
                                        }
                                      }
                                    }                            
                                 }
                             }
                             e.processOnServer = false;
                        }";
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Grid_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {
            try
            {

                if (e.MenuType == GridViewContextMenuType.Rows)
                {
                    e.Items.Add("Copy To All Cell", "CopyToAllCell");
                    e.Items.Remove(e.Items.FindByText("Edit"));
                    GridViewContextMenuItem Edititem = e.Items.FindByName("EditRow");
                    if (Edititem != null)
                        Edititem.Visible = false;
                    GridViewContextMenuItem item = e.Items.FindByName("CopyToAllCell");
                    if (item != null)
                        item.Image.IconID = "edit_copy_16x16office2013";//"navigation_home_16x16";
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
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
