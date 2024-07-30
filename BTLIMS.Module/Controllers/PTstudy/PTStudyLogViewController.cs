using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Export;
using DevExpress.Web;
using DevExpress.Xpo;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.PLM;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.PLM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using static Modules.BusinessObjects.PLM.PTStudyLog;
using static Modules.BusinessObjects.Setting.PLM.PTStudyLogResults;

namespace LDM.Module.Web.Controllers.PLM
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class PTStudyLogViewController : ViewController, IXafCallbackHandler
    {
        PTStudyLogInfo objPTstudy = new PTStudyLogInfo();
        MessageTimer timer = new MessageTimer();
        SimpleAction ResultRemove;
        bool PopupBool = false;
        ICallbackManagerHolder sheet;
        //string ReportedValue = string.Empty;
        //string DateAnalyzed = string.Empty;
        //string AnalyzedBy = string.Empty;
        string strZ_Score = string.Empty;
        string strPT_Value = string.Empty;
        string strAcceptable = string.Empty;
        string strComments = string.Empty;
        string jScript = @" Grid.UpdateEdit();";
        public PTStudyLogViewController()
        {
            InitializeComponent();
            TargetViewId = "PTStudyLog_ListView;" + "PTStudyLog_DetailView_Study;" + "Client_Request_General_Information_ListView_PTStudyLog;" + "PTStudyLog_ListView_Results;" + "PTStudyLog_Results_ListView;" + "PTStudyLog_DetailView;";
            //Add.TargetViewId = "PTStudyLog_Results_ListView;";
            //PTStudySave.TargetViewId = "PTStudyLog_ListView";
            PTStudyDateFilter.TargetViewId = "PTStudyLog_ListView;";
            PTStudyLogRelease.TargetViewId = "PTStudyLog_ListView;" + "PTStudyLog_DetailView;";
            ImportResultsFileAction.TargetViewId = "PTStudyLog_Results_ListView;";

            /*Removebtn*/
            //ResultRemove = new SimpleAction(this, "Results", PredefinedCategory.RecordEdit);
            //ResultRemove.Caption = "Unlink";
            //ResultRemove.ToolTip = "Remove";
            //ResultRemove.Category = "Edit";
            //ResultRemove.ImageName = "Remove.png";
            //ResultRemove.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
            //ResultRemove.TargetViewId = "PTStudyLog_Results_ListView;";
            //ResultRemove.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;


        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id == "PTStudyLog_Results_ListView")
                {
                    //ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    //ResultRemove.Execute += ResultRemove_Executing;
                    //    if (((ListView)View).CollectionSource.GetCount() > 0)
                    //    {
                    //        if (objPTstudy.lstexistTestparamOid == null)
                    //        {
                    //            objPTstudy.lstexistTestparamOid = new List<Guid>();
                    //        }
                    //        foreach (PTStudyLogResults obj in ((ListView)View).CollectionSource.List.Cast<PTStudyLogResults>().ToList())
                    //        {
                    //            Testparameter objParam = ObjectSpace.FindObject<Testparameter>((CriteriaOperator.Parse("[TestMethod.MatrixName.Oid] = ? And [TestMethod.Oid] = ? And [TestMethod.MethodName.Oid] = ? And [Parameter.Oid]= ? ", obj.Matrix.Oid, obj.Test.Oid, obj.Method.Oid,obj.Parameter.Oid)));
                    //            if (objParam != null)
                    //            {
                    //                objPTstudy.lstexistTestparamOid.Add(objParam.Oid);
                    //            }
                    //        }
                    //    }
                }

                else if (View.Id == "PTStudyLog_ListView")
                {
                    SimpleAction objDelete = Frame.GetController<DeleteObjectsViewController>().DeleteAction;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing;
                    if (objDelete != null)
                    {
                        objDelete.Caption = "Delete";
                        objDelete.ImageName = "Action_Delete";
                        objDelete.ConfirmationMessage = @"Do you want to delete the record?";
                    }

                    if (PTStudyDateFilter.Items.Count == 0)
                    {
                        var Item1 = new ChoiceActionItem();
                        var Item2 = new ChoiceActionItem();
                        var Item3 = new ChoiceActionItem();
                        var Item4 = new ChoiceActionItem();

                        PTStudyDateFilter.Items.Add(new ChoiceActionItem("1Y", Item1));
                        PTStudyDateFilter.Items.Add(new ChoiceActionItem("2Y", Item2));
                        PTStudyDateFilter.Items.Add(new ChoiceActionItem("5Y", Item3));
                        PTStudyDateFilter.Items.Add(new ChoiceActionItem("All", Item4));
                    }
                    PTStudyDateFilter.SelectedIndex = 0;
                    ((ListView)View).CollectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 1 And [CreatedDate] Is Not Null");


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
                if (View.Id == "PTStudyLog_ListView" || View.Id == "PTStudyLog_DetailView")
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        foreach (PTStudyLog objPT in View.SelectedObjects)
                        {
                            IList<PTStudyLogResults> lstobj = ObjectSpace.GetObjects<PTStudyLogResults>(CriteriaOperator.Parse("[PTStudyLog.Oid] =? ", objPT.Oid));
                            foreach (PTStudyLogResults obj in lstobj)
                            {
                                if (obj.SampleID.QCBatchID != null && obj.SampleID.ABID != null)
                                {
                                    Application.ShowViewStrategy.ShowMessage("Unable to delete the data since already this JobID has been used in 'SDMS'", InformationType.Error, 4000, InformationPosition.Top);
                                    e.Cancel = true;
                                    return;
                                }
                            }

                        }
                    }
                }
                if (View.Id == "PTStudyLog_ListView")
                {
                    e.Cancel = true;
                    if (View.SelectedObjects.Count > 0)
                    {
                        WebWindow.CurrentRequestWindow.RegisterClientScript("Openspreadsheet", string.Format(CultureInfo.InvariantCulture, @"var openconfirm = confirm('Are you sure you want to delete the record?'); {0}", sheet.CallbackManager.GetScript("clrbtnvalidation", "openconfirm")));
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

                        //else if (item is ASPxIntPropertyEditor)
                        //{
                        //    ASPxIntPropertyEditor editor = (ASPxIntPropertyEditor)item;
                        //    if (editor != null && editor.Editor != null)
                        //    {
                        //        editor.Editor.ForeColor = Color.Black;
                        //    }
                        //}

                        //else if (item is ASPxDoublePropertyEditor)
                        //{
                        //    ASPxDoublePropertyEditor editor = (ASPxDoublePropertyEditor)item;
                        //    if (editor != null && editor.Editor != null)
                        //    {
                        //        editor.Editor.ForeColor = Color.Black;
                        //    }
                        //}

                        //else if (item is ASPxDecimalPropertyEditor)
                        //{
                        //    ASPxDecimalPropertyEditor editor = (ASPxDecimalPropertyEditor)item;
                        //    if (editor != null && editor.Editor != null)
                        //    {
                        //        editor.Editor.ForeColor = Color.Black;
                        //    }

                        //}

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
                        else if (item is ASPxEnumPropertyEditor)
                        {
                            ASPxEnumPropertyEditor editor = (ASPxEnumPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                    }
                }
                sheet = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                sheet.CallbackManager.RegisterHandler("clrbtnvalidation", this);
                if (View.Id == "PTStudyLog_DetailView")
                {
                    PTStudyLog objPT = Application.MainWindow.View.CurrentObject as PTStudyLog;
                    foreach (ViewItem item in ((DetailView)View).Items)
                    {
                        if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
                        {
                            ASPxCheckedLookupStringPropertyEditor propertyEditor = item as ASPxCheckedLookupStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                ASPxGridLookup editor = (ASPxGridLookup)propertyEditor.Editor;
                                if (editor != null)
                                {
                                    editor.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                                    editor.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                                    editor.GridView.Settings.VerticalScrollableHeight = 240;
                                }
                            }
                        }
                    }
                }

                else if (View.Id == "Testparameter_ListView_PTStudyLog")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("");
                }
                else if (View.Id == "PTStudyLog_ListView")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Executing += SaveAction_Executing;
                }
                else if (View.Id == "PTStudyLog_Results_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    XafCallbackManager parameter = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    parameter.RegisterHandler("PTStudyLogTypePopup", this);
                    gridListEditor.Grid.ClientInstanceName = "PTStudyLogType";
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                    gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                    gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                    gridListEditor.Grid.Load += Grid_Load;
                    //gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) 
                    //    { 
                    //    if(e.focusedColumn.fieldName != 'Z_Score' || e.focusedColumn.fieldName != 'PT_Value' || e.focusedColumn.fieldName != 'Acceptable' || e.focusedColumn.fieldName != 'Commentbool') 
                    //    { 
                    //      e.cancel = true;  
                    //    }
                    //    else
                    //      {
                    //          var Z_Score = s.batchEditApi.GetCellValue(e.visibleIndex, 'Z_Score');
                    //          var PT_Value = s.batchEditApi.GetCellValue(e.visibleIndex, 'PT_Value');
                    //          var Acceptable = s.batchEditApi.GetCellValue(e.visibleIndex, 'Acceptable');
                    //          var Commentbool = s.batchEditApi.GetCellValue(e.visibleIndex, 'Commentbool');
                    //          sessionStorage.setItem('valZ_Score', Z_Score);
                    //          s.Z_Score=Z_Score;                              
                    //          sessionStorage.setItem('valPT_Value', PT_Value);
                    //          s.PT_Value=PT_Value;                              
                    //          sessionStorage.setItem('valAcceptable', Acceptable);
                    //          s.Acceptable=Acceptable;
                    //          sessionStorage.setItem('valCommentbool', Commentbool);
                    //          s.Commentbool=Commentbool;
                    //      }
                    //    }";
                    gridListEditor.Grid.JSProperties["cpVisibleRowCount"] = gridListEditor.Grid.VisibleRowCount;
                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            s.timerHandle = setTimeout(function() {  
                                 if (s.batchEditApi.HasChanges()) {  
                                   s.UpdateEdit();  
                                 } 
                                   var newZ_Score = s.batchEditApi.GetCellValue(e.visibleIndex, 'Z_Score');
                                   var oldZ_Score = sessionStorage.getItem('valQty');                                   
                                   var newPT_Value = s.batchEditApi.GetCellValue(e.visibleIndex, 'PT_Value');
                                   var oldPT_Value = sessionStorage.getItem('valPT_Value');                                   
                                   var newAcceptable = s.batchEditApi.GetCellValue(e.visibleIndex, 'Acceptable');
                                   var oldAcceptable = sessionStorage.getItem('valAcceptable');
                                   var newCommentbool = s.batchEditApi.GetCellValue(e.visibleIndex, 'Commentbool');                                   
                                   var oldCommentbool = sessionStorage.getItem('valCommentbool');
                                   if(newZ_Score!=null && newZ_Score!=null && newZ_Score != newZ_Score)
                                      {
                                           RaiseXafCallback(globalCallbackControl,'Bottle', 'Z_Score|'+e.visibleIndex+'|'+oldZ_Score+'|'+newZ_Score, '', false);
                                      }
                                      if(newPT_Value!=null && oldPT_Value!=null && newPT_Value != oldPT_Value)
                                      {
                                           RaiseXafCallback(globalCallbackControl,'Bottle', 'PT_Value|'+e.visibleIndex+'|'+oldPT_Value+'|'+newPT_Value, '', false);
                                      }
                                      if(newAcceptable!=null && oldAcceptable!=null && newAcceptable != oldAcceptable)
                                      {
                                           RaiseXafCallback(globalCallbackControl,'Bottle', 'Acceptable|'+e.visibleIndex+'|'+oldAcceptable+'|'+newAcceptable, '', false);
                                      }
                                      if(newCommentbool!=null && oldCommentbool!=null && newCommentbool != oldCommentbool)
                                      {
                                           RaiseXafCallback(globalCallbackControl,'Bottle', 'Commentbool!=null|'+e.visibleIndex+'|'+oldCommentbool+'|'+newCommentbool, '', false);
                                      }
                               }, 20);}";
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
                    gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s, e) { 
                    if (s.IsRowSelectedOnPage(e.elementIndex)) 
                    { 
                         var FocusedColumn = sessionStorage.getItem('CurrFocusedColumn');                                
                         var oid;
                         var text;
                         //console.log('FocusedColumn:', FocusedColumn);
                         if (FocusedColumn && FocusedColumn.includes('.')) 
                         {                                       
                              oid = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn, false);
                              text = s.batchEditApi.GetCellTextContainer(e.elementIndex, FocusedColumn).innerText;                                                     
                              //console.log('oid:', oid);
                              //console.log('text:', text);

                              if (e.item.name == 'CopyToAllCell')
                               { 
                                  if (FocusedColumn=='Z_Score' || FocusedColumn=='PT_Value' || FocusedColumn=='Acceptable' || FocusedColumn=='Commentbool')
	                              {                                 
                                    for (var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                    { 
                                       if (s.IsRowSelectedOnPage(i))
                                       {                                               
                                         s.batchEditApi.SetCellValue(i, FocusedColumn, oid, text, false);
                                       }                                            
                                    }
                                   if (e.item.name === 'CopyToAllCell' && FocusedColumn === 'Commentbool') 
                                   {
                                       RaiseXafCallback(globalCallbackControl, 'PTStudyLogTypePopup', 'CopyToAllCell|' + e.elementIndex, '', false);
                                   }
                                         
                                   //console.log('CopyValue:', FocusedColumn);
                                 }                                 
                               }        
                         } 
                         else if (FocusedColumn) 
                         {                                                             
                            var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn);                            
                            //console.log('CopyValue:', CopyValue);
                            if (e.item.name == 'CopyToAllCell')
                             {
                                 if (FocusedColumn=='Z_Score' || FocusedColumn=='PT_Value' || FocusedColumn=='Acceptable' || FocusedColumn=='Commentbool')
	                             {
                                   for (var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                   { 
                                     if (s.IsRowSelectedOnPage(i)) 
                                     {
                                        s.batchEditApi.SetCellValue(i, FocusedColumn, CopyValue);
                                     }
                                   }
                                 }
                                if (e.item.name === 'CopyToAllCell' && FocusedColumn === 'Commentbool') 
                                {
                                     RaiseXafCallback(globalCallbackControl, 'PTStudyLogTypePopup', 'CopyToAllCell|' + e.elementIndex, '', false);
                                }                              
                              }                            
                          }                    
                    }
                     e.processOnServer = false;
                     }";
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
                ASPxGridView gridView = sender as ASPxGridView;
                if (View.Id == "PTStudyLog_Results_ListView")
                {
                    gridView.JSProperties["cpVisibleRowCount"] = gridView.VisibleRowCount;
                }

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
                    CurrentLanguage currentLanguage = ObjectSpace.FindObject<CurrentLanguage>(CriteriaOperator.Parse("Oid is null"));
                    if (currentLanguage != null && currentLanguage.Chinese)
                    {
                        e.Items.Add("复制到所有单元格", "CopyToAllCell");
                    }
                    else
                    {
                        e.Items.Add("Copy To All Cell", "CopyToAllCell");
                    }
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
        private void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (View.Id == "PTStudyLog_Results_ListView")
                {
                    if (e.DataColumn.FieldName == "Commentbool")
                    {
                        e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'PTStudyLogTypePopup', 'Commentbool|'+{0}, '', false)", e.VisibleIndex));
                    }
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
            try
            {
                if (View.Id == "PTStudyLog_Results_ListView")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    foreach (Testparameter objTp in e.AcceptActionArgs.SelectedObjects)
                    {
                        PTStudyLogResults objResult = os.CreateObject<PTStudyLogResults>();
                        //Testparameter testparameter = os.FindObject<Testparameter>(CriteriaOperator.Parse("[TestMethod.MatrixName.Oid] = ? And [TestMethod.Oid] = ? And [TestMethod.MethodName.Oid] = ?", ObjPT.TestMethod.MatrixName.Oid,ObjPT.TestMethod.Oid,ObjPT.TestMethod.MethodName.Oid));
                        objResult.Matrix = os.GetObject(objTp.TestMethod.MatrixName);
                        objResult.Test = os.GetObject(objTp.TestMethod);
                        objResult.Method = os.GetObject(objTp.TestMethod.MethodName);
                        objResult.Parameter = os.GetObject(objTp.Parameter);
                        os.CommitChanges();
                        ((ListView)View).CollectionSource.Add(View.ObjectSpace.GetObject(objResult));
                        if (!objPTstudy.lstexistTestparamOid.Contains(objTp.Oid))
                        {
                            objPTstudy.lstexistTestparamOid.Add(objTp.Oid);
                        }
                    }
                    ((ListView)View).Refresh();

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Add_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            //try
            //{
            //    if (View.Id == "PTStudyLog_Results_ListView")
            //    {
            //        PTStudyLog objPT = Application.MainWindow.View.CurrentObject as PTStudyLog;
            //        if (objPT != null && objPT.Matrix != null )
            //        {
            //            if (objPT != null && objPT.Test != null)
            //            {
            //                if (objPT != null && objPT.Method != null)
            //                {
            //                    IObjectSpace objectSpace = Application.CreateObjectSpace();
            //                    CollectionSource cs = new CollectionSource(objectSpace, typeof(Testparameter));
            //                    List<Guid> lstOid = new List<Guid>();
            //                    List<string> lstMatrix = new List<string>();
            //                    List<string> lstTest = new List<string>();
            //                    List<string> lstMethods = new List<string>();
            //                    if (objPTstudy.lstexistTestparamOid == null)
            //                    {
            //                        objPTstudy.lstexistTestparamOid = new List<Guid>();
            //                    }
            //                    lstMatrix = objPT.Matrix.Split(';').ToList();
            //                    lstTest = objPT.Test.Split(';').ToList();
            //                    lstMethods = objPT.Method.Split(';').ToList();
            //                    foreach (string strMatr in lstMatrix.ToList())
            //                    {
            //                        foreach (string strtest in lstTest.ToList())
            //                        {
            //                            foreach (string strmethod in lstMethods.ToList())
            //                            {
            //                                Testparameter objParam = objectSpace.FindObject<Testparameter>((CriteriaOperator.Parse("[TestMethod.MatrixName.Oid] = ? And [TestMethod.Oid] = ? And [TestMethod.MethodName.Oid] = ? ", new Guid(strMatr), new Guid(strtest), new Guid(strmethod))));
            //                                if (objPTstudy.lstexistTestparamOid.Count>0)
            //                                {
            //                                    if (objParam != null && !lstOid.Contains(objParam.Oid) && !objPTstudy.lstexistTestparamOid.Contains(objParam.Oid))
            //                                    {
            //                                        lstOid.Add(objParam.Oid);
            //                                    } 
            //                                }
            //                                else
            //                                {
            //                                    if (objParam != null && !lstOid.Contains(objParam.Oid))
            //                                    {
            //                                        lstOid.Add(objParam.Oid);
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }


            //                    //cs.Criteria["filter"] = new InOperator("TestMethod.Matrix.MatrixName", lstMat.Select(i => i.Replace(" ", "")));
            //                    //cs.Criteria["filter1"] = new InOperator("TestMethod.TestName", lstT.Select(i => i.Replace(" ", "")));
            //                    cs.Criteria["filter2"] = new InOperator("Oid", lstOid);
            //                    cs.Criteria["filter3"] = new NotOperator(new InOperator("Oid", objPTstudy.lstexistTestparamOid));                               

            //                    ListView lv = Application.CreateListView("Testparameter_ListView_PTStudyLog", cs, false);
            //                    ShowViewParameters showViewParameters = new ShowViewParameters(lv);
            //                    showViewParameters.CreatedView = lv;
            //                    showViewParameters.Context = TemplateContext.PopupWindow;
            //                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            //                    DialogController dc = Application.CreateController<DialogController>();
            //                    //dc.SaveOnAccept = false;
            //                    //dc.CloseOnCurrentObjectProcessing = false;
            //                    dc.Accepting += Dc_Accepting;
            //                    //dc.AcceptAction.Executed += AcceptAction_Executed;
            //                    showViewParameters.Controllers.Add(dc);
            //                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            //                    return;
            //                }
            //                Application.ShowViewStrategy.ShowMessage("Please select the method.", InformationType.Error, 3000, InformationPosition.Top);
            //                return;
            //            }
            //            Application.ShowViewStrategy.ShowMessage("Please select the test.", InformationType.Error, 3000, InformationPosition.Top);
            //            return;
            //        }
            //        Application.ShowViewStrategy.ShowMessage("Please select the matrix.", InformationType.Error, 3000, InformationPosition.Top);
            //        return;

            //    }


            //}
            //catch (Exception ex)
            //{
            //    Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            //    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            //}
        }
        private void PTStudyDateFilter_SelectedItemChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (View != null && PTStudyDateFilter != null && PTStudyDateFilter.SelectedItem != null)
                {
                    string strSelectedItem = ((DevExpress.ExpressApp.Actions.SingleChoiceAction)sender).SelectedItem.Id.ToString();
                    if (View.Id == "PTStudyLog_ListView")
                    {
                        if (strSelectedItem == "1Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 1 And [CreatedDate] Is Not Null");
                        }
                        else if (strSelectedItem == "2Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 2 And [CreatedDate] Is Not Null");
                        }
                        else if (strSelectedItem == "5Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 5 And [CreatedDate] Is Not Null");
                        }
                        else if (strSelectedItem == "ALL")
                        {
                            ((ListView)View).CollectionSource.Criteria.Remove("DateFilter");
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

        private void PTStudyLogSubmit_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "PTStudyLog_ListView" || View.Id == "PTStudyLog_DetailView")
                {
                    if (e.SelectedObjects.Count > 0)
                    {
                        PTStudyLog objPTStudy = View.CurrentObject as PTStudyLog;
                        if (objPTStudy.StudyName != null && objPTStudy.StudyID != null)
                        {
                            if (objPTStudy.Status == PTStudyLogStatus.PendingSubmission)
                            {
                                foreach (PTStudyLog objPT in e.SelectedObjects)
                                {
                                    PTStudyLog Objstudylog = ObjectSpace.FindObject<PTStudyLog>(CriteriaOperator.Parse("Oid =?", objPT.Oid));
                                    Objstudylog.ReleaseBool = true;
                                    Objstudylog.Status = PTStudyLogStatus.Submitted;
                                    ObjectSpace.CommitChanges();
                                }
                                View.ObjectSpace.Refresh();
                                if (View.Id == "PTStudyLog_DetailView")
                                {
                                    View.Close();
                                }
                                Application.ShowViewStrategy.ShowMessage("Submitted successfully.", InformationType.Success, 3000, InformationPosition.Top);
                                return;
                            }
                            Application.ShowViewStrategy.ShowMessage("Please enter the 'PT_Value'.", InformationType.Error, 3000, InformationPosition.Top);
                            return;
                        }
                        Application.ShowViewStrategy.ShowMessage("Please fill 'Study Name' and 'StudyID' before submit.", InformationType.Error, 3000, InformationPosition.Top);
                        return;

                    }
                    Application.ShowViewStrategy.ShowMessage("Select the check box.", InformationType.Error, 3000, InformationPosition.Top);
                    return;
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                PTStudyLog pTStudyLog = Application.MainWindow.View.CurrentObject as PTStudyLog;
                if (View.Id == "PTStudyLog_DetailView")
                {
                    if (pTStudyLog.DatePTSampleReceived > DateTime.Now && pTStudyLog.DatePTSampleReceived != null)
                    {
                        Application.ShowViewStrategy.ShowMessage("Date PT sameple received must be less than current date.", InformationType.Warning, 3000, InformationPosition.Top);

                        return;
                    }
                    else if (pTStudyLog.DateLabResultSubmitted < pTStudyLog.DatePTSampleReceived && pTStudyLog.DateLabResultSubmitted != null && pTStudyLog.DateLabResultSubmitted != DateTime.MinValue)
                    {
                        Application.ShowViewStrategy.ShowMessage("Date lab result submitted must be greater than date PT sample received.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        return;
                    }
                    else if (pTStudyLog.DatePTResultReceived < pTStudyLog.DateLabResultSubmitted && pTStudyLog.DatePTResultReceived != null && pTStudyLog.DatePTResultReceived != DateTime.MinValue)
                    {
                        Application.ShowViewStrategy.ShowMessage("Date PT result received must be greater than the date lab result submitted.", InformationType.Warning, 3000, InformationPosition.Top);
                        return;
                    }

                    else if (pTStudyLog.StudyName != null && pTStudyLog.StudyID != null && pTStudyLog.Status == PTStudyLogStatus.PendingPTResultEntry)
                    {
                        ListPropertyEditor lstResults = ((DetailView)View).FindItem("Results") as ListPropertyEditor;
                        if (lstResults != null && lstResults.ListView != null)
                        {
                            PTStudyLogResults obj = ((ListView)lstResults.ListView).CollectionSource.List.Cast<PTStudyLogResults>().FirstOrDefault(i => i.PT_Value == null || i.Z_Score == null || i.Acceptable == TypesOfAcceptable.NA);
                            if (((ListView)lstResults.ListView).CollectionSource.List.Cast<PTStudyLogResults>().FirstOrDefault(i => i.PT_Value == null || i.Z_Score == null || i.Acceptable == TypesOfAcceptable.NA) == null)
                            {
                                pTStudyLog.Status = PTStudyLogStatus.PendingSubmission;
                                View.ObjectSpace.CommitChanges();
                            }
                        }
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
        //private void ResultRemove_Executing(object sender, SimpleActionExecuteEventArgs e)
        //{

        //    //try
        //    //{
        //    //    if (e.SelectedObjects.Count > 0)
        //    //    {
        //    //        foreach (PTStudyLogResults objResult in e.SelectedObjects)
        //    //        {
        //    //            ((ListView)View).CollectionSource.List.Remove(objResult);
        //    //        }
        //    //           ((ListView)View).Refresh();
        //    //    }
        //    //    else
        //    //    {
        //    //        Application.ShowViewStrategy.ShowMessage("Select the check box", InformationType.Error, 4000, InformationPosition.Top);
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //    //    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    //}
        //}
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            try
            {
                if (View.Id == "PTStudyLog_DetailView")
                {
                    if (objPTstudy.lstexistTestparamOid != null && objPTstudy.lstexistTestparamOid.Count > 0)
                    {
                        objPTstudy.lstexistTestparamOid.Clear();
                    }
                }
                //ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                //ResultRemove.Execute -= ResultRemove_Executing;
                SimpleAction objDelete = Frame.GetController<DeleteObjectsViewController>().DeleteAction;
                if (objDelete != null)
                {
                    objDelete.Caption = "Delete";
                    objDelete.ImageName = "Action_Delete";
                    objDelete.ConfirmationMessage = @"You are about to delete the selected record(s). Do you want to proceed?";
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
                    if (View.Id == "PTStudyLog_ListView")
                    {
                        if (bool.TryParse(parameter, out bool CacelContract))
                        {
                            if (CacelContract)
                            {
                                if (Application.MainWindow.View is ListView)
                                {
                                    foreach (PTStudyLog objPslin in View.SelectedObjects.Cast<PTStudyLog>().ToList())
                                    {
                                        ((ListView)View).CollectionSource.Remove(objPslin);
                                        ObjectSpace.Delete(objPslin);
                                    }
                                }
                                ObjectSpace.CommitChanges();
                                Application.ShowViewStrategy.ShowMessage("Delete successfully.", InformationType.Success, 4000, InformationPosition.Top);
                            }
                            View.ObjectSpace.Refresh();
                        }
                    }
                    else
                    {
                        string[] param = parameter.Split('|');
                        if (param.Length >= 2 && param[0] == "CopyToAllCell")
                        {
                            ASPxGridListEditor gridListEditor1 = ((ListView)View).Editor as ASPxGridListEditor;
                            if (gridListEditor1 != null)
                            {
                                IObjectSpace os = Application.CreateObjectSpace();
                                int rowIndex = int.Parse(param[1]);
                                string strGuid = gridListEditor1.Grid.GetRowValues(rowIndex, "Oid").ToString();
                                HttpContext.Current.Session["rowid"] = strGuid;
                                IList<PTStudyLogResults> objResults = os.GetObjects<PTStudyLogResults>(CriteriaOperator.Parse("Oid = ?", new Guid(strGuid)));

                                // Check if the PTStudyLogResults object is found
                                if (objResults != null && objResults.Count > 0)
                                {
                                    PTStudyLogResults objre = objResults.First();
                                    // Check if the Commentbool property is true
                                    if (objre.Commentbool == false || objre.Commentbool)
                                    {
                                        // Retrieve the Comments value from the selected row
                                        string commentsValue = gridListEditor1.Grid.GetRowValues(rowIndex, "Comment")?.ToString();
                                        // Get the selected rows
                                        List<object> selectedRowKeys = gridListEditor1.Grid.GetSelectedFieldValues("Oid");
                                        // Update all selected rows with the same Comments value
                                        foreach (object selectedRowKey in selectedRowKeys)
                                        {
                                            // You may need to convert the selectedRowKey to Guid based on your data model
                                            Guid rowGuid = (Guid)selectedRowKey;
                                            // Retrieve the PTStudyLogResults object for the selected row
                                            PTStudyLogResults selectedRow = os.GetObjectByKey<PTStudyLogResults>(rowGuid);
                                            // Update the Comments value
                                            selectedRow.Comment = commentsValue;
                                            if (selectedRow.Comment != null)
                                            {
                                                objre.Commentbool = true;
                                            }
                                            else
                                            {
                                                objre.Commentbool = false;
                                            }

                                        }
                                        // Commit changes to the Object Space
                                        os.CommitChanges();
                                        View.ObjectSpace.CommitChanges();
                                        View.ObjectSpace.Refresh();
                                    }
                                }
                            }
                        }
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridListEditor != null)
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            string strGuid = gridListEditor.Grid.GetRowValues(int.Parse(param[1]), "Oid").ToString();
                            HttpContext.Current.Session["rowid"] = strGuid;
                            IList<PTStudyLogResults> objResults = os.GetObjects<PTStudyLogResults>(CriteriaOperator.Parse("Oid= ?", new Guid(strGuid)));

                            if (param[0] == "Commentbool" && !string.IsNullOrEmpty(strGuid))
                            {

                                if (objResults != null)
                                {
                                    if (View.Id == "PTStudyLog_Results_ListView")
                                    {

                                        foreach (PTStudyLogResults objre in objResults.ToList())
                                        {
                                            PTStudyLogComment obj = os.CreateObject<PTStudyLogComment>();
                                            obj.Comment = objre.Comment;
                                            DetailView createdView = Application.CreateDetailView(os, "PTStudyLogComment_DetailView", true, obj);
                                            createdView.ViewEditMode = ViewEditMode.Edit;
                                            ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                                            showViewParameters.Context = TemplateContext.NestedFrame;
                                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                            showViewParameters.CreatedView = createdView;
                                            DialogController dc = Application.CreateController<DialogController>();
                                            dc.SaveOnAccept = false;
                                            // dc.Accepting += RollBack_Accepting;
                                            dc.CloseOnCurrentObjectProcessing = false;
                                            showViewParameters.Controllers.Add(dc);
                                            dc.Accepting += Dc_Accepting1;
                                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
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

        private void Dc_Accepting1(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                DialogController objDialog = (DialogController)sender as DialogController;
                PTStudyLog objPT = Application.MainWindow.View.CurrentObject as PTStudyLog;
                ASPxStringPropertyEditor obj = ((DetailView)objDialog.Frame.View).FindItem("Comment") as ASPxStringPropertyEditor;
                if (objDialog != null && objDialog.Frame != null && objDialog.Frame.View != null)
                {
                    DevExpress.ExpressApp.View views = objDialog.Frame.View;
                    if (obj.ControlValue != null)
                    {
                        if (obj != null && !string.IsNullOrEmpty(obj.ControlValue.ToString()))
                        {
                            if (HttpContext.Current.Session["rowid"] != null)
                            {
                                PTStudyLogResults objResult = View.ObjectSpace.FindObject<PTStudyLogResults>(CriteriaOperator.Parse("Oid=?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                                if (objPT.StudyID != null && objPT.StudyName != null)
                                {
                                    if (objResult != null)
                                    {
                                        objResult.Comment = obj.ControlValue.ToString();
                                        objResult.Commentbool = true;

                                    }
                                    View.Refresh();
                                    View.ObjectSpace.CommitChanges();
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage("Please enter the mondatory fields", InformationType.Error, 4000, InformationPosition.Top);
                                }

                            }
                        }
                    }
                    else
                    {
                        PTStudyLogResults objResult = View.ObjectSpace.FindObject<PTStudyLogResults>(CriteriaOperator.Parse("Oid=?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        if (objPT.StudyID != null && objPT.StudyName != null)
                        {
                            if (objResult != null)
                            {
                                objResult.Comment = null;
                                objResult.Commentbool = false;

                            }
                            View.Refresh();
                            View.ObjectSpace.CommitChanges();
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
        private void Dc_Accepting2(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                ItemsFileUpload objFileUpload = (ItemsFileUpload)e.AcceptActionArgs.CurrentObject;
                //DialogController itemsFile = (DialogController)sender as DialogController;
                if (objFileUpload != null && objFileUpload.InputFile != null)
                {
                    byte[] file = objFileUpload.InputFile.Content;
                    string fileExtension = Path.GetExtension(objFileUpload.InputFile.FileName);
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
                            ListPropertyEditor lstResults = ((DetailView)Application.MainWindow.View).FindItem("Results") as ListPropertyEditor;
                            if (lstResults != null && lstResults.ListView != null)
                            {
                                foreach (PTStudyLogResults objResults in ((ListView)lstResults.ListView).CollectionSource.List.Cast<PTStudyLogResults>().ToList())
                                {
                                    if (objResults.SampleID != null && objResults.SampleID.Testparameter.TestMethod.MatrixName.MatrixName != null && objResults.SampleID.Testparameter.TestMethod.TestName != null
                                        && objResults.SampleID.Testparameter.TestMethod.MethodName.MethodNumber != null && objResults.SampleID.Testparameter.Parameter.ParameterName != null)
                                    {

                                        string strPattern = @"(?:\r|\n|\a|\t|)";
                                        //string strPattern = @"(?:\r|\n|\a|\t|[@%])";
                                        if (!string.IsNullOrEmpty(row[0].ToString()))
                                        {
                                            row[0] = System.Text.RegularExpressions.Regex.Replace(row[0].ToString(), strPattern, string.Empty);
                                        }
                                        if (!string.IsNullOrEmpty(row[1].ToString()))
                                        {
                                            row[1] = System.Text.RegularExpressions.Regex.Replace(row[1].ToString(), strPattern, string.Empty);
                                        }
                                        if (!string.IsNullOrEmpty(row[2].ToString()))
                                        {
                                            row[2] = System.Text.RegularExpressions.Regex.Replace(row[2].ToString(), strPattern, string.Empty);
                                        }
                                        if (!string.IsNullOrEmpty(row[3].ToString()))
                                        {
                                            row[3] = System.Text.RegularExpressions.Regex.Replace(row[3].ToString(), strPattern, string.Empty);
                                        }
                                        if (!string.IsNullOrEmpty(row[4].ToString()))
                                        {
                                            row[4] = System.Text.RegularExpressions.Regex.Replace(row[4].ToString(), strPattern, string.Empty);
                                        }

                                        if (objResults.SampleID.Samplelogin.SampleID.Trim() == row[0].ToString().Trim()
                                            && objResults.SampleID.Testparameter.TestMethod.MatrixName.MatrixName.Trim() == row[1].ToString().Trim()
                                            && objResults.SampleID.Testparameter.TestMethod.TestName.Trim() == row[2].ToString().Trim()
                                            && objResults.SampleID.Testparameter.TestMethod.MethodName.MethodNumber.Trim() == row[3].ToString().Trim()
                                            && objResults.SampleID.Testparameter.Parameter.ParameterName.Trim() == row[4].ToString().Trim())
                                        {
                                            if (dt.Columns.Contains("Z-Score") && !row.IsNull("Z-Score"))
                                            {
                                                strZ_Score = row[("Z-Score")].ToString();
                                            }
                                            else
                                            {
                                                strZ_Score = string.Empty;
                                            }
                                            if (dt.Columns.Contains("PT_Value") && !row.IsNull("PT_Value"))
                                            {
                                                strPT_Value = row[("PT_Value")].ToString();
                                            }
                                            else
                                            {
                                                strPT_Value = string.Empty;
                                            }
                                            if (dt.Columns.Contains("Acceptable") && !row.IsNull("Acceptable"))
                                            {
                                                strAcceptable = row[("Acceptable")].ToString().Trim();
                                            }
                                            else
                                            {
                                                strAcceptable = string.Empty;
                                            }
                                            if (dt.Columns.Contains("Comment") && !row.IsNull("Comment"))
                                            {
                                                strComments = row[("Comment")].ToString();
                                            }
                                            else
                                            {
                                                strComments = string.Empty;
                                            }
                                            objResults.Z_Score = strZ_Score.Trim();
                                            objResults.PT_Value = strPT_Value.Trim();
                                            //if (strAcceptable != null)
                                            //{
                                            //    if (strAcceptable == "Yes")
                                            //    {
                                            //        objResults.Acceptable = TypesOfAcceptable.Yes;
                                            //    }
                                            //    else if(strAcceptable == "No")
                                            //    {
                                            //        objResults.Acceptable = TypesOfAcceptable.No;
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    objResults.Acceptable = TypesOfAcceptable.NA;
                                            //}
                                            if (Enum.TryParse<TypesOfAcceptable>(strAcceptable, out var enumValue))
                                            {
                                                objResults.Acceptable = (TypesOfAcceptable)enumValue;
                                            }
                                            else
                                            {
                                                objResults.Acceptable = TypesOfAcceptable.NA;
                                            }
                                            objResults.Comment = strComments.Trim();
                                            if (objResults.Comment != "")
                                            {
                                                objResults.Commentbool = true;
                                            }
                                            else
                                            {
                                                objResults.Commentbool = false;
                                            }


                                            ObjectSpace.CommitChanges();
                                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "importdata"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                        }
                                    }
                                }
                            }
                        }
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
        }
        //private void ImportImportResultsFileAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        //{
        //    try
        //    {
        //        PTStudyLog objPT = Application.MainWindow.View.CurrentObject as PTStudyLog;
        //        if (objPT !=null &&objPT.StudyName !=null && objPT.StudyID !=null)
        //        {
        //            IObjectSpace sheetObjectSpace = Application.CreateObjectSpace(typeof(ItemsFileUpload));
        //            ItemsFileUpload spreadSheet = sheetObjectSpace.CreateObject<ItemsFileUpload>();
        //            DetailView createdView = Application.CreateDetailView(sheetObjectSpace, spreadSheet);
        //            createdView.ViewEditMode = ViewEditMode.Edit;
        //            e.View = createdView; 
        //        }
        //        else
        //        {
        //            Application.ShowViewStrategy.ShowMessage("Please fill 'Study Name' and 'Study ID' before import file.", InformationType.Error, 4000, InformationPosition.Top);
        //            return;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        //private void ImportFileAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        //{
        //    try
        //    {              
        //        ItemsFileUpload itemsFile = (ItemsFileUpload)e.PopupWindowViewCurrentObject;
        //        if (itemsFile.InputFile != null)
        //        {
        //            byte[] file = itemsFile.InputFile.Content;
        //            string fileExtension = Path.GetExtension(itemsFile.InputFile.FileName);
        //            DevExpress.Spreadsheet.Workbook workbook = new DevExpress.Spreadsheet.Workbook();
        //            if (fileExtension == ".xlsx")
        //            {
        //                workbook.LoadDocument(file, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
        //            }
        //            else if (fileExtension == ".xls")
        //            {
        //                workbook.LoadDocument(file, DevExpress.Spreadsheet.DocumentFormat.Xls);
        //            }
        //            DevExpress.Spreadsheet.WorksheetCollection worksheets = workbook.Worksheets;
        //            DevExpress.Spreadsheet.Worksheet worksheet = workbook.Worksheets[0];
        //            CellRange range = worksheet.Range.FromLTRB(0, 0, worksheet.Columns.LastUsedIndex, worksheet.GetUsedRange().BottomRowIndex);
        //            DataTable dt = worksheet.CreateDataTable(range, true);
        //            for (int col = 0; col < range.ColumnCount; col++)
        //            {
        //                CellValueType cellType = range[0, col].Value.Type;
        //                for (int r = 1; r < range.RowCount; r++)
        //                {
        //                    if (cellType != range[r, col].Value.Type)
        //                    {
        //                        dt.Columns[col].DataType = typeof(string);
        //                        break;
        //                    }
        //                }
        //            }
        //            DevExpress.Spreadsheet.Export.DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dt, false);
        //            IObjectSpace os = Application.CreateObjectSpace();
        //            Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
        //            UnitOfWork uow = new UnitOfWork(((XPObjectSpace)os).Session.DataLayer);
        //            exporter.Export();
        //            if (dt != null && dt.Rows.Count > 0)
        //            {
        //                DataRow row1 = dt.Rows[0];
        //                if (row1[0].ToString() == dt.Columns[0].Caption)
        //                {
        //                    row1.Delete();
        //                    dt.AcceptChanges();
        //                }
        //                foreach (DataColumn c in dt.Columns)
        //                    c.ColumnName = c.ColumnName.ToString().Trim();

        //                foreach (DataRow row in dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(c => c is DBNull)))
        //                {
        //                    ListPropertyEditor lstResults = ((DetailView)Application.MainWindow.View).FindItem("Results") as ListPropertyEditor;
        //                    if (lstResults != null && lstResults.ListView !=null)
        //                    {
        //                        foreach (PTStudyLogResults objResults in ((ListView)lstResults.ListView).CollectionSource.List.Cast<PTStudyLogResults>().ToList())
        //                        {
        //                            if(objResults.SampleID !=null && objResults.SampleID.Testparameter.TestMethod.MatrixName.MatrixName !=null && objResults.SampleID.Testparameter.TestMethod.TestName !=null 
        //                                && objResults.SampleID.Testparameter.TestMethod.MethodName.MethodNumber !=null && objResults.SampleID.Testparameter.Parameter.ParameterName !=null)
        //                            {

        //                                string strPattern = @"(?:\r|\n|\a|\t|)";
        //                                if (!string.IsNullOrEmpty(row[0].ToString()))
        //                                {
        //                                    row[0] = System.Text.RegularExpressions.Regex.Replace(row[0].ToString(), strPattern, string.Empty);
        //                                }
        //                                if (!string.IsNullOrEmpty(row[1].ToString()))
        //                                {
        //                                    row[0] = System.Text.RegularExpressions.Regex.Replace(row[0].ToString(), strPattern, string.Empty);
        //                                }
        //                                if (!string.IsNullOrEmpty(row[2].ToString()))
        //                                {
        //                                    row[2] = System.Text.RegularExpressions.Regex.Replace(row[2].ToString(), strPattern, string.Empty);
        //                                }
        //                                if (!string.IsNullOrEmpty(row[3].ToString()))
        //                                {
        //                                    row[3] = System.Text.RegularExpressions.Regex.Replace(row[3].ToString(), strPattern, string.Empty);
        //                                }
        //                                if (!string.IsNullOrEmpty(row[4].ToString()))
        //                                {
        //                                    row[4] = System.Text.RegularExpressions.Regex.Replace(row[4].ToString(), strPattern, string.Empty);
        //                                }

        //                                if (objResults.SampleID.Samplelogin.SampleID == row[0].ToString().Trim()
        //                                    && objResults.SampleID.Testparameter.TestMethod.MatrixName.MatrixName ==row[1].ToString().Trim()
        //                                    && objResults.SampleID.Testparameter.TestMethod.TestName ==row[2].ToString().Trim()
        //                                    &&  objResults.SampleID.Testparameter.TestMethod.MethodName.MethodNumber==row[3].ToString().Trim()
        //                                    && objResults.SampleID.Testparameter.Parameter.ParameterName ==row[4].ToString().Trim())
        //                                {
        //                                    if (dt.Columns.Contains("Z-Score") && !row.IsNull("Z-Score"))
        //                                    {
        //                                        strZ_Score = row[("Z-Score")].ToString();
        //                                    }
        //                                    else
        //                                    {
        //                                        strZ_Score = string.Empty;
        //                                    }
        //                                    if (dt.Columns.Contains("PT_Value") && !row.IsNull("PT_Value"))
        //                                    {
        //                                        strPT_Value = row[("PT_Value")].ToString();
        //                                    }
        //                                    else
        //                                    {
        //                                        strPT_Value = string.Empty;
        //                                    }
        //                                    objResults.Z_Score = strZ_Score.Trim();
        //                                    objResults.PT_Value = strPT_Value.Trim();
        //                                    ObjectSpace.CommitChanges();
        //                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "importdata"), InformationType.Success, timer.Seconds, InformationPosition.Top);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "uploadfile"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void ImportResultsFileAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                PTStudyLog objPT = Application.MainWindow.View.CurrentObject as PTStudyLog;
                if (objPT != null && objPT.StudyName != null && objPT.StudyID != null)
                {
                    IObjectSpace sheetObjectSpace = Application.CreateObjectSpace(typeof(ItemsFileUpload));
                    ItemsFileUpload spreadSheet = sheetObjectSpace.CreateObject<ItemsFileUpload>();
                    DetailView createdView = Application.CreateDetailView(sheetObjectSpace, spreadSheet);
                    createdView.ViewEditMode = ViewEditMode.Edit;
                    ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                    showViewParameters.Context = TemplateContext.NestedFrame;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    showViewParameters.CreatedView = createdView;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    // dc.Accepting += RollBack_Accepting;
                    dc.CloseOnCurrentObjectProcessing = false;
                    showViewParameters.Controllers.Add(dc);
                    dc.Accepting += Dc_Accepting2;
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));

                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("Please fill 'Study Name' and 'Study ID' before import file.", InformationType.Error, 4000, InformationPosition.Top);
                    return;
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
