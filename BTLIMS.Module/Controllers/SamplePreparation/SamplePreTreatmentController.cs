using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.FileAttachments.Web;
using DevExpress.ExpressApp.Office.Web;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Web.ASPxRichEdit;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SampleManagement.SamplePreparation;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace LDM.Module.Controllers.SamplePreparation
{
    public partial class SamplePreTreatmentController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        //string CurrentLanguage = string.Empty;
        curlanguage objLanguage = new curlanguage();

        //bool IsNew = false;
        public SamplePreTreatmentController()
        {
            InitializeComponent();
            TargetViewId = "SamplePrepTemplates_ListView;" + "SamplePrepTemplates_DetailView;" + "SamplePretreatmentBatch_ListView;" +
                           "SamplePrepTemplateFields_Templates_ListView;" + "SamplePrepTemplateFields_LookupListView;" +
                           "SamplePretreatmentBatch_SamplePretreatmentBatchSeqDetail_ListView;" + "SamplePretreatmentBatch_DetailView;" + "Testparameter_LookupListView_Copy_SampleLogin_Copy_Parameter;" + "Testparameter_LookupListView_Copy_SampleLogin;" + "Testparameter_LookupListView_Copy_SampleLogin_Copy;";
            SamplePreTreatmentLoad.TargetViewId = "SamplePretreatmentBatch_DetailView;";
            SamplePreTreatmentReset.TargetViewId = "SamplePretreatmentBatch_DetailView;";
            SamplePreTreatmentSort.TargetViewId = "SamplePretreatmentBatch_DetailView;";
            SamplePreTreatmentPrevious.TargetViewId = "SamplePretreatmentBatch_DetailView;";
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                //SelectedData sproc = ((XPObjectSpace)(ObjectSpace)).Session.ExecuteSproc("getCurrentLanguage", "");
                //CurrentLanguage = sproc.ResultSet[1].Rows[0].Values[0].ToString();

                if (View.Id == "SamplePrepTemplates_ListView")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    List<string> lstExistingProps = os.GetObjects<SamplePrepTemplateFields>().Select(i => i.FieldName).Distinct().ToList();
                    TypeInfo info = typeof(SamplePretreatmentBatchSequence).GetTypeInfo();
                    List<string> lstAllProps = typeof(SamplePretreatmentBatchSequence).GetProperties().Where(i => i.Name != "ClassInfo" && i.Name != "IsDeleted" &&
                    i.Name != "IsLoading" && i.Name != "Loading" && i.Name != "Oid" && i.Name != "Session" && i.Name != "This" && i.Name != "Sort" &&
                    i.Name != "SamplePretreatmentBatchDetail").Select(i => i.Name).Distinct().ToList();
                    if (lstAllProps != null && lstAllProps.Count > 0)
                    {
                        if (lstExistingProps != null && lstExistingProps.Count > 0)
                        {
                            lstAllProps = lstAllProps.Except(lstExistingProps).ToList();
                        }
                        bool CanCommit = false;
                        foreach (string strProp in lstAllProps)
                        {
                            SamplePrepTemplateFields objField = os.FindObject<SamplePrepTemplateFields>(CriteriaOperator.Parse("[FieldName] = ?", strProp));
                            if (objField == null)
                            {
                                objField = os.CreateObject<SamplePrepTemplateFields>();
                                objField.FieldName = strProp;
                                CanCommit = true;
                            }
                        }
                        if (CanCommit)
                        {
                            os.CommitChanges();
                        }
                        os.Dispose();
                    }
                }
                else if (View.Id == "SamplePretreatmentBatch_DetailView")
                {
                    ASPxRichTextPropertyEditor RichText = ((DetailView)View).FindItem("Comment") as ASPxRichTextPropertyEditor;
                    if (RichText != null)
                    {
                        RichText.ControlCreated += RichText_ControlCreated;
                    }
                    SamplePreTreatmentPrevious.Enabled.SetItemValue("Key", false);
                    Frame.GetController<ModificationsController>().SaveAction.Active.SetItemValue("ShowSaveAction", false);
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Active.SetItemValue("ShowSaveAndNewAction", false);
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.SetItemValue("ShowSaveAndCloseAction", false);
                    Frame.GetController<ModificationsController>().CancelAction.Active.SetItemValue("ShowCancel", false);
                    Frame.GetController<RefreshController>().RefreshAction.Executing += RefreshAction_Executing;

                    if (View.ObjectSpace.IsNewObject(View.CurrentObject))
                    {
                        //IsNew = true;
                        if (objLanguage.strcurlanguage == "En")
                        {
                            SamplePreTreatmentSort.Caption = "Sort";
                        }
                        else
                        {
                            SamplePreTreatmentSort.Caption = "序号";
                        }
                    }
                    else
                    {
                        //IsNew = false;
                        if (objLanguage.strcurlanguage == "En")
                        {
                            SamplePreTreatmentSort.Caption = "Ok";
                        }
                        else
                        {
                            SamplePreTreatmentSort.Caption = "确定";
                        }
                    }
                }
                else if (View.Id == "SamplePretreatmentBatch_SamplePretreatmentBatchSeqDetail_ListView")
                {
                    Frame.GetController<ExportController>().ExportAction.Active["ShowExport"] = false;
                }
                else if (View.Id == "SamplePretreatmentBatch_ListView")
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RichText_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                ASPxRichEdit RichEdit = ((ASPxRichTextPropertyEditor)sender).ASPxRichEditControl;
                if (RichEdit != null)
                {
                    //RichEdit.RibbonTabs.FindByName("File").Visible = false;
                    RichEdit.RibbonTabs[0].Visible = false;
                    RichEdit.RibbonTabs[2].Visible = false;
                    RichEdit.RibbonTabs[3].Visible = false;
                    RichEdit.RibbonTabs[4].Visible = false;
                    RichEdit.RibbonTabs[5].Visible = false;
                    RichEdit.RibbonTabs[6].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RefreshAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.ObjectSpace.IsNewObject(View.CurrentObject))
                {
                    SamplePreTreatmentReset.DoExecute();
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


                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (View is DetailView)
                {
                    foreach (ViewItem item in ((DetailView)View).Items)
                    {
                        if (item is ASPxStringPropertyEditor)
                        {
                            ASPxStringPropertyEditor editor = (ASPxStringPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }

                        }

                        else if (item is ASPxIntPropertyEditor)
                        {
                            ASPxIntPropertyEditor editor = (ASPxIntPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }

                        else if (item is ASPxDoublePropertyEditor)
                        {
                            ASPxDoublePropertyEditor editor = (ASPxDoublePropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }

                        else if (item is ASPxDecimalPropertyEditor)
                        {
                            ASPxDecimalPropertyEditor editor = (ASPxDecimalPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }

                        }

                        else if (item is ASPxDateTimePropertyEditor)
                        {
                            ASPxDateTimePropertyEditor editor = (ASPxDateTimePropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxLookupPropertyEditor)
                        {
                            ASPxLookupPropertyEditor editor = (ASPxLookupPropertyEditor)item;
                            if (editor != null && editor.DropDownEdit != null && editor.DropDownEdit.DropDown != null)
                            {
                                editor.DropDownEdit.DropDown.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxBooleanPropertyEditor)
                        {
                            ASPxBooleanPropertyEditor editor = (ASPxBooleanPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxEnumPropertyEditor)
                        {
                            ASPxEnumPropertyEditor editor = (ASPxEnumPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxGridLookupPropertyEditor)
                        {
                            ASPxGridLookupPropertyEditor editor = (ASPxGridLookupPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                    }
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




                //if (View.Id == "Testparameter_LookupListView_Copy_SampleLogin_Copy_Parameter")
                //{
                //    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                //    //gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                //    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                //    gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                //    //gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                //    //ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                //    //selparameter.CallbackManager.RegisterHandler("SamplePreTreatment", this);
                //    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                //    {
                //        //var nav = document.getElementById('LPcell');
                //        //var sep = document.getElementById('separatorCell');
                //        //if(nav != null && sep != null) {
                //        //   var totusablescr = screen.width - (sep.offsetWidth + nav.offsetWidth);
                //        //   s.SetWidth((totusablescr / 100) * 90);         
                //        //}
                //        //else {
                //            s.SetWidth(400); 
                //        //}                  
                //    }";
                //}

                else if (View.Id == "Testparameter_LookupListView_Copy_SampleLogin")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    //gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                    //gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    //ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    //selparameter.CallbackManager.RegisterHandler("SamplePreTreatment", this);
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                    {
                        //var nav = document.getElementById('LPcell');
                        //var sep = document.getElementById('separatorCell');
                        //if(nav != null && sep != null) {
                        //   var totusablescr = screen.width - (sep.offsetWidth + nav.offsetWidth);
                        //   s.SetWidth((totusablescr / 100) * 90);         
                        //}
                        //else {
                         //   s.SetWidth(400); 
                        //}                  
                    }";
                }

                else if (View.Id == "Testparameter_LookupListView_Copy_SampleLogin_Copy")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    //gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                    //gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    //ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    //selparameter.CallbackManager.RegisterHandler("SamplePreTreatment", this);
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                    {
                        //var nav = document.getElementById('LPcell');
                        //var sep = document.getElementById('separatorCell');
                        //if(nav != null && sep != null) {
                        //   var totusablescr = screen.width - (sep.offsetWidth + nav.offsetWidth);
                        //   s.SetWidth((totusablescr / 100) * 90);         
                        //}
                        //else {
                          //  s.SetWidth(400); 
                        //}                  
                    }";
                }

                else if (View.Id == "SamplePretreatmentBatch_SamplePretreatmentBatchSeqDetail_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 265;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    selparameter.CallbackManager.RegisterHandler("SamplePreTreatment", this);
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                    {
                        var nav = document.getElementById('LPcell');
                        var sep = document.getElementById('separatorCell');
                        if(nav != null && sep != null) {
                           var totusablescr = screen.width - (sep.offsetWidth + nav.offsetWidth);
                           s.SetWidth((totusablescr / 100) * 90);         
                        }
                        else {
                            s.SetWidth(900); 
                        }                  
                    }";
                    gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                      if (e.visibleIndex != '-1')
                      {
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {  
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {                             
                            RaiseXafCallback(globalCallbackControl, 'SamplePreTreatment', 'Selected|' + Oidvalue , '', false);    
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'SamplePreTreatment', 'UNSelected|' + Oidvalue, '', false);    
                         }
                        }); 
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                      {        
                        RaiseXafCallback(globalCallbackControl, 'SamplePreTreatment', 'Selectall', '', false);     
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                      {
                        RaiseXafCallback(globalCallbackControl, 'SamplePreTreatment', 'UNSelectall', '', false);                        
                      }                      
                    }";
                    gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Load += Grid_Load;
                }
                else if (View.Id == "SamplePretreatmentBatch_DetailView" && View.CurrentObject != null && !View.ObjectSpace.IsNewObject(View.CurrentObject))
                {
                    disablecontrols(enbstat: false, View, IsNewObj: false);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                if (Frame is NestedFrame)
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    if (nestedFrame != null && nestedFrame.ViewItem != null && nestedFrame.ViewItem.View != null)
                    {
                        CompositeView cv = nestedFrame.ViewItem.View;
                        if (cv != null)
                        {
                            SamplePretreatmentBatch batch = (SamplePretreatmentBatch)cv.CurrentObject;
                            if (batch != null && !cv.ObjectSpace.IsNewObject(batch) && batch.TemplateID != null)
                            {
                                DevExpress.Xpo.XPCollection<SamplePrepTemplateFields> templateFields = batch.TemplateID.SelectedFields;
                                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                                if (gridListEditor != null && gridListEditor.Grid != null)
                                {
                                    ASPxGridView gridView = (ASPxGridView)gridListEditor.Grid;
                                    if (gridView != null)
                                    {
                                        foreach (WebColumnBase column in gridView.Columns)
                                        {
                                            if (column.Name == "SelectionCommandColumn")
                                            {
                                                column.Visible = false;
                                            }
                                            else
                                            {
                                                IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                                                if (columnInfo != null)
                                                {
                                                    if (columnInfo.Model.Id == "NPSampleID" || templateFields.FirstOrDefault(i => i.FieldName == columnInfo.Model.Id) != null)
                                                    {
                                                        if (columnInfo.Model.Id != "SampleID")
                                                        {
                                                            column.Visible = true;
                                                        }
                                                        else
                                                        {
                                                            column.Visible = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        column.Visible = false;
                                                    }
                                                }
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

        protected override void OnDeactivated()
        {
            try
            {
                base.OnDeactivated();
                if (View.Id == "SamplePretreatmentBatch_DetailView")
                {
                    Frame.GetController<RefreshController>().RefreshAction.Executing -= RefreshAction_Executing;
                    if (Frame.GetController<ModificationsController>().SaveAction.Active.Contains("ShowSaveAction"))
                    {
                        Frame.GetController<ModificationsController>().SaveAction.Active.RemoveItem("ShowSaveAction");
                    }
                    if (Frame.GetController<ModificationsController>().SaveAndNewAction.Active.Contains("ShowSaveAndNewAction"))
                    {
                        Frame.GetController<ModificationsController>().SaveAndNewAction.Active.RemoveItem("ShowSaveAndNewAction");
                    }
                    if (Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.Contains("ShowSaveAndCloseAction"))
                    {
                        Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.RemoveItem("ShowSaveAndCloseAction");
                    }
                    if (Frame.GetController<ModificationsController>().CancelAction.Active.Contains("ShowCancel"))
                    {
                        Frame.GetController<ModificationsController>().CancelAction.Active.RemoveItem("ShowCancel");
                    }
                }
                else if (View.Id == "SamplePretreatmentBatch_SamplePretreatmentBatchSeqDetail_ListView")
                {
                    if (Frame.GetController<ExportController>().ExportAction.Active.Contains("ShowExport"))
                    {
                        Frame.GetController<ExportController>().ExportAction.Active.RemoveItem("ShowExport");
                    }
                }
                else if (View.Id == "SamplePretreatmentBatch_ListView")
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DeleteAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                bool IsDeleted = false;
                List<Guid> lstSampleLogin = new List<Guid>();
                foreach (SamplePretreatmentBatch batch in View.SelectedObjects.Cast<SamplePretreatmentBatch>().ToList())
                {
                    IList<SamplePretreatmentBatchSequence> lstBatch = ObjectSpace.GetObjects<SamplePretreatmentBatchSequence>(CriteriaOperator.Parse("[SamplePretreatmentBatchDetail.Oid]=?", batch.Oid));
                    IList<Modules.BusinessObjects.SampleManagement.SampleLogIn> lstSamples = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[SamplePretreatmentBatchID.SamplePretreatmentBatchDetail.Oid]=?", batch.Oid));
                    foreach (Modules.BusinessObjects.SampleManagement.SampleLogIn objSample in lstSamples)
                    {
                        objSample.SamplePretreatmentBatchID = null;
                    }
                    ObjectSpace.Delete(lstBatch);
                    ObjectSpace.Delete(batch);
                    IsDeleted = true;
                }
                if (IsDeleted)
                {
                    ObjectSpace.CommitChanges();
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "DeleteSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SamplePreTreatmentPrevious_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }

        private void SamplePreTreatmentSort_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (e.Action.Caption == "Sort" || e.Action.Caption == "序号")
                {
                    ListPropertyEditor samplePretreatmentList = ((DetailView)View).FindItem("SamplePretreatmentBatchSeqDetail") as ListPropertyEditor;
                    SamplePretreatmentBatch batch = (SamplePretreatmentBatch)View.CurrentObject;
                    if (batch != null && samplePretreatmentList != null && samplePretreatmentList.ListView != null)
                    {
                        if (samplePretreatmentList.ListView.SelectedObjects.Count > 0)
                        {
                            disablecontrols(enbstat: false, View, IsNewObj: true);
                            foreach (SamplePretreatmentBatchSequence sequences in (samplePretreatmentList.ListView.CollectionSource.List.Cast<SamplePretreatmentBatchSequence>().Where(i => i.Sort == 0).ToList()))
                            {
                                samplePretreatmentList.ListView.ObjectSpace.RemoveFromModifiedObjects(sequences);
                                samplePretreatmentList.ListView.CollectionSource.Remove(sequences);
                                samplePretreatmentList.ListView.Refresh();
                            }

                            ASPxGridListEditor gridListEditor = samplePretreatmentList.ListView.Editor as ASPxGridListEditor;
                            if (gridListEditor != null && gridListEditor.Grid != null)
                            {
                                gridListEditor.Grid.ClearSort();
                                gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns[e.Action.Caption], ColumnSortOrder.Ascending);
                                gridListEditor.Grid.Columns["SelectionCommandColumn"].Visible = false;
                                gridListEditor.Grid.Columns[e.Action.Caption].Visible = false;
                                gridListEditor.Grid.Selection.UnselectAll();
                            }

                            samplePretreatmentList.ListView.Refresh();

                            if (objLanguage.strcurlanguage == "En")
                            {
                                SamplePreTreatmentSort.Caption = "Ok";
                            }
                            else
                            {
                                SamplePreTreatmentSort.Caption = "确定";
                            }

                            WebWindow.CurrentRequestWindow.RegisterClientScript("AllowEnterWeighing", @"sessionStorage.setItem('CanEnterPreTreatment', 'Yes');");

                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Selectsample"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
                else if (e.Action.Caption == "Ok" || e.Action.Caption == "确定")
                {
                    ListPropertyEditor samplePretreatmentList = ((DetailView)View).FindItem("SamplePretreatmentBatchSeqDetail") as ListPropertyEditor;
                    SamplePretreatmentBatch batch = (SamplePretreatmentBatch)View.CurrentObject;
                    if (batch != null && samplePretreatmentList != null && samplePretreatmentList.ListView != null)
                    {
                        /* Below code has commented due to Prepbactch id format was changed */
                        /*CriteriaOperator qcct = CriteriaOperator.Parse("Max(SUBSTRING(PreTreatBatchID, 2))");
                        string tempID = (Convert.ToInt32(((XPObjectSpace)View.ObjectSpace).Session.Evaluate(typeof(SamplePretreatmentBatch), qcct, null)) + 1).ToString();
                        var curdate = DateTime.Now.ToString("yyMMdd");
                        if (tempID != "1")
                        {
                            var predate = tempID.Substring(0, 6);
                            if (predate == curdate)
                            {
                                tempID = "PT" + tempID;
                            }
                            else
                            {
                                tempID = "PT" + curdate + "01";
                            }
                        }
                        else
                        {
                            tempID = "PT" + curdate + "01";
                        }*/

                        // PretreatmentBatch id format-"PB +YYMMDD + UserID + Seq
                        var curdate = DateTime.Now.ToString("yyMMdd");
                        string userid = ((Employee)SecuritySystem.CurrentUser).UserID.ToString("000");
                        string tempID = string.Empty;
                        //IList<SamplePretreatmentBatch> SamplePretreament = ((XPObjectSpace)View.ObjectSpace).GetObjects<SamplePretreatmentBatch>(CriteriaOperator.Parse("SUBSTRING([PreTreatBatchID], 2, 9)=?", curdate + userid));
                        //if (SamplePretreament.Count > 0)
                        //{
                        //    SamplePretreament = SamplePretreament.OrderBy(a => a.PreTreatBatchID).ToList();
                        //    tempID = "PT" + curdate + userid + (Convert.ToInt32(SamplePretreament[SamplePretreament.Count - 1].PreTreatBatchID.Substring(11, 2)) + 1).ToString("00");
                        //}
                        //else
                        //{
                        //    tempID = "PT" + curdate + userid + "01";
                        //}
                        IList<SamplePretreatmentBatch> SamplePrepatch = ((XPObjectSpace)View.ObjectSpace).GetObjects<SamplePretreatmentBatch>(CriteriaOperator.Parse("SUBSTRING([PreTreatBatchID], 2, 6)=?", curdate));
                        if (SamplePrepatch.Count > 0)
                        {
                            SamplePrepatch = SamplePrepatch.OrderBy(a => a.PreTreatBatchID).ToList();
                            tempID = "PT" + curdate + (Convert.ToInt32(SamplePrepatch[SamplePrepatch.Count - 1].PreTreatBatchID.Substring(8, 2)) + 1).ToString("00") + userid;

                        }
                        else
                        {
                            tempID = "PT" + curdate + "01" + userid;
                        }

                        batch.PreTreatBatchID = tempID;
                        View.ObjectSpace.CommitChanges();

                        ((ASPxGridListEditor)samplePretreatmentList.ListView.Editor).Grid.UpdateEdit();
                        batch = samplePretreatmentList.ListView.ObjectSpace.GetObject<SamplePretreatmentBatch>(batch);
                        if (batch != null)
                        {
                            foreach (SamplePretreatmentBatchSequence sequence in samplePretreatmentList.ListView.CollectionSource.List.Cast<SamplePretreatmentBatchSequence>().OrderBy(a => a.Sort).ToList())
                            {
                                sequence.SamplePretreatmentBatchDetail = batch;
                            }
                        }
                        samplePretreatmentList.ListView.ObjectSpace.CommitChanges();

                        bool CanCommit = false;
                        IObjectSpace os = Application.CreateObjectSpace();
                        foreach (SamplePretreatmentBatchSequence sequence in samplePretreatmentList.ListView.CollectionSource.List.Cast<SamplePretreatmentBatchSequence>().OrderBy(a => a.Sort).ToList())
                        {
                            Modules.BusinessObjects.SampleManagement.SampleLogIn objSample = os.GetObject<Modules.BusinessObjects.SampleManagement.SampleLogIn>(sequence.SampleID);
                            if (objSample != null)
                            {
                                objSample.SamplePretreatmentBatchID = os.GetObject<SamplePretreatmentBatchSequence>(sequence);
                                CanCommit = true;
                            }
                        }
                        if (CanCommit)
                        {
                            os.CommitChanges();
                            os.Dispose();
                        }
                        Frame.SetView(Application.CreateListView(typeof(SamplePretreatmentBatch), true));

                        //string msg;
                        //if (CurrentLanguage == "En")
                        //{
                        //    msg = "A Weighing Batch ID " + tempID + " has been created. Do you want to save it?";
                        //}
                        //else
                        //{
                        //    msg = "填写其余信息，然后单击“确定”。弹出消息框“已创建称重批次编号" + tempID + "。您是否要保存？";
                        //}
                        //WebWindow.CurrentRequestWindow.RegisterClientScript("CloseWeighingBatch", string.Format(CultureInfo.InvariantCulture, @"var openconfirm = confirm('" + msg + "'); {0}", callbackManager.CallbackManager.GetScript("CanCloseView", "openconfirm")));
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SamplePreTreatmentLoad_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SamplePretreatmentBatch_DetailView")
                {
                    ListPropertyEditor samplePretreatmentList = ((DetailView)View).FindItem("SamplePretreatmentBatchSeqDetail") as ListPropertyEditor;
                    SamplePretreatmentBatch batch = (SamplePretreatmentBatch)View.CurrentObject;
                    if (batch != null && samplePretreatmentList != null && samplePretreatmentList.ListView != null)
                    {
                        if (batch.Matrix != null && !string.IsNullOrEmpty(batch.Jobid) && !string.IsNullOrEmpty(batch.Equipment) && batch.PrepMethod != null && batch.TemplateID != null)
                        {
                            if (samplePretreatmentList.ListView.CollectionSource.GetCount() > 0 && (SamplePreTreatmentSort.Caption == "Sort" || SamplePreTreatmentSort.Caption == "序号"))
                            {
                                foreach (SamplePretreatmentBatchSequence sequence in samplePretreatmentList.ListView.CollectionSource.List.Cast<SamplePretreatmentBatchSequence>().ToList())
                                {
                                    samplePretreatmentList.ListView.ObjectSpace.RemoveFromModifiedObjects(sequence);
                                    samplePretreatmentList.ListView.CollectionSource.Remove(sequence);
                                }
                                samplePretreatmentList.ListView.Refresh();
                            }
                            else if (samplePretreatmentList.ListView.CollectionSource.GetCount() > 0 && (SamplePreTreatmentSort.Caption == "Ok" || SamplePreTreatmentSort.Caption == "确定"))
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "samplesorterror"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }

                            QCType sampleqCType = samplePretreatmentList.ListView.ObjectSpace.FindObject<QCType>(CriteriaOperator.Parse("[QCTypeName] = 'Sample'"));
                            string[] ids = batch.Jobid.Split(';');
                            foreach (string id in ids)
                            {
                                Samplecheckin objsamplecheckin = samplePretreatmentList.ListView.ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]=?", new Guid(id.Replace(" ", ""))));
                                if (objsamplecheckin != null)
                                {
                                    IList<Modules.BusinessObjects.SampleManagement.SampleLogIn> objsp = samplePretreatmentList.ListView.ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[JobID.Oid]=? And [SamplePretreatmentBatchID] Is Null", objsamplecheckin.Oid));

                                    foreach (Modules.BusinessObjects.SampleManagement.SampleLogIn sampleLog in objsp.OrderBy(a => int.Parse(a.SampleID.Split('-')[1])).ToList())
                                    {
                                        SamplePretreatmentBatchSequence sequence = samplePretreatmentList.ListView.ObjectSpace.CreateObject<SamplePretreatmentBatchSequence>();
                                        sequence.QCType = sampleqCType;
                                        sequence.SampleID = sampleLog;
                                        sequence.SYSSamplecode = sampleLog.SampleID;
                                        samplePretreatmentList.ListView.CollectionSource.Add(sequence);
                                    }
                                }
                            }

                            if (samplePretreatmentList.ListView.Editor != null)
                            {
                                ASPxGridListEditor editor = (ASPxGridListEditor)samplePretreatmentList.ListView.Editor;
                                if (editor != null)
                                {
                                    editor.Grid.JSProperties["cpVisibleRowCount"] = samplePretreatmentList.ListView.CollectionSource.List.Count;
                                }
                            }
                            samplePretreatmentList.ListView.Refresh();
                            //batchInfo.IsSorted = false;
                            HideColumnsAsPerTemplate(samplePretreatmentList.ListView, batch);
                        }
                        else
                        {
                            if (batch.Matrix == null)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "matrixnotempty"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            }
                            else if (string.IsNullOrEmpty(batch.Jobid))
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "jobidnotempty"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            }
                            else if (batch.PrepMethod == null)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "prepmethodnotempty"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            }
                            else if (string.IsNullOrEmpty(batch.Equipment))
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "equipmentnotempty"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            }
                            else if (batch.TemplateID == null)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "templatenotempty"), InformationType.Info, timer.Seconds, InformationPosition.Top);
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

        private void HideColumnsAsPerTemplate(ListView samplePretreatmentList, SamplePretreatmentBatch batch)
        {
            try
            {
                if (samplePretreatmentList != null && samplePretreatmentList.Editor != null)
                {
                    ASPxGridListEditor gridListEditor = samplePretreatmentList.Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        DevExpress.Xpo.XPCollection<SamplePrepTemplateFields> templateFields = batch.TemplateID.SelectedFields;
                        ASPxGridView gridView = gridListEditor.Grid;
                        if (gridView != null)
                        {
                            foreach (WebColumnBase column in gridView.Columns)
                            {
                                IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                                if (columnInfo != null)
                                {
                                    if (columnInfo.Model.Id == "NPSampleID" || columnInfo.Model.Id == "Sort" || templateFields.FirstOrDefault(i => i.FieldName == columnInfo.Model.Id) != null)
                                    {
                                        column.Visible = true;
                                    }
                                    else
                                    {
                                        column.Visible = false;
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

        private void SamplePreTreatmentReset_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.ObjectSpace.IsNewObject(View.CurrentObject))
                {
                    NewObjectViewController newcontroller = Frame.GetController<NewObjectViewController>();
                    newcontroller.NewObjectAction.DoExecute(newcontroller.NewObjectAction.Items[0]);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void ProcessAction(string parameter)
        {
            try
            {
                if (!string.IsNullOrEmpty(parameter))
                {
                    if (bool.TryParse(parameter, out bool CanCloseView))
                    {
                        if (CanCloseView)
                        {
                            ListPropertyEditor samplePretreatmentList = ((DetailView)View).FindItem("SamplePretreatmentBatchSeqDetail") as ListPropertyEditor;
                            if (View.CurrentObject != null && samplePretreatmentList != null && samplePretreatmentList.ListView != null)
                            {
                                View.ObjectSpace.CommitChanges();
                                SamplePretreatmentBatch batch = samplePretreatmentList.ListView.ObjectSpace.GetObject<SamplePretreatmentBatch>((SamplePretreatmentBatch)View.CurrentObject);
                                if (batch != null)
                                {
                                    foreach (SamplePretreatmentBatchSequence sequence in samplePretreatmentList.ListView.CollectionSource.List.Cast<SamplePretreatmentBatchSequence>().OrderBy(a => a.Sort).ToList())
                                    {
                                        sequence.SamplePretreatmentBatchDetail = batch;
                                    }
                                }
                                samplePretreatmentList.ListView.ObjectSpace.CommitChanges();

                                bool CanCommit = false;
                                IObjectSpace os = Application.CreateObjectSpace();
                                foreach (SamplePretreatmentBatchSequence sequence in samplePretreatmentList.ListView.CollectionSource.List.Cast<SamplePretreatmentBatchSequence>().OrderBy(a => a.Sort).ToList())
                                {
                                    Modules.BusinessObjects.SampleManagement.SampleLogIn objSample = os.GetObject<Modules.BusinessObjects.SampleManagement.SampleLogIn>(sequence.SampleID);
                                    if (objSample != null)
                                    {
                                        objSample.SamplePretreatmentBatchID = os.GetObject<SamplePretreatmentBatchSequence>(sequence);
                                        CanCommit = true;
                                    }
                                }
                                if (CanCommit)
                                {
                                    os.CommitChanges();
                                    os.Dispose();
                                }
                            }
                            Frame.SetView(Application.CreateListView(typeof(SamplePretreatmentBatch), true));
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    {
                        if (View.Id == "SamplePretreatmentBatch_SamplePretreatmentBatchSeqDetail_ListView")
                        {
                            if (SamplePreTreatmentSort.Caption == "Sort" || SamplePreTreatmentSort.Caption == "序号")
                            {
                                if (parameter == "Selectall")
                                {
                                    int maxsort = 1;
                                    foreach (SamplePretreatmentBatchSequence sequences in ((ListView)View).CollectionSource.List.Cast<SamplePretreatmentBatchSequence>().ToList())
                                    {
                                        sequences.Sort = maxsort;
                                        maxsort++;
                                    }
                                }
                                else if (parameter == "UNSelectall")
                                {
                                    foreach (SamplePretreatmentBatchSequence sequences in ((ListView)View).CollectionSource.List.Cast<SamplePretreatmentBatchSequence>().ToList())
                                    {
                                        sequences.Sort = 0;
                                    }
                                }
                                else
                                {
                                    string[] splparm = parameter.Split('|');
                                    if (splparm[0] == "Selected")
                                    {
                                        int maxsort = 0;
                                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                                        if (gridListEditor != null && gridListEditor.Grid != null)
                                        {
                                            for (int i = 0; i <= gridListEditor.Grid.VisibleRowCount; i++)
                                            {
                                                int cursort = Convert.ToInt32(gridListEditor.Grid.GetRowValues(i, "Sort"));
                                                if (maxsort <= cursort)
                                                {
                                                    maxsort = cursort + 1;
                                                }
                                            }
                                        }
                                        SamplePretreatmentBatchSequence qCseq = View.ObjectSpace.FindObject<SamplePretreatmentBatchSequence>(CriteriaOperator.Parse("[Oid]=?", new Guid(splparm[1])), true);
                                        if (qCseq != null && qCseq.Sort == 0)
                                        {
                                            qCseq.Sort = maxsort;
                                        }
                                    }
                                    else if (splparm[0] == "UNSelected")
                                    {
                                        SamplePretreatmentBatchSequence qCseq = View.ObjectSpace.FindObject<SamplePretreatmentBatchSequence>(CriteriaOperator.Parse("[Oid]=?", new Guid(splparm[1])), true);
                                        if (qCseq != null)
                                        {
                                            foreach (SamplePretreatmentBatchSequence sequences in ((ListView)View).CollectionSource.List.Cast<SamplePretreatmentBatchSequence>().Where(i => i.Sort > qCseq.Sort).OrderBy(a => a.SampleID.SampleID).ToList())
                                            {
                                                sequences.Sort -= 1;
                                            }
                                            qCseq.Sort = 0;
                                        }
                                    }
                                }
                                ((ListView)View).Refresh();
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

        private void disablecontrols(bool enbstat, DevExpress.ExpressApp.View view, bool IsNewObj)
        {
            try
            {
                foreach (ViewItem item in ((DetailView)view).Items)
                {
                    if (item.GetType() == typeof(ASPxStringPropertyEditor))
                    {
                        ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(ASPxRichTextPropertyEditor))
                    {
                        ASPxRichTextPropertyEditor propertyEditor = item as ASPxRichTextPropertyEditor;
                        if (propertyEditor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
                    {
                        ASPxCheckedLookupStringPropertyEditor propertyEditor = item as ASPxCheckedLookupStringPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                    {
                        ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                    {
                        ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(FileDataPropertyEditor))
                    {
                        FileDataPropertyEditor propertyEditor = item as FileDataPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(ASPxEnumPropertyEditor))
                    {
                        ASPxEnumPropertyEditor propertyEditor = item as ASPxEnumPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                    {
                        ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                    {
                        ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(ListPropertyEditor) && !IsNewObj)
                    {
                        ListPropertyEditor propertyEditor = item as ListPropertyEditor;
                        if (propertyEditor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
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
