using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.FileAttachments.Web;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;

namespace LDM.Module.Controllers.DocumentManagement
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ManualDocumentManagementController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        DocumentAttachmentInfo objDocInfo = new DocumentAttachmentInfo();
        NavigationRefresh objNavigationRefresh = new NavigationRefresh();
        public ManualDocumentManagementController()
        {
            InitializeComponent();
            TargetViewId = "Manual_Attachments_ListView;" + "DocumentAttachment_ListView;" + "DocumentAttachment_DetailView;" + "Manual_DetailView;" + "Manual_DetailView_IsRetired;" +
                "Manual_ListView;" + "Manual_ListView_IsRetired;" + "DocumentCategory_ListView;" + "DocumentCategory_DetailView;" + "Manual_ListView_Category;" + "Manual_Attachments_ListView;";

            btnUnRetire.TargetViewId = "Manual_ListView_IsRetired;";
            btnUnRetire.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;

                if (View.Id == "Manual_DetailView" || View.Id == "Manual_ListView")
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Execute += DeleteAction_Execute;
                }
                if (View.Id == "Manual_ListView_IsRetired")
                {
                    btnUnRetire.Active["hideClose"] = false;
                    Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                    if (currentUser != null && currentUser.UserName != "Administrator" && currentUser.UserName != "Service")
                    {
                        foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                        {
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "RetiredDocument" && i.Write == true) != null)
                            {
                                btnUnRetire.Active.RemoveItem("hideClose");
                            }
                        }

                    }
                    else
                    {
                        btnUnRetire.Active.RemoveItem("hideClose");
                    }
                }

                if (View.Id == "Manual_DetailView")
                {
                    ModificationsController objModify = Frame.GetController<ModificationsController>();
                    objModify.SaveAction.Executing += SaveAction_Executing;
                    Manual objManual = View.CurrentObject as Manual;
                    if (objManual != null)
                    {
                        objDocInfo.DocumentIDOid = objManual.Oid;
                    }
                    if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                    {
                        ASPxStringPropertyEditor hideDownload = (ASPxStringPropertyEditor)((DetailView)View).FindItem("Download");
                        if (hideDownload is IAppearanceVisibility)
                        {
                            ((IAppearanceVisibility)hideDownload).Visibility = (((DetailView)View).ViewEditMode == ViewEditMode.View) ? ViewItemVisibility.Hide : ViewItemVisibility.Show;
                        }
                    }
                }

                if (Application.MainWindow.View.Id == "Manual_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Executed += SaveAction_Executed;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executed += SaveAction_Executed;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Executed += SaveAction_Executed;

                    //if (Frame is NestedFrame)
                    //{
                    //    NestedFrame nestedFrame = (NestedFrame)Frame;
                    //    Manual objManual = ((Manual)Application.MainWindow.View.CurrentObject);
                    //    if (objManual != null)
                    //    {
                    //        if (nestedFrame != null && nestedFrame.ViewItem.Id != null && nestedFrame.ViewItem.View != null)
                    //        {
                    //            CompositeView cv = nestedFrame.ViewItem.View;
                    //            if (cv != null && cv.CurrentObject != null)
                    //            {
                    //                NewObjectViewController objNew = Frame.GetController<NewObjectViewController>();
                    //                if (objNew != null)
                    //                {
                    //                    objNew.NewObjectAction.Executing += NewObjectAction_Executing;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
                if (View.Id == "Manual_ListView_Category")
                {
                    View.Caption = objNavigationRefresh.SelectedNavigationItem;
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Category.Name] = ?", objNavigationRefresh.SelectedNavigationItem);
                    //try
                    //{
                    //    if (objNavigationRefresh.SelectedNavigationItem != null)
                    //    {
                    //        View.Caption = objNavigationRefresh.SelectedNavigationItem;
                    //        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Category.Name] = ?", objNavigationRefresh.SelectedNavigationItem);
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                    //    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //}
                }

                else if (View.Id == "DocumentAttachment_DetailView")
                {
                    FileDataPropertyEditor FilePropertyEditor = ((DetailView)View).FindItem("Attachment") as FileDataPropertyEditor;
                    if (FilePropertyEditor != null)
                    {
                        FilePropertyEditor.ControlCreated += FilePropertyEditor_ControlCreated;
                    }
                    //DocumentAttachment objAttachment = View.CurrentObject as DocumentAttachment;
                    //if (objAttachment != null)
                    //{
                    //    objDocInfo.RevNumberOid = objAttachment.Oid;
                    //}
                }
                else if (View.Id == "Manual_Attachments_ListView")
                {
                    NewObjectViewController objNew = Frame.GetController<NewObjectViewController>();
                    if (objNew != null)
                    {
                        objNew.NewObjectAction.Executing += NewObjectAction_Executing;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void NewObjectAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "Manual_Attachments_ListView")
                {
                    objDocInfo.rowCount = ((ListView)View).CollectionSource.GetCount();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                ShowNavigationItemController showNavigationItemController = Frame.GetController<ShowNavigationItemController>();
                if (showNavigationItemController != null)
                {
                    showNavigationItemController.RecreateNavigationItems();
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
                ShowNavigationItemController showNavigationItemController = Frame.GetController<ShowNavigationItemController>();
                if (showNavigationItemController != null)
                {
                    showNavigationItemController.RecreateNavigationItems();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void FilePropertyEditor_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                FileDataEdit FileControl = ((FileDataPropertyEditor)sender).Editor;
                if (FileControl != null)
                {
                    FileControl.UploadControlCreated += FileControl_UploadControlCreated;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void FileControl_UploadControlCreated(object sender, EventArgs e)
        {
            try
            {
                ASPxUploadControl FileUploadControl = ((FileDataEdit)sender).UploadControl;
                FileUploadControl.ValidationSettings.AllowedFileExtensions = new String[] { ".pdf" };
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PopupWindowManager_PopupShowing(object sender, PopupShowingEventArgs e)
        {
            try
            {
                e.PopupControl.CustomizePopupWindowSize += PopupControl_CustomizePopupWindowSize;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PopupControl_CustomizePopupWindowSize(object sender, CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                if (e.PopupFrame.View != null && e.PopupFrame.View.Id == "DocumentAttachment_DetailView")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(600);
                    e.Height = new System.Web.UI.WebControls.Unit(400);
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void NewObjectAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    if (Application.MainWindow.View.Id == "Manual_DetailView")
        //    {
        //        ObjectSpace.CommitChanges();
        //    }
        //}

        private void SaveAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Manual objManual = View.CurrentObject as Manual;
                if (objManual.IsRetire == true)
                {
                    if (!ObjectSpace.IsNewObject(objManual))
                    {
                        if (objManual.ReasonForRetiring == null)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ReasonForRetiring"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        objManual.IsRetire = false;
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ActiveFileOnlyRetire"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        e.Cancel = true;
                    }
                }
                else if (objManual.IsRetire == true && objManual.DateRetired == DateTime.Now && objManual.ReasonForRetiring != null)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "DocRetiredSuccessfully"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
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
                if (Application.MainWindow.View.Id == "Manual_DetailView")
                {
                    if (Frame is NestedFrame)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        Manual objManual = ((Manual)Application.MainWindow.View.CurrentObject);
                        if (objManual != null)
                        {
                            if (nestedFrame != null && nestedFrame.ViewItem.Id != null && nestedFrame.ViewItem.View != null)
                            {
                                CompositeView cv = nestedFrame.ViewItem.View;
                                if (cv != null && cv.CurrentObject != null)
                                {
                                    int RowCount = ((ListView)View).CollectionSource.GetCount();
                                    if (RowCount > -1)
                                    {
                                        objDocInfo.rowCount = RowCount;
                                    }
                                }
                            }
                        }
                    }
                }

                //if (View.Id == "DocumentAttachment_ListView")
                //{
                //    int RowCount = ((ListView)View).CollectionSource.GetCount();
                //    if (RowCount > -1)
                //    {
                //        objDocInfo.rowCount = RowCount;
                //    }
                //}

                if (View.Id == "Manual_ListView_Category")
                {
                    XafCallbackManager parameter = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    parameter.RegisterHandler("AttachmentViewMode", this);

                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        ASPxGridView gv = gridListEditor.Grid;
                        if (gv != null)
                        {
                            gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                            gv.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                        }
                    }
                }
                if (View.Id == "Manual_DetailView" || View.Id == "DocumentAttachment_DetailView")
                {
                    foreach (ViewItem item in ((DetailView)View).Items)
                    {
                        if (item.GetType() == typeof(ASPxStringPropertyEditor))
                        {
                            ASPxStringPropertyEditor propertyEditor = (ASPxStringPropertyEditor)item;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                        {
                            ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxBooleanPropertyEditor))
                        {
                            ASPxBooleanPropertyEditor propertyEditor = item as ASPxBooleanPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                        {
                            ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                                propertyEditor.Editor.CalendarCustomDisabledDate += Editor_CalendarCustomDisabledDate;
                            }
                        }
                    }
                }

                if (View.Id == "DocumentCategory_DetailView")
                {
                    if ((((DetailView)View).ViewEditMode == ViewEditMode.View))
                    {
                        NewObjectViewController objNew = Frame.GetController<NewObjectViewController>();
                        if (objNew != null)
                        {
                            objNew.NewObjectAction.Active["DisableNew"] = false;
                        }
                        ListViewController objEdit = Frame.GetController<ListViewController>();
                        if (objEdit != null)
                        {
                            objEdit.EditAction.Active["DisableEdit"] = false;
                        }
                        DeleteObjectsViewController ObjDelete = Frame.GetController<DeleteObjectsViewController>();
                        if (ObjDelete != null)
                        {
                            ObjDelete.DeleteAction.Active["DisableDelete"] = false;
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

        private void Editor_CalendarCustomDisabledDate(object sender, CalendarCustomDisabledDateEventArgs e)
        {
            try
            {
                if (e.Date < DateTime.Today)
                {
                    e.IsDisabled = true;
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
                if (e.DataColumn.FieldName == "Download")
                {
                    e.Cell.Attributes.Add("onclick", "RaiseXafCallback(globalCallbackControl, 'AttachmentViewMode'," + e.VisibleIndex + " , '', false);");
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
            try
            {
                if (View.Id == "Manual_DetailView")
                {
                    ModificationsController objModify = Frame.GetController<ModificationsController>();
                    objModify.SaveAction.Executing -= SaveAction_Executing;
                }

                if (Application.MainWindow.View.Id == "Manual_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Executed -= SaveAction_Executed;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executed -= SaveAction_Executed;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Executed -= SaveAction_Executed;

                    //if (Frame is NestedFrame)
                    //{
                    //    NestedFrame nestedFrame = (NestedFrame)Frame;
                    //    Manual objManual = ((Manual)Application.MainWindow.View.CurrentObject);
                    //    if (objManual != null)
                    //    {
                    //        if (nestedFrame != null && nestedFrame.ViewItem.Id != null && nestedFrame.ViewItem.View != null)
                    //        {
                    //            CompositeView cv = nestedFrame.ViewItem.View;
                    //            if (cv != null && cv.CurrentObject != null)
                    //            {
                    //                NewObjectViewController objNew = Frame.GetController<NewObjectViewController>();
                    //                if (objNew != null)
                    //                {
                    //                    objNew.NewObjectAction.Executing -= NewObjectAction_Executing;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
                if (View.Id == "Manual_DetailView" || View.Id == "Manual_ListView")
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Execute -= DeleteAction_Execute;
                }
                else if (View.Id == "Manual_Attachments_ListView")
                {
                    NewObjectViewController objNew = Frame.GetController<NewObjectViewController>();
                    if (objNew != null)
                    {
                        objNew.NewObjectAction.Executing -= NewObjectAction_Executing;
                    }
                }
                if (View.Id == "DocumentCategory_DetailView")
                {
                    if ((((DetailView)View).ViewEditMode == ViewEditMode.View))
                    {
                        NewObjectViewController objNew = Frame.GetController<NewObjectViewController>();
                        if (objNew != null)
                        {
                            objNew.NewObjectAction.Active["DisableNew"] = true;
                        }
                        ListViewController objEdit = Frame.GetController<ListViewController>();
                        if (objEdit != null)
                        {
                            objEdit.EditAction.Active["DisableEdit"] = true;
                        }
                        DeleteObjectsViewController ObjDelete = Frame.GetController<DeleteObjectsViewController>();
                        if (ObjDelete != null)
                        {
                            ObjDelete.DeleteAction.Active["DisableDelete"] = true;
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
        private void btnUnRetire_simpleAction1_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Manual_ListView_IsRetired")
                {
                    Manual objManual = View.CurrentObject as Manual;
                    if (objManual != null)
                    {
                        objManual.IsRetire = false;
                        objManual.DateRetired = DateTime.Now;
                        objManual.ReasonForRetiring = null;
                    }
                    ObjectSpace.CommitChanges();
                    View.ObjectSpace.Refresh();
                    //Frame.GetController<RefreshController>().RefreshAction.DoExecute();
                }
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "UnRetireTheDocument"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                ShowNavigationItemController showNavigationItemController = Frame.GetController<ShowNavigationItemController>();
                if (showNavigationItemController != null)
                {
                    showNavigationItemController.RecreateNavigationItems();
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
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        string strDownload = gridListEditor.Grid.GetRowValues(int.Parse(parameter), "Download").ToString();
                        if (strDownload == "Available")
                        {
                            MemoryStream tempms = new MemoryStream();
                            Guid oid = new Guid(gridListEditor.Grid.GetRowValues(int.Parse(parameter), "Oid").ToString());
                            Manual objManual = ObjectSpace.GetObjectByKey<Manual>(oid);
                            objManual.Attachments.FirstOrDefault(i => i.IsActive).Attachment.SaveToStream(tempms);
                            NonPersistentObjectSpace Popupos = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(PDFPreview));
                            PDFPreview objToShow = (PDFPreview)Popupos.CreateObject(typeof(PDFPreview));
                            objToShow.PDFData = tempms.ToArray();
                            DetailView CreatedDetailView = Application.CreateDetailView(Popupos, objToShow);
                            CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                            ShowViewParameters showViewParameters = new ShowViewParameters(CreatedDetailView);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            showViewParameters.CreatedView.Caption = "PDFViewer";
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.AcceptAction.Active.SetItemValue("disable", false);
                            dc.CancelAction.Active.SetItemValue("disable", false);
                            dc.CloseOnCurrentObjectProcessing = false;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
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
