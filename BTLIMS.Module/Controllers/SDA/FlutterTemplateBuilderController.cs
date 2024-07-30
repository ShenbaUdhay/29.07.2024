using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SDA;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Controllers.SDA
{
    public partial class FlutterTemplateBuilderController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public FlutterTemplateBuilderController()
        {
            InitializeComponent();
            TargetViewId = "SDATemplate_ListView;" + "SDATemplate_DetailView;" + "SDATemplate_SDATemplateFields_ListView_Sampling;" + "SDATemplate_SDATemplateFields_ListView_Station;" +
                "SDATemplate_SDATemplateFields_ListView_Test;" + "tbl_FlutterSDA_FieldEntryColumn_ListView_Sampling;" + "tbl_FlutterSDA_FieldEntryColumn_ListView_Station;" + "tbl_FlutterSDA_FieldEntryColumn_ListView_Test;";
            HeaderAddAction.TargetViewId = "SDATemplate_DetailView;";
            HeaderRemoveAction.TargetViewId = "SDATemplate_DetailView;";
            DetailAddAction.TargetViewId = "SDATemplate_DetailView;";
            DetailRemoveAction.TargetViewId = "SDATemplate_DetailView;";
            CalibrationAddAction.TargetViewId = "SDATemplate_DetailView;";
            CalibrationRemoveAction.TargetViewId = "SDATemplate_DetailView;";
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                if (View.Id == "tbl_FlutterSDA_FieldEntryColumn_ListView_Sampling" || View.Id == "tbl_FlutterSDA_FieldEntryColumn_ListView_Station" || View.Id == "tbl_FlutterSDA_FieldEntryColumn_ListView_Test")
                {
                    SDATemplate objTemplate = (SDATemplate)Application.MainWindow.View.CurrentObject;
                    if (objTemplate != null)
                    {
                        List<string> lstFieldID = null;
                        if (View.Id == "tbl_FlutterSDA_FieldEntryColumn_ListView_Station")
                        {
                            lstFieldID = objTemplate.SDATemplateFields.Where(i => i.FieldType == "Station").Select(i => i.FieldName).Distinct().ToList();
                        }
                        else if (View.Id == "tbl_FlutterSDA_FieldEntryColumn_ListView_Sampling")
                        {
                            lstFieldID = objTemplate.SDATemplateFields.Where(i => i.FieldType == "Sampling").Select(i => i.FieldName).Distinct().ToList();
                        }
                        else if (View.Id == "tbl_FlutterSDA_FieldEntryColumn_ListView_Test")
                        {
                            lstFieldID = objTemplate.SDATemplateFields.Where(i => i.FieldType == "Test").Select(i => i.FieldName).Distinct().ToList();
                        }

                        if (lstFieldID != null && lstFieldID.Count > 0)
                        {
                            ((ListView)View).CollectionSource.Criteria["Filter"] = new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[Element] = ?", objTemplate.Element),
                                                                                    new NotOperator(new InOperator("FieldName", lstFieldID)));
                        }
                        else
                        {
                            ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Element] = ?", objTemplate.Element);
                        }
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("1=2");
                    }
                }
                else if (View.Id == "SDATemplate_DetailView")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    ObjectSpace.Committed += ObjectSpace_Committed;
                }

                if (View.Id == "SDATemplate_ListView" || View.Id == "SDATemplate_DetailView")
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

        private void DeleteAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                foreach (SDATemplate sDA in View.SelectedObjects)
                {
                    IList<SDATemplateDetail> details = View.ObjectSpace.GetObjects<SDATemplateDetail>(CriteriaOperator.Parse("TemplateID=?", sDA.uqSDATemplateID));
                    if (details.Count > 0)
                    {
                        View.ObjectSpace.Delete(details);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ObjectSpace_Committed(object sender, EventArgs e)
        {
            try
            {

                if (View.Id == "SDATemplate_DetailView")
                {
                    SDATemplate objTemplate = (SDATemplate)View.CurrentObject;
                    if (objTemplate != null)
                    {
                        bool CanEdit = false;
                        if (View.ObjectSpace.IsNewObject(objTemplate))
                        {
                            CanEdit = true;
                        }

                        DevExpress.ExpressApp.Web.Editors.ASPx.ASPxEnumPropertyEditor objCategoryEditor = (DevExpress.ExpressApp.Web.Editors.ASPx.ASPxEnumPropertyEditor)((DetailView)View).FindItem("Category");
                        DevExpress.ExpressApp.Web.Editors.ASPx.ASPxEnumPropertyEditor objSDMSTemplateIDEditor = (DevExpress.ExpressApp.Web.Editors.ASPx.ASPxEnumPropertyEditor)((DetailView)View).FindItem("Element");

                        if (objCategoryEditor != null)
                        {
                            objCategoryEditor.AllowEdit.SetItemValue("CanEdit", CanEdit);
                        }
                        if (objSDMSTemplateIDEditor != null)
                        {
                            objSDMSTemplateIDEditor.AllowEdit.SetItemValue("CanEdit", CanEdit);
                        }
                    }

                    ListPropertyEditor liHeader = (ListPropertyEditor)((DetailView)View).FindItem("SDATemplateFields_Station");
                    ListPropertyEditor liDetail = (ListPropertyEditor)((DetailView)View).FindItem("SDATemplateFields_Sampling");
                    ListPropertyEditor liCalibration = (ListPropertyEditor)((DetailView)View).FindItem("SDATemplateFields_Test");

                    DashboardViewItem dviFieldSetup_Header = (DashboardViewItem)((DetailView)View).FindItem("FieldEntryColumn_Station");
                    DashboardViewItem dviFieldSetup_Detail = (DashboardViewItem)((DetailView)View).FindItem("FieldEntryColumn_Sampling");
                    DashboardViewItem dviFieldSetup_Calibration = (DashboardViewItem)((DetailView)View).FindItem("FieldEntryColumn_Test");

                    if (liHeader != null && liHeader.ListView != null)
                    {
                        ASPxGridListEditor gridlistEditor = (ASPxGridListEditor)liHeader.ListView.Editor;
                        if (gridlistEditor != null && gridlistEditor.Grid != null)
                        {
                            gridlistEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridlistEditor.Grid.Settings.VerticalScrollableHeight = 300;
                            gridlistEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                    }
                    if (liDetail != null && liDetail.ListView != null)
                    {
                        ASPxGridListEditor gridlistEditor = (ASPxGridListEditor)liDetail.ListView.Editor;
                        if (gridlistEditor != null && gridlistEditor.Grid != null)
                        {
                            gridlistEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridlistEditor.Grid.Settings.VerticalScrollableHeight = 300;
                            gridlistEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                    }
                    if (liCalibration != null && liCalibration.ListView != null)
                    {
                        ASPxGridListEditor gridlistEditor = (ASPxGridListEditor)liCalibration.ListView.Editor;
                        if (gridlistEditor != null && gridlistEditor.Grid != null)
                        {
                            gridlistEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridlistEditor.Grid.Settings.VerticalScrollableHeight = 300;
                            gridlistEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                    }

                    if (dviFieldSetup_Header != null && dviFieldSetup_Header.InnerView != null)
                    {
                        ASPxGridListEditor gridlistEditor = (ASPxGridListEditor)((ListView)dviFieldSetup_Header.InnerView).Editor;
                        if (gridlistEditor != null && gridlistEditor.Grid != null)
                        {
                            gridlistEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridlistEditor.Grid.Settings.VerticalScrollableHeight = 300;
                            gridlistEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                    }
                    if (dviFieldSetup_Detail != null && dviFieldSetup_Detail.InnerView != null)
                    {
                        ASPxGridListEditor gridlistEditor = (ASPxGridListEditor)((ListView)dviFieldSetup_Detail.InnerView).Editor;
                        if (gridlistEditor != null && gridlistEditor.Grid != null)
                        {
                            gridlistEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridlistEditor.Grid.Settings.VerticalScrollableHeight = 300;
                            gridlistEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                    }
                    if (dviFieldSetup_Calibration != null && dviFieldSetup_Calibration.InnerView != null)
                    {
                        ASPxGridListEditor gridlistEditor = (ASPxGridListEditor)((ListView)dviFieldSetup_Calibration.InnerView).Editor;
                        if (gridlistEditor != null && gridlistEditor.Grid != null)
                        {
                            gridlistEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridlistEditor.Grid.Settings.VerticalScrollableHeight = 300;
                            gridlistEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
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

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View.Id == "SDATemplate_DetailView" && e.PropertyName == "Element" && e.OldValue != e.NewValue)
                {
                    SDATemplate objTemplate = (SDATemplate)e.Object;

                    ListPropertyEditor liHeader = (ListPropertyEditor)((DetailView)View).FindItem("SDATemplateFields_Station");
                    ListPropertyEditor liDetail = (ListPropertyEditor)((DetailView)View).FindItem("SDATemplateFields_Sampling");
                    ListPropertyEditor liCalibration = (ListPropertyEditor)((DetailView)View).FindItem("SDATemplateFields_Test");

                    DashboardViewItem dviFieldSetup_Header = (DashboardViewItem)((DetailView)View).FindItem("FieldEntryColumn_Station");
                    DashboardViewItem dviFieldSetup_Detail = (DashboardViewItem)((DetailView)View).FindItem("FieldEntryColumn_Sampling");
                    DashboardViewItem dviFieldSetup_Calibration = (DashboardViewItem)((DetailView)View).FindItem("FieldEntryColumn_Test");

                    if (liHeader != null && liHeader.ListView == null)
                    {
                        liHeader.CreateControl();
                    }
                    if (liDetail != null && liDetail.ListView == null)
                    {
                        liDetail.CreateControl();
                    }
                    if (liCalibration != null && liCalibration.ListView == null)
                    {
                        liCalibration.CreateControl();
                    }

                    if (dviFieldSetup_Header != null && dviFieldSetup_Header.InnerView == null)
                    {
                        dviFieldSetup_Header.CreateControl();
                    }
                    if (dviFieldSetup_Detail != null && dviFieldSetup_Detail.InnerView == null)
                    {
                        dviFieldSetup_Detail.CreateControl();
                    }
                    if (dviFieldSetup_Calibration != null && dviFieldSetup_Calibration.InnerView == null)
                    {
                        dviFieldSetup_Calibration.CreateControl();
                    }

                    if (objTemplate != null)
                    {
                        foreach (SDATemplateDetail objDetail in objTemplate.SDATemplateFields.ToList())
                        {
                            objTemplate.SDATemplateFields.Remove(objDetail);
                            ObjectSpace.Delete(objDetail);
                        }
                        ((ListView)dviFieldSetup_Header.InnerView).CollectionSource.Criteria.Clear();
                        ((ListView)dviFieldSetup_Detail.InnerView).CollectionSource.Criteria.Clear();
                        ((ListView)dviFieldSetup_Calibration.InnerView).CollectionSource.Criteria.Clear();
                        ((ListView)dviFieldSetup_Header.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[FieldType] = 'Station' and [Element] = ?", objTemplate.Element);
                        ((ListView)dviFieldSetup_Detail.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[FieldType] = 'Sampling' and [Element] = ?", objTemplate.Element);
                        ((ListView)dviFieldSetup_Calibration.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[FieldType] = 'Test' and [Element] = ?", objTemplate.Element);
                    }
                    else
                    {
                        ((ListView)dviFieldSetup_Header.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("1=2");
                        ((ListView)dviFieldSetup_Detail.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("1=2");
                        ((ListView)dviFieldSetup_Calibration.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("1=2");
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
            try
            {
                base.OnViewControlsCreated();
                if (View.Id == "SDATemplate_DetailView")
                {
                    SDATemplate objTemplate = (SDATemplate)View.CurrentObject;
                    if (objTemplate != null)
                    {
                        bool CanEdit = false;
                        if (View.ObjectSpace.IsNewObject(objTemplate))
                        {
                            CanEdit = true;
                        }

                        DevExpress.ExpressApp.Web.Editors.ASPx.ASPxEnumPropertyEditor objCategoryEditor = (DevExpress.ExpressApp.Web.Editors.ASPx.ASPxEnumPropertyEditor)((DetailView)View).FindItem("Category");
                        DevExpress.ExpressApp.Web.Editors.ASPx.ASPxEnumPropertyEditor objSDMSTemplateIDEditor = (DevExpress.ExpressApp.Web.Editors.ASPx.ASPxEnumPropertyEditor)((DetailView)View).FindItem("Element");

                        if (objCategoryEditor != null)
                        {
                            objCategoryEditor.AllowEdit.SetItemValue("CanEdit", CanEdit);
                        }
                        if (objSDMSTemplateIDEditor != null)
                        {
                            objSDMSTemplateIDEditor.AllowEdit.SetItemValue("CanEdit", CanEdit);
                        }
                    }

                    ListPropertyEditor liHeader = (ListPropertyEditor)((DetailView)View).FindItem("SDATemplateFields_Station");
                    ListPropertyEditor liDetail = (ListPropertyEditor)((DetailView)View).FindItem("SDATemplateFields_Sampling");
                    ListPropertyEditor liCalibration = (ListPropertyEditor)((DetailView)View).FindItem("SDATemplateFields_Test");

                    DashboardViewItem dviFieldSetup_Header = (DashboardViewItem)((DetailView)View).FindItem("FieldEntryColumn_Station");
                    DashboardViewItem dviFieldSetup_Detail = (DashboardViewItem)((DetailView)View).FindItem("FieldEntryColumn_Sampling");
                    DashboardViewItem dviFieldSetup_Calibration = (DashboardViewItem)((DetailView)View).FindItem("FieldEntryColumn_Test");

                    if (liHeader != null && liHeader.ListView != null)
                    {
                        ASPxGridListEditor gridlistEditor = (ASPxGridListEditor)liHeader.ListView.Editor;
                        if (gridlistEditor != null && gridlistEditor.Grid != null)
                        {
                            gridlistEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridlistEditor.Grid.Settings.VerticalScrollableHeight = 300;
                            gridlistEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                    }
                    if (liDetail != null && liDetail.ListView != null)
                    {
                        ASPxGridListEditor gridlistEditor = (ASPxGridListEditor)liDetail.ListView.Editor;
                        if (gridlistEditor != null && gridlistEditor.Grid != null)
                        {
                            gridlistEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridlistEditor.Grid.Settings.VerticalScrollableHeight = 300;
                            gridlistEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                    }
                    if (liCalibration != null && liCalibration.ListView != null)
                    {
                        ASPxGridListEditor gridlistEditor = (ASPxGridListEditor)liCalibration.ListView.Editor;
                        if (gridlistEditor != null && gridlistEditor.Grid != null)
                        {
                            gridlistEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridlistEditor.Grid.Settings.VerticalScrollableHeight = 300;
                            gridlistEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                    }

                    if (dviFieldSetup_Header != null && dviFieldSetup_Header.InnerView != null)
                    {
                        ASPxGridListEditor gridlistEditor = (ASPxGridListEditor)((ListView)dviFieldSetup_Header.InnerView).Editor;
                        if (gridlistEditor != null && gridlistEditor.Grid != null)
                        {
                            gridlistEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridlistEditor.Grid.Settings.VerticalScrollableHeight = 300;
                            gridlistEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                    }
                    if (dviFieldSetup_Detail != null && dviFieldSetup_Detail.InnerView != null)
                    {
                        ASPxGridListEditor gridlistEditor = (ASPxGridListEditor)((ListView)dviFieldSetup_Detail.InnerView).Editor;
                        if (gridlistEditor != null && gridlistEditor.Grid != null)
                        {
                            gridlistEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridlistEditor.Grid.Settings.VerticalScrollableHeight = 300;
                            gridlistEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                    }
                    if (dviFieldSetup_Calibration != null && dviFieldSetup_Calibration.InnerView != null)
                    {
                        ASPxGridListEditor gridlistEditor = (ASPxGridListEditor)((ListView)dviFieldSetup_Calibration.InnerView).Editor;
                        if (gridlistEditor != null && gridlistEditor.Grid != null)
                        {
                            gridlistEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridlistEditor.Grid.Settings.VerticalScrollableHeight = 300;
                            gridlistEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                    }
                }
                else if (View.Id == "tbl_FlutterSDA_FieldEntryColumn_ListView_Sampling" || View.Id == "tbl_FlutterSDA_FieldEntryColumn_ListView_Station" || View.Id == "tbl_FlutterSDA_FieldEntryColumn_ListView_Test" ||
                View.Id == "SDATemplate_SDATemplateFields_ListView_Station" || View.Id == "SDATemplate_SDATemplateFields_ListView_Sampling" || View.Id == "SDATemplate_SDATemplateFields_ListView_Test")
                {
                    ASPxGridListEditor gridListEditor = (ASPxGridListEditor)((ListView)View).Editor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                        gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                        gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        if (View.Id == "SDATemplate_SDATemplateFields_ListView_Sampling" || View.Id == "SDATemplate_SDATemplateFields_ListView_Station" || View.Id == "SDATemplate_SDATemplateFields_ListView_Test")
                        {
                            gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                            gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                    { 
                        var nav = document.getElementById('LPcell');
                        var sep = document.getElementById('separatorCell');
                        if(nav != null && sep != null) {
                           var totusablescr = screen.width - (sep.offsetWidth + nav.offsetWidth);
                           s.SetWidth((totusablescr / 100) * 50);         
                        }
                        else {
                            s.SetWidth(650); 
                        }                  
                    }";
                        }
                        else
                        {
                            gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                    {
                        var nav = document.getElementById('LPcell');
                        var sep = document.getElementById('separatorCell');
                        if(nav != null && sep != null) {
                           var totusablescr = screen.width - (sep.offsetWidth + nav.offsetWidth);
                           s.SetWidth((totusablescr / 100) * 36); 
                        }
                        else {
                            s.SetWidth(230); 
                        }                        
                    }";
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
                if (View.Id == "SDATemplate_DetailView")
                {
                    ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                    ObjectSpace.Committed -= ObjectSpace_Committed;
                }
                if (View.Id == "SDATemplate_ListView" || View.Id == "SDATemplate_DetailView")
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

        private void HeaderAddAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ListPropertyEditor liHeader = (ListPropertyEditor)((DetailView)View).FindItem("SDATemplateFields_Station");
                DashboardViewItem dviFieldSetup_Header = (DashboardViewItem)((DetailView)View).FindItem("FieldEntryColumn_Station");
                ListView lvHeaderFieldSetup = (ListView)dviFieldSetup_Header.InnerView;
                SDATemplate objTemplate = (SDATemplate)Application.MainWindow.View.CurrentObject;
                IObjectSpace os = liHeader.ListView.ObjectSpace;

                if (lvHeaderFieldSetup != null && lvHeaderFieldSetup.SelectedObjects.Count > 0)
                {
                    foreach (tbl_FlutterSDA_FieldEntryColumn objFields in lvHeaderFieldSetup.SelectedObjects.Cast<tbl_FlutterSDA_FieldEntryColumn>().Where(i => i.FieldName != null).ToList())
                    {
                        SDATemplateDetail objHeader = os.CreateObject<SDATemplateDetail>();
                        objHeader.TemplateID = os.GetObject<SDATemplate>(objTemplate);
                        objHeader.FieldName = objFields.FieldName;
                        objHeader.FieldDataType = objFields.DataType;
                        objHeader.FieldType = objFields.FieldType;
                        objHeader.Caption = objFields.Caption_EN;
                        objHeader.uqFieldEntryColumnID = objFields.uqFieldEntryColumnID;

                        liHeader.ListView.CollectionSource.Add(objHeader);
                        lvHeaderFieldSetup.CollectionSource.Remove(objFields);
                    }
                    liHeader.ListView.Refresh();
                    liHeader.Refresh();
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

        private void HeaderRemoveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ListPropertyEditor liHeader = (ListPropertyEditor)((DetailView)View).FindItem("SDATemplateFields_Station");
                DashboardViewItem dviFieldSetup_Header = (DashboardViewItem)((DetailView)View).FindItem("FieldEntryColumn_Station");
                ListView lvHeaderFieldSetup = (ListView)dviFieldSetup_Header.InnerView;
                IObjectSpace os = liHeader.ListView.ObjectSpace;

                if (liHeader != null && liHeader.ListView.SelectedObjects.Count > 0)
                {
                    foreach (SDATemplateDetail objHeader in liHeader.ListView.SelectedObjects.Cast<SDATemplateDetail>().ToList())
                    {
                        liHeader.ListView.CollectionSource.Remove(objHeader);
                        lvHeaderFieldSetup.CollectionSource.Add(lvHeaderFieldSetup.ObjectSpace.FindObject<tbl_FlutterSDA_FieldEntryColumn>(CriteriaOperator.Parse("[uqFieldEntryColumnID]=?", objHeader.uqFieldEntryColumnID)));
                        os.Delete(objHeader);
                    }
                    liHeader.ListView.Refresh();
                    liHeader.Refresh();
                    dviFieldSetup_Header.InnerView.Refresh();
                    dviFieldSetup_Header.Refresh();
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

        private void DetailAddAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ListPropertyEditor liDetail = (ListPropertyEditor)((DetailView)View).FindItem("SDATemplateFields_Sampling");
                DashboardViewItem dviFieldSetup_Detail = (DashboardViewItem)((DetailView)View).FindItem("FieldEntryColumn_Sampling");
                ListView lvDetailFieldSetup = (ListView)dviFieldSetup_Detail.InnerView;
                SDATemplate objTemplate = (SDATemplate)Application.MainWindow.View.CurrentObject;
                IObjectSpace os = liDetail.ListView.ObjectSpace;

                if (lvDetailFieldSetup != null && lvDetailFieldSetup.SelectedObjects.Count > 0)
                {
                    foreach (tbl_FlutterSDA_FieldEntryColumn objFields in lvDetailFieldSetup.SelectedObjects.Cast<tbl_FlutterSDA_FieldEntryColumn>().Where(i => i.FieldName != null).ToList())
                    {
                        SDATemplateDetail objDetail = os.CreateObject<SDATemplateDetail>();
                        objDetail.TemplateID = os.GetObject<SDATemplate>(objTemplate);
                        objDetail.FieldName = objFields.FieldName;
                        objDetail.FieldDataType = objFields.DataType;
                        objDetail.FieldType = objFields.FieldType;
                        objDetail.Caption = objFields.Caption_EN;
                        objDetail.uqFieldEntryColumnID = objFields.uqFieldEntryColumnID;

                        liDetail.ListView.CollectionSource.Add(objDetail);
                        lvDetailFieldSetup.CollectionSource.Remove(objFields);
                    }
                    liDetail.ListView.Refresh();
                    liDetail.Refresh();
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

        private void DetailRemoveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ListPropertyEditor liDetail = (ListPropertyEditor)((DetailView)View).FindItem("SDATemplateFields_Sampling");
                DashboardViewItem dviFieldSetup_Detail = (DashboardViewItem)((DetailView)View).FindItem("FieldEntryColumn_Sampling");
                ListView lvDetailFieldSetup = (ListView)dviFieldSetup_Detail.InnerView;
                IObjectSpace os = liDetail.ListView.ObjectSpace;

                if (liDetail != null && liDetail.ListView.SelectedObjects.Count > 0)
                {
                    foreach (SDATemplateDetail objDetail in liDetail.ListView.SelectedObjects.Cast<SDATemplateDetail>().ToList())
                    {
                        liDetail.ListView.CollectionSource.Remove(objDetail);
                        lvDetailFieldSetup.CollectionSource.Add(lvDetailFieldSetup.ObjectSpace.FindObject<tbl_FlutterSDA_FieldEntryColumn>(CriteriaOperator.Parse("[uqFieldEntryColumnID]=?", objDetail.uqFieldEntryColumnID)));
                        os.Delete(objDetail);
                    }
                    liDetail.ListView.Refresh();
                    liDetail.Refresh();
                    lvDetailFieldSetup.Refresh();
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

        private void CalibrationAddAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ListPropertyEditor liCalibration = (ListPropertyEditor)((DetailView)View).FindItem("SDATemplateFields_Test");
                DashboardViewItem dviFieldSetup_Calibration = (DashboardViewItem)((DetailView)View).FindItem("FieldEntryColumn_Test");
                ListView lvCalibFieldSetup = (ListView)dviFieldSetup_Calibration.InnerView;
                SDATemplate objTemplate = (SDATemplate)Application.MainWindow.View.CurrentObject;
                IObjectSpace os = liCalibration.ListView.ObjectSpace;

                if (lvCalibFieldSetup != null && lvCalibFieldSetup.SelectedObjects.Count > 0)
                {
                    foreach (tbl_FlutterSDA_FieldEntryColumn objFields in lvCalibFieldSetup.SelectedObjects.Cast<tbl_FlutterSDA_FieldEntryColumn>().Where(i => i.FieldName != null).ToList())
                    {
                        SDATemplateDetail objCalibration = os.CreateObject<SDATemplateDetail>();
                        objCalibration.TemplateID = os.GetObject<SDATemplate>(objTemplate);
                        objCalibration.FieldName = objFields.FieldName;
                        objCalibration.FieldDataType = objFields.DataType;
                        objCalibration.FieldType = objFields.FieldType;
                        objCalibration.Caption = objFields.Caption_EN;
                        objCalibration.uqFieldEntryColumnID = objFields.uqFieldEntryColumnID;

                        liCalibration.ListView.CollectionSource.Add(objCalibration);
                        lvCalibFieldSetup.CollectionSource.Remove(objFields);
                    }
                    liCalibration.ListView.Refresh();
                    liCalibration.Refresh();
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

        private void CalibrationRemoveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ListPropertyEditor liCalibration = (ListPropertyEditor)((DetailView)View).FindItem("SDATemplateFields_Test");
                DashboardViewItem dviFieldSetup_Calibration = (DashboardViewItem)((DetailView)View).FindItem("FieldEntryColumn_Test");
                ListView lvDetailFieldSetup = (ListView)dviFieldSetup_Calibration.InnerView;
                IObjectSpace os = liCalibration.ListView.ObjectSpace;

                if (liCalibration != null && liCalibration.ListView.SelectedObjects.Count > 0)
                {
                    foreach (SDATemplateDetail objCalibration in liCalibration.ListView.SelectedObjects.Cast<SDATemplateDetail>().ToList())
                    {
                        liCalibration.ListView.CollectionSource.Remove(objCalibration);
                        lvDetailFieldSetup.CollectionSource.Add(lvDetailFieldSetup.ObjectSpace.FindObject<tbl_FlutterSDA_FieldEntryColumn>(CriteriaOperator.Parse("[uqFieldEntryColumnID]=?", objCalibration.uqFieldEntryColumnID)));
                        os.Delete(objCalibration);
                    }
                    liCalibration.ListView.Refresh();
                    liCalibration.Refresh();
                    lvDetailFieldSetup.Refresh();
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
