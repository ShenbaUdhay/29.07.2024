using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.Drawing;

namespace LDM.Module.Controllers.TestParameter
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class QCTestParameterController : ViewController
    {
        #region Declaration
        string strtmOid = string.Empty;
        MessageTimer timer = new MessageTimer();
        ASPxGridListEditor GridListEditor;
        #endregion

        #region Constructor
        public QCTestParameterController()
        {
            InitializeComponent();
            this.TargetViewId = "QcParameter_DetailView;" + "QCTestParameter_ListView_Copy;" + "TestMethod_LookupListView_Copy_QCParameter;" + "Parameter_LookupListView_Copy_QCParameter;";
        }
        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                if (View != null && View.Id == "QCTestParameter_ListView_Copy")
                {
                    //ASPxGridView gridView = (((ListView)View).Editor as ASPxGridListEditor).Grid;
                    GridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (GridListEditor != null)
                    {
                        ASPxGridView gridView = GridListEditor.Grid;
                        if (gridView != null)
                        {
                            gridView.Load += gridView_Load;

                            gridView.FillContextMenuItems += GridView_FillContextMenuItems;
                            gridView.SettingsContextMenu.Enabled = true;
                            gridView.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                            gridView.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                            {   
                                sessionStorage.setItem('QTPFocusedColumn', null);  
                                if((e.cellInfo.column.name.indexOf('Command') !== -1) || (e.cellInfo.column.name == 'Edit'))
                                {                              
                                    e.cancel = true;
                                }
                                else if (e.cellInfo.column.fieldName == 'Parameter.ParameterName')
                                {                         
                                    e.cancel = true;
                                }                        
                                else
                                {
                                    var fieldName = e.cellInfo.column.fieldName;                       
                                    sessionStorage.setItem('QTPFocusedColumn', fieldName);  
                                }                                         
                            }";

                            gridView.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                            { 
                                if (s.IsRowSelectedOnPage(e.elementIndex))  
                                { 
                                    var FocusedColumn = sessionStorage.getItem('QTPFocusedColumn');                                
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
                                                    s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);
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
                if (View != null && View.Id == "TestMethod_LookupListView_Copy_QCParameter")
                {
                    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse(" [GCRecord] IS NULL AND (([RetireDate] IS NULL OR [RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')) AND " +
                           " ([MethodName.RetireDate] IS NULL OR [MethodName.RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')");
                }
                if (View != null && View.Id == "Parameter_LookupListView_Copy_QCParameter")
                {
                    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse(" [GCRecord] IS NULL AND (([RetireDate] IS NULL OR [RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "'))  ");

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
        }
        #endregion

        private void DefaultQCTestParameter_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "QcParameter_DetailView")
                {
                    if (View.CurrentObject != null)
                    {
                        strtmOid = ObjectSpace.GetKeyValueAsString((QcParameter)View.CurrentObject);
                    }
                }
                IObjectSpace objspace = Application.CreateObjectSpace();
                object objToShow = objspace.CreateObject(typeof(QCTestParameter));
                if (objspace != null)
                {
                    CollectionSource cs = new CollectionSource(objspace, typeof(QCTestParameter));
                    cs.Criteria.Clear();
                    cs.Criteria["filter"] = CriteriaOperator.Parse("[QcParameter]='" + strtmOid + "'");
                    ListView CreateListView = Application.CreateListView("QCTestParameter_ListView_Copy", cs, true);
                    e.Size = new Size(1200, 600);
                    e.View = CreateListView;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        void gridView_Load(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                //gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                gridView.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
                foreach (WebColumnBase column in gridView.Columns)
                {
                    IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)GridListEditor).GetColumnInfo(column);
                    if (columnInfo != null)
                    {
                        IModelColumn modelColumn = (IModelColumn)columnInfo.Model;
                        column.Width = modelColumn.Width;
                    }
                }
                gridView.VisibleColumns[0].FixedStyle = GridViewColumnFixedStyle.Left;
                gridView.VisibleColumns["Test"].FixedStyle = GridViewColumnFixedStyle.Left;
                gridView.VisibleColumns["Method"].FixedStyle = GridViewColumnFixedStyle.Left;
                gridView.VisibleColumns["QCType"].FixedStyle = GridViewColumnFixedStyle.Left;
                gridView.VisibleColumns["Parameter"].FixedStyle = GridViewColumnFixedStyle.Left;

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
                    e.Items.Add("Copy To All Cell", "CopyToAllCell");
                    GridViewContextMenuItem Edititem = e.Items.FindByName("EditRow");
                    if (Edititem != null)
                        Edititem.Visible = false;
                    GridViewContextMenuItem item = e.Items.FindByName("CopyToAllCell");
                    if (item != null)
                        item.Image.IconID = "edit_copy_16x16office2013";//"navigation_home_16x16";
                }
                //throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
