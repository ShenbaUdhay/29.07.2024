using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Controllers.ICM
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class AsgnItemstoTestViewController : ViewController, IXafCallbackHandler
    {
        AssignItemsToTestInfo objAssignItemsToTestInfo = new AssignItemsToTestInfo();
        MessageTimer timer = new MessageTimer();
        public AsgnItemstoTestViewController()
        {
            InitializeComponent();
            TargetViewId = "TestMethod_ListView_Copy_ItemLink;" + "Items_ListView_Copy_TestMethodLink;";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Perform various tasks depending on the target View.
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

        private void PopupControl_CustomizePopupWindowSize(object sender, DevExpress.ExpressApp.Web.Controls.CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                if (e.PopupFrame.View.Id == "Items_ListView_Copy_TestMethodLink" || View.Id == "TestMethod_ListView_Copy_ItemLink")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(730);
                    e.Height = new System.Web.UI.WebControls.Unit(700);
                    e.Handled = true;
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
                if (View.Id == "TestMethod_ListView_Copy_ItemLink")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("TestItemLink", this);
                        gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    }
                }
                else if (View.Id == "Items_ListView_Copy_TestMethodLink")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gridView = gridListEditor.Grid;
                    if (gridView != null)
                    {
                        gridView.PreRender += GridView_PreRender;
                    }
                }
                // Access and customize the target View control.
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void GridView_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "Items_ListView_Copy_TestMethodLink")
                {
                    ASPxGridView grid = (ASPxGridView)sender;
                    if (grid != null && objAssignItemsToTestInfo.ItemsOid != null)
                    {
                        foreach (Guid obj in objAssignItemsToTestInfo.ItemsOid)
                        {
                            grid.Selection.SelectRowByKey(obj);
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

        private void Grid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                //if (e.DataColumn.FieldName != "Items" || e.DataColumn.FieldName != "LinkedItems") return;
                if (e.DataColumn.FieldName == "LinkedItems")
                {
                    e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'TestItemLink', {0}, '', false)", e.VisibleIndex));
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
            // base.OnDeactivated();
            try
            {
                // Unsubscribe from previously subscribed events and release other references and resources.
                base.OnDeactivated();
                ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
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
                if (View.Id == "TestMethod_ListView_Copy_ItemLink")
                {
                    //TestItemLink
                    //Items_ListView_Copy_TestMethodLink
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        object currentOid = editor.Grid.GetRowValues(int.Parse(parameter), "Oid");
                        TestMethod curTestMethod = View.ObjectSpace.GetObjectByKey<TestMethod>(currentOid);
                        if (curTestMethod != null)
                        {
                            objAssignItemsToTestInfo.CurrentTest = curTestMethod.Oid;
                            objAssignItemsToTestInfo.ItemsOid = curTestMethod.Item.Select(i => i.Oid).ToList();
                            IObjectSpace objspace = Application.CreateObjectSpace();
                            CollectionSource cs = new CollectionSource(objspace, typeof(Items));
                            ListView createListView = Application.CreateListView("Items_ListView_Copy_TestMethodLink", cs, false);
                            ShowViewParameters showViewParameters = new ShowViewParameters();
                            showViewParameters.CreatedView = createListView;
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            dc.Accepting += Dc_Accepting;
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

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (Application.MainWindow.View.Id == "TestMethod_ListView_Copy_ItemLink")
                {

                    if(e.AcceptActionArgs.SelectedObjects.Count > 0)
                    {
                    IObjectSpace os = Application.CreateObjectSpace();
                    TestMethod testMethod = os.GetObjectByKey<TestMethod>(objAssignItemsToTestInfo.CurrentTest);
                    if (testMethod != null)
                    {
                        List<Guid> lstSelItemsOid = e.AcceptActionArgs.SelectedObjects.Cast<Items>().Select(i => i.Oid).ToList();
                        List<Guid> lstRemovedItemsOid = objAssignItemsToTestInfo.ItemsOid.Except(lstSelItemsOid).ToList();
                        foreach (Guid oid in lstRemovedItemsOid)
                        {
                            Items obj = testMethod.Item.FirstOrDefault(i => i.Oid == oid);
                            if (obj != null)
                            {
                                testMethod.Item.Remove(obj);
                            }
                        }
                        foreach (Guid oid in lstSelItemsOid)
                        {
                            Items obj = testMethod.Item.FirstOrDefault(i => i.Oid == oid);
                            if (obj == null)
                            {
                                obj = os.GetObjectByKey<Items>(oid);
                                testMethod.Item.Add(obj);
                            }
                        }
                        os.CommitChanges();
                    }
                    View.ObjectSpace.Refresh();
                    if (testMethod.Item.Count > 1)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ItemLinkedToTestSuccessfully"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }

                        //else
                        //{
                        //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        //} 
                    
                    //else
                    //{
                    //    Application.ShowViewStrategy.ShowMessage("Select atleast one checkbox", InformationType.Warning, timer.Seconds, InformationPosition.Top);

                    //}
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
