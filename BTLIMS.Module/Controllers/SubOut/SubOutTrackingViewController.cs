using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.Collections.Generic;

namespace LDM.Module.Controllers.SubOutTracking
{
    public partial class SubOutTrackingViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();

        public SubOutTrackingViewController()
        {
            InitializeComponent();
            TargetViewId = "SampleParameter_ListView_Copy_SubOut;" + "SampleParameter_ListView_Copy_SubOut_Viewmode";
            SubOut.ExecuteCompleted += SubOut_ExecuteCompleted;
            SubOutview.TargetViewId = "SampleParameter_ListView_Copy_SubOut";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
        }

        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                if (base.View != null && base.View.Id == "SampleParameter_ListView_Copy_SubOut")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                    {
                        lstview.Criteria = CriteriaOperator.Parse("[SubOut] = True And [SubLabName] Is Null");
                        //And [SubOutBy] Is Null And [SubOutDate] Is Null
                        lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                        lstview.Properties.Add(new ViewProperty("SampleNo", SortDirection.Ascending, "Samplelogin.SampleNo", true, true));
                        lstview.Properties.Add(new ViewProperty("TestName", SortDirection.Ascending, "Testparameter.TestMethod.TestName", true, true));
                        lstview.Properties.Add(new ViewProperty("TOid", SortDirection.Ascending, "MAX(Oid)", false, true));
                        List<object> groups = new List<object>();
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["TOid"]);
                        ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", groups);
                    }
                }
                else if (View.Id == "SampleParameter_ListView_Copy_SubOut_Viewmode")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                    {
                        lstview.Criteria = CriteriaOperator.Parse("[SubOut] = True And [SubLabName] Is Not Null");
                        //And [SubOutBy] Is Not Null And [SubOutDate] Is Not Null
                        lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                        lstview.Properties.Add(new ViewProperty("SampleNo", SortDirection.Ascending, "Samplelogin.SampleNo", true, true));
                        lstview.Properties.Add(new ViewProperty("TestName", SortDirection.Ascending, "Testparameter.TestMethod.TestName", true, true));
                        lstview.Properties.Add(new ViewProperty("TOid", SortDirection.Ascending, "MAX(Oid)", false, true));
                        List<object> groups = new List<object>();
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["TOid"]);
                        ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", groups);
                    }
                }

                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                ASPxGridView gv = gridListEditor.Grid;
                gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                if (gridListEditor.Grid != null)
                {
                    WebWindow.CurrentRequestWindow.RegisterClientScript("user", "sessionStorage.setItem('curuser', '" + SecuritySystem.CurrentUserId + "');");
                    WebWindow.CurrentRequestWindow.RegisterClientScript("user1", "sessionStorage.setItem('curuser1', '" + SecuritySystem.CurrentUserName + "')");
                    gridListEditor.Grid.FillContextMenuItems += Grid_FillContextMenuItems;
                    gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                    gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                    gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                    {     
                        sessionStorage.setItem('SuboutFocusedColumn', null);            
                        if (e.cellInfo.column.fieldName == 'SubOutBy.Oid' || e.cellInfo.column.fieldName == 'SubOutDate' || e.cellInfo.column.fieldName == 'SubLabName.Oid')
                        {                          
                            var fieldName = e.cellInfo.column.fieldName;                       
                            sessionStorage.setItem('SuboutFocusedColumn', fieldName);  
                            e.cancel = false;
                        }                                              
                        else
                        {
                            e.cancel = true;
                        }                                         
                    }";
                    gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                      {      
                            var FocusedColumn = sessionStorage.getItem('SuboutFocusedColumn');      
                            var oid;
                            var text; 
                            if(FocusedColumn.includes('.'))
                            { 
                            oid = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                            text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;                                                     
                            if (e.item.name =='CopyToAllCell')
                            {                                
                                for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                {  
                                     if (s.IsRowSelectedOnPage(i)) 
                                     {
                                       s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);                                                                                 }
                                     }                                 
                                } 
                             }
                             else{
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
                            e.processOnServer = false;
                      }";
                    if (base.View != null && base.View.Id == "SampleParameter_ListView_Copy_SubOut")
                    {
                        gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s, e) {                   
                    if (e.visibleIndex == -1 && !s.IsRowSelectedOnPage(0)) {
                    //selectall checkbox unselected 
                    for (var i = 0 ; i <= s.GetVisibleRowsOnPage() - 1; i++){
                    s.batchEditApi.ResetChanges(i);
                    }}
                    else if(e.visibleIndex == -1 && s.IsRowSelectedOnPage(0)){
                    //selectall checkbox selected 
                    var curusr = sessionStorage.getItem('curuser');
                    var curusr1 = sessionStorage.getItem('curuser1');                   
                    var today = new Date();       
                    for (var i = 0 ; i <= s.GetVisibleRowsOnPage() - 1; i++){
                    s.batchEditApi.SetCellValue(i, 'SubOutDate', today);          
                    s.batchEditApi.SetCellValue(i, 'SubOutBy', curusr, curusr1, false);
                    }}
                    else{                    
                    if (s.IsRowSelectedOnPage(e.visibleIndex)) {                    
                    //single checkbox selected 
                    var curusr = sessionStorage.getItem('curuser');
                    var curusr1 = sessionStorage.getItem('curuser1');                 
                    var today = new Date();                     
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'SubOutDate', today);
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'SubOutBy', curusr, curusr1, false);
                    }
                    else{
                    //single checkbox unselected 
                    s.batchEditApi.ResetChanges(e.visibleIndex);
                    }}}";
                    }
                    else
                    {
                        gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s, e) { 
                    if (e.visibleIndex == -1 && !s.IsRowSelectedOnPage(0)) {
                    //selectall checkbox unselected 
                    for (var i = 0 ; i <= s.GetVisibleRowsOnPage() - 1; i++){
                    s.batchEditApi.ResetChanges(i);
                    }}
                    else if (!s.IsRowSelectedOnPage(e.visibleIndex))
                    {
                    s.batchEditApi.ResetChanges(e.visibleIndex);
                    }}";
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
            if (e.MenuType == GridViewContextMenuType.Rows)
            {
                e.Items.Add("Copy To All Cell", "CopyToAllCell");
                e.Items.Remove(e.Items.FindByText("Edit"));
                GridViewContextMenuItem item = e.Items.FindByName("CopyToAllCell");
                if (item != null)
                    item.Image.IconID = "edit_copy_16x16office2013";//"navigation_home_16x16";
            }
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        private void SubOut_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count > 0)
                {
                    ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                }
                foreach (Modules.BusinessObjects.SampleManagement.SampleParameter obj in View.SelectedObjects)
                {
                    string testname = obj.Testparameter.TestMethod.TestName;
                    Guid sloid = obj.Samplelogin.Oid;
                    CriteriaOperator criteria = CriteriaOperator.Parse("[Samplelogin]='" + sloid + "' AND [Testparameter.TestMethod.TestName]='" + testname + "'");
                    IList<SampleParameter> splst = ObjectSpace.GetObjects<SampleParameter>(criteria);
                    if (splst != null)
                    {
                        foreach (SampleParameter sp in splst)
                        {
                            sp.SubLabName = obj.SubLabName;
                            sp.SubOutBy = obj.SubOutBy;
                            sp.SubOutDate = obj.SubOutDate;
                            ObjectSpace.CommitChanges();
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

        private void SubOut_ExecuteCompleted(object sender, ActionBaseEventArgs e)
        {
            try
            {
                WebWindow.CurrentRequestWindow.RegisterStartupScript("gridRefresh", "function sam(s, e){ Grid.Refresh();} ");
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "suboutupdate"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                if (View.Id == "SampleParameter_ListView_Copy_SubOut_Viewmode")
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[SubOut] = True And [SubLabName] Is Not Null");
                }
                else
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[SubOut] = True And [SubLabName] Is Null");
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SubOutview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objectSpace, typeof(SampleParameter));
                WebWindow.CurrentRequestWindow.RegisterClientScript("clearsession", "sessionStorage.removeItem('SuboutFocusedColumn');");
                Frame.SetView(Application.CreateListView("SampleParameter_ListView_Copy_SubOut_Viewmode", cs, true));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
