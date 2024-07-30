using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.CCID;
using Modules.BusinessObjects.Setting.NCAID;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LDM.Module.Controllers.Settings.NCA
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class NCAViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        CorrectiveActionVerificationInfo objCAVInfo = new CorrectiveActionVerificationInfo();
        public NCAViewController()
        {
            InitializeComponent();
            TargetViewId = "NonConformityInitiation_ListView;" + "NonConformityInitiation_DetailView;" + "NonConformityInitiation_CorrectiveActionVerifications_ListView;"
                + "NonConformityInitiation_DetailView_PendingVerification;" + "CorrectiveActionLog_DetailView;" + "CompliantInitiation_DetailView;" + "CompliantInitiation_ListView;"
                + "CompliantInitiation_DetailView_Verification;" + "CompliantInitiation_CorrectiveActionLogs_ListView_PendingVerification;" + "CompliantInitiation_CorrectiveActionVerifications_ListView;"
                + "CorrectiveActionVerification_DetailView;" + "NonConformityInitiation_ListView_PendingVerification;" + "CompliantInitiation_ListView_Verification;" + "LabwareCertificate_DetailView;" + "Labware_Attachment_DetailView;"
                + "EDDReportGenerator_DetailView_popup;" + "NonConformityInitiation_DetailView_PendingVerification_History;" + "CompliantInitiation_DetailView_Verification_History;"
                + "NonConformityInitiation_CorrectiveActionVerifications_ListView_History;" + "ProblemCategory_DetailView;";
            NonconformitySubmitAction.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
            NonconformitySubmitAction.TargetViewId = "NonConformityInitiation_ListView;" + "NonConformityInitiation_DetailView;" + "CompliantInitiation_DetailView;" + "CompliantInitiation_ListView;";
            NonconformityCloseAction.TargetViewId = "NonConformityInitiation_DetailView_PendingVerification;" + "CompliantInitiation_DetailView_Verification;";
            History.TargetViewId = "CompliantInitiation_ListView_Verification;" + "NonConformityInitiation_ListView_PendingVerification;";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id == "NonConformityInitiation_CorrectiveActionVerifications_ListView" || View.Id == "CompliantInitiation_CorrectiveActionVerifications_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                    Frame.GetController<ListViewController>().EditAction.Executing += EditAction_Executing;
                }
                if (View.Id == "NonConformityInitiation_ListView" || View.Id == "CompliantInitiation_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Executed += SaveAction_Executed;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executed += SaveAction_Executed;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Executed += SaveAction_Executed;
                }
                if (View.Id == "NonConformityInitiation_DetailView_PendingVerification")
                {
                    NonconformityCloseAction.Active["hideClose"] = false;
                    NonConformityInitiation objNCI = View.CurrentObject as NonConformityInitiation;
                    Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                    if (currentUser != null && currentUser.UserName != "Administrator" && currentUser.UserName != "Service")
                    {
                        List<string> lstRoles = currentUser.RoleNames.Split(',').ToList();
                        foreach (string strrole in lstRoles)
                        {
                            RoleNavigationPermission objRNP = ObjectSpace.FindObject<RoleNavigationPermission>(CriteriaOperator.Parse("[RoleName] = ?", strrole));
                            NavigationItem objNI = ObjectSpace.FindObject<NavigationItem>(CriteriaOperator.Parse("[NavigationId] = 'CorrectiveActionVerificationNonconformity'"));
                            if (objNI != null)
                            {
                                foreach (RoleNavigationPermissionDetails objRNPD in objRNP.RoleNavigationPermissionDetails.Where(i => i.NavigationItem.NavigationId == objNI.NavigationId))
                                {
                                    if (objRNPD.Create == true && objRNPD.Write == true && objRNPD.Delete == true)
                                    {
                                        objNCI.IsPermission = true;
                                        NonconformityCloseAction.Active.RemoveItem("hideClose");
                                    }
                                }
                            }
                        }

                    }
                    else if (currentUser != null && currentUser.UserName != "Administrator" || currentUser.UserName != "Service")
                    {
                        objNCI.IsPermission = true;
                        NonconformityCloseAction.Active.RemoveItem("hideClose");
                    }
                    else
                    {
                        objNCI.IsPermission = false;
                    }
                    Frame.GetController<RefreshController>().RefreshAction.Executed += RefreshAction_Executed;
                    //DashboardViewItem vilstReports = ((DetailView)Application.MainWindow.View).FindItem("CorrectiveActionVerification") as DashboardViewItem;
                    //if (vilstReports != null)
                    //{
                    //    if (vilstReports.Control == null)
                    //    {
                    //        vilstReports.CreateControl();
                    //    }
                    //    ((Control)vilstReports.Control).Visible = false;
                    //}
                    //vilstReports.Dispose();
                }
                if (View.Id == "CompliantInitiation_DetailView_Verification")
                {
                    NonconformityCloseAction.Active["hideClose"] = false;
                    CompliantInitiation objNCI = View.CurrentObject as CompliantInitiation;
                    Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                    if (currentUser != null && currentUser.UserName != "Administrator" && currentUser.UserName != "Service")
                    {
                        List<string> lstRoles = currentUser.RoleNames.Split(',').ToList();
                        foreach (string strrole in lstRoles)
                        {
                            RoleNavigationPermission objRNP = ObjectSpace.FindObject<RoleNavigationPermission>(CriteriaOperator.Parse("[RoleName] = ?", strrole));
                            NavigationItem objNI = ObjectSpace.FindObject<NavigationItem>(CriteriaOperator.Parse("[NavigationId] = 'CorrectiveActionVerificationCompliant'"));
                            if (objNI != null)
                            {
                                foreach (RoleNavigationPermissionDetails objRNPD in objRNP.RoleNavigationPermissionDetails.Where(i => i.NavigationItem.NavigationId == objNI.NavigationId))
                                {
                                    if (objRNPD.Create == true && objRNPD.Write == true && objRNPD.Delete == true)
                                    {
                                        objNCI.IsPermission = true;
                                        NonconformityCloseAction.Active.RemoveItem("hideClose");
                                    }
                                }
                            }
                        }

                    }
                    else if (currentUser != null && currentUser.UserName != "Administrator" || currentUser.UserName != "Service")
                    {
                        objNCI.IsPermission = true;
                        NonconformityCloseAction.Active.RemoveItem("hideClose");
                    }
                    else
                    {
                        objNCI.IsPermission = false;
                    }
                    Frame.GetController<RefreshController>().RefreshAction.Executed += RefreshAction_Executed;
                    //DashboardViewItem vilstReports = ((DetailView)Application.MainWindow.View).FindItem("CorrectiveActionVerification") as DashboardViewItem;
                    //if (vilstReports != null)
                    //{
                    //    if (vilstReports.Control == null)
                    //    {
                    //        vilstReports.CreateControl();
                    //    }
                    //    ((Control)vilstReports.Control).Visible = false;
                    //}
                    //vilstReports.Dispose();
                }
                else if (View.Id == "CompliantInitiation_DetailView_Verification_History")
                {
                    DashboardViewItem lvCorrectiveActionVerification = ((DetailView)View).FindItem("CorrectiveActionVerification") as DashboardViewItem;
                    if (lvCorrectiveActionVerification != null)
                    {
                        lvCorrectiveActionVerification.ControlCreated += LvCorrectiveActionVerification_ControlCreated;
                    }
                }
                else if (View.Id == "NonConformityInitiation_CorrectiveActionVerifications_ListView_History")
                {
                    if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(NonConformityInitiation))
                    {
                        NonConformityInitiation objNonConformityInitiation = (NonConformityInitiation)Application.MainWindow.View.CurrentObject;
                        if (objNonConformityInitiation != null)
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[NonConformityInitiation]=?", objNonConformityInitiation.Oid);
                        }
                        else
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Oid is null");
                        }
                    }
                }
                 ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void LvCorrectiveActionVerification_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                CompliantInitiation objCompliant = (CompliantInitiation)View.CurrentObject;
                if (objCompliant != null)
                {
                    DashboardViewItem lvCorrectiveActionVerification = ((DetailView)View).FindItem("CorrectiveActionVerification") as DashboardViewItem;
                    if (lvCorrectiveActionVerification != null && lvCorrectiveActionVerification.InnerView != null)
                    {
                        ((ListView)lvCorrectiveActionVerification.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[CompliantInitiation]=?", objCompliant.Oid);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RefreshAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                NonConformityInitiation objNCI = View.CurrentObject as NonConformityInitiation;
                if (objNCI != null)
                {
                objNCI.IsPermission = true;

                }
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
                if (View != null && e.PopupFrame.View.Id == "CorrectiveActionLog_DetailView")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(509);
                    e.Height = new System.Web.UI.WebControls.Unit(310);
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "CorrectiveActionVerification_DetailView")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(486);
                    e.Height = new System.Web.UI.WebControls.Unit(394);
                    e.Handled = true;
                }
                //else if (View != null && e.PopupFrame.View.Id == "LabwareCertificate_DetailView")
                //{
                //    e.Width = new System.Web.UI.WebControls.Unit(830);
                //    e.Height = new System.Web.UI.WebControls.Unit(300);
                //    e.Handled = true;
                //}
                //else if (View != null && e.PopupFrame.View.Id == "Labware_Attachment_DetailView")
                //{
                //    e.Width = new System.Web.UI.WebControls.Unit(450);
                //    e.Height = new System.Web.UI.WebControls.Unit(320);
                //    e.Handled = true;
                //}
                //else if (View != null && e.PopupFrame.View.Id == "EDDReportGenerator_DetailView_popup")
                //{
                //    e.Width = new System.Web.UI.WebControls.Unit(800);
                //    e.Height = new System.Web.UI.WebControls.Unit(350);
                //    e.Handled = true;
                //}
                else if (View != null && e.PopupFrame.View.Id == "ProblemCategory_DetailView")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(800);
                    e.Height = new System.Web.UI.WebControls.Unit(200);
                    e.Handled = true;
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
            //ShowNavigationItemController ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
            //foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items.Where(i => i.Id == "InternalAudit"))
            //{
            //    {
            //        if (parent.Id == "InternalAudit")
            //        {
            //            ChoiceActionItem dataentryNode = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "InternalAudit");
            //            if (dataentryNode != null)
            //            {

            //                ChoiceActionItem child = dataentryNode.Items.FirstOrDefault(i => i.Id == "NonconformityCorrectiveAction");
            //                ChoiceActionItem Complaintchild = dataentryNode.Items.FirstOrDefault(i => i.Id == "ClientCompliant");
            //                if (child != null)
            //                {
            //                    ChoiceActionItem child1 = child.Items.FirstOrDefault(i => i.Id == "NonconformityInitiation");
            //                    if (child1 != null)
            //                    {
            //                        int count = 0;
            //                        var cap = child1.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
            //                        IObjectSpace objSpace = Application.CreateObjectSpace();
            //                        IList<NonConformityInitiation> lstTests = objSpace.GetObjects<NonConformityInitiation>(CriteriaOperator.Parse(""));
            //                        if (lstTests != null && lstTests.Count > 0)
            //                        {
            //                            //count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsSDMSTest).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
            //                            count = lstTests.Where(i => i.Status == NCAStatus.PendingSubmission).Count();
            //                        }
            //                        //var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
            //                        if (count > 0)
            //                        {
            //                            child1.Caption = cap[0] + " (" + count + ")";
            //                        }
            //                        else
            //                        {
            //                            child1.Caption = cap[0];
            //                        }
            //                    }



            //                }
            //                if (Complaintchild != null)
            //                {
            //                    ChoiceActionItem child1 = Complaintchild.Items.FirstOrDefault(i => i.Id == "CompliantInitiation");
            //                    //ChoiceActionItem child2 = child.Items.FirstOrDefault(i => i.Id == "CorrectiveActionVerificationCompliant");
            //                    if (child1 != null)
            //                    {
            //                        int count = 0;
            //                        var cap = child1.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
            //                        IObjectSpace objSpace = Application.CreateObjectSpace();
            //                        IList<CompliantInitiation> lstTests = objSpace.GetObjects<CompliantInitiation>(CriteriaOperator.Parse(""));
            //                        if (lstTests != null && lstTests.Count > 0)
            //                        {
            //                            //count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsSDMSTest).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
            //                            count = lstTests.Where(i => i.Status == CompliantInitiation.NCAStatus.PendingSubmission).Count();
            //                        }
            //                        //var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
            //                        if (count > 0)
            //                        {
            //                            child1.Caption = cap[0] + " (" + count + ")";
            //                        }
            //                        else
            //                        {
            //                            child1.Caption = cap[0];
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }

            //}
        }

        private void EditAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                CorrectiveActionVerification objView = objectSpace.GetObject((CorrectiveActionVerification)View.CurrentObject);
                DetailView dv = Application.CreateDetailView(objectSpace, "CorrectiveActionVerification_DetailView", true, objView);
                dv.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showViewParameters = new ShowViewParameters();
                showViewParameters.CreatedView = dv;
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void NewObjectAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                CorrectiveActionVerification objView = objectSpace.CreateObject<CorrectiveActionVerification>();
                DetailView dv = Application.CreateDetailView(objectSpace, "CorrectiveActionVerification_DetailView", true, objView);
                dv.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showViewParameters = new ShowViewParameters();
                showViewParameters.CreatedView = dv;
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
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
                if (View.Id == "NonConformityInitiation_CorrectiveActionVerifications_ListView")
                {
                    NonConformityInitiation objNCI = Application.MainWindow.View.CurrentObject as NonConformityInitiation;
                    ((ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("NonConformityInitiation", objNCI.Oid);
                    objCAVInfo.CorrectiveActionVerificationOid = objNCI.Oid;
                }
                else if (View.Id == "CompliantInitiation_CorrectiveActionVerifications_ListView")
                {
                    CompliantInitiation objNCI = Application.MainWindow.View.CurrentObject as CompliantInitiation;
                    ((ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("CompliantInitiation", objNCI.Oid);
                    objCAVInfo.CorrectiveActionVerificationOid = objNCI.Oid;
                }
                else if (View.Id == "NonConformityInitiation_DetailView" || View.Id == "CompliantInitiation_DetailView")
                {
                    foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor)))
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
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
            if (View.Id == "NonConformityInitiation_CorrectiveActionVerifications_ListView" || View.Id == "CompliantInitiation_CorrectiveActionVerifications_ListView")
            {
                Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                Frame.GetController<ListViewController>().EditAction.Executing -= EditAction_Executing;
            }
            if (View.Id == "NonConformityInitiation_ListView" || View.Id == "CompliantInitiation_DetailView")
            {
                Frame.GetController<ModificationsController>().SaveAction.Executed -= SaveAction_Executed;
                Frame.GetController<ModificationsController>().SaveAndCloseAction.Executed -= SaveAction_Executed;
                Frame.GetController<ModificationsController>().SaveAndNewAction.Executed -= SaveAction_Executed;
            }
            if (View.Id == "NonConformityInitiation_DetailView_PendingVerification")
            {
                Frame.GetController<RefreshController>().RefreshAction.Executed -= RefreshAction_Executed;
            }
            else if (View.Id == "CompliantInitiation_DetailView_Verification_History")
            {
                DashboardViewItem lvCorrectiveActionVerification = ((DetailView)View).FindItem("CorrectiveActionVerification") as DashboardViewItem;
                if (lvCorrectiveActionVerification != null)
                {
                    lvCorrectiveActionVerification.ControlCreated -= LvCorrectiveActionVerification_ControlCreated;
                }
            }
        }
        private void NonconformitySubmitAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                bool IsSubmit = false;
                if (View is ListView)
                {
                    if (View.Id == "NonConformityInitiation_ListView")
                    {
                        foreach (NonConformityInitiation objNCI in View.SelectedObjects)
                        {
                            objNCI.Status = NonConformityInitiation.NCAStatus.PendingVerification;
                            ObjectSpace.CommitChanges();
                            View.Refresh();
                            ObjectSpace.Refresh();
                            IsSubmit = true;
                            //((ListView)View).CollectionSource.ObjectSpace.Refresh();
                        }
                    }
                    else if (View.Id == "CompliantInitiation_ListView")
                    {
                        foreach (CompliantInitiation objNCI in View.SelectedObjects)
                        {
                            objNCI.Status = CompliantInitiation.NCAStatus.PendingVerification;
                            ObjectSpace.CommitChanges();
                            View.Refresh();
                            ObjectSpace.Refresh();
                            IsSubmit = true;
                            //((ListView)View).CollectionSource.ObjectSpace.Refresh();
                        }
                    }
                }
                else
                {
                    if (View.Id == "CompliantInitiation_DetailView")
                    {
                        CompliantInitiation objNCI = View.CurrentObject as CompliantInitiation;
                        if (objNCI.CCID != null)
                        {
                            objNCI.Status = CompliantInitiation.NCAStatus.PendingVerification;
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            ObjectSpace.CommitChanges();
                            View.Close();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage("Save the changes.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else if (View.Id == "NonConformityInitiation_DetailView")
                    {
                        NonConformityInitiation objNCII = View.CurrentObject as NonConformityInitiation;
                        if (objNCII.NCAID != null)
                        {
                            objNCII.Status = NonConformityInitiation.NCAStatus.PendingVerification;
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            ObjectSpace.CommitChanges();
                            View.Close();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage("Save the changes.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
                if (IsSubmit == true)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
                //ShowNavigationItemController ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                //foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items.Where(i => i.Id == "InternalAudit"))
                //{
                //    {
                //        if (parent.Id == "InternalAudit")
                //        {
                //            ChoiceActionItem dataentryNode = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "InternalAudit");
                //            if (dataentryNode != null)
                //            {

                //                ChoiceActionItem child = dataentryNode.Items.FirstOrDefault(i => i.Id == "NonconformityCorrectiveAction");
                //                ChoiceActionItem Complaintchild = dataentryNode.Items.FirstOrDefault(i => i.Id == "ClientCompliant");
                //                if (child != null)
                //                {
                //                    ChoiceActionItem child1 = child.Items.FirstOrDefault(i => i.Id == "NonconformityInitiation");
                //                    ChoiceActionItem child2 = child.Items.FirstOrDefault(i => i.Id == "CorrectiveActionVerificationNonconformity");
                //                    if (child1 != null)
                //                    {
                //                        int count = 0;
                //                        var cap = child1.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                //                        IObjectSpace objSpace = Application.CreateObjectSpace();
                //                        IList<NonConformityInitiation> lstTests = objSpace.GetObjects<NonConformityInitiation>(CriteriaOperator.Parse(""));
                //                        if (lstTests != null && lstTests.Count > 0)
                //                        {
                //                            //count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsSDMSTest).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                //                            count = lstTests.Where(i => i.Status == NCAStatus.PendingSubmission).Count();
                //                        }
                //                        //var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                //                        if (count > 0)
                //                        {
                //                            child1.Caption = cap[0] + " (" + count + ")";
                //                        }
                //                        else
                //                        {
                //                            child1.Caption = cap[0];
                //                        }
                //                    }
                //                    if (child2 != null)
                //                    {
                //                        int count = 0;
                //                        var cap = child2.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                //                        IObjectSpace objSpace = Application.CreateObjectSpace();
                //                        IList<NonConformityInitiation> lstTests = objSpace.GetObjects<NonConformityInitiation>(CriteriaOperator.Parse(""));
                //                        if (lstTests != null && lstTests.Count > 0)
                //                        {
                //                            //count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsSDMSTest).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                //                            count = lstTests.Where(i => i.Status == NCAStatus.PendingVerification).Count();
                //                        }
                //                        //var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                //                        if (count > 0)
                //                        {
                //                            child2.Caption = cap[0] + " (" + count + ")";
                //                        }
                //                        else
                //                        {
                //                            child2.Caption = cap[0];
                //                        }
                //                    }


                //                }
                //                if (Complaintchild != null)
                //                {
                //                    ChoiceActionItem child1 = Complaintchild.Items.FirstOrDefault(i => i.Id == "CompliantInitiation");
                //                    ChoiceActionItem child2 = Complaintchild.Items.FirstOrDefault(i => i.Id == "CorrectiveActionVerificationCompliant");
                //                    if (child1 != null)
                //                    {
                //                        int count = 0;
                //                        var cap = child1.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                //                        IObjectSpace objSpace = Application.CreateObjectSpace();
                //                        IList<CompliantInitiation> lstTests = objSpace.GetObjects<CompliantInitiation>(CriteriaOperator.Parse(""));
                //                        if (lstTests != null && lstTests.Count > 0)
                //                        {
                //                            //count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsSDMSTest).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                //                            count = lstTests.Where(i => i.Status == CompliantInitiation.NCAStatus.PendingSubmission).Count();
                //                        }
                //                        //var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                //                        if (count > 0)
                //                        {
                //                            child1.Caption = cap[0] + " (" + count + ")";
                //                        }
                //                        else
                //                        {
                //                            child1.Caption = cap[0];
                //                        }
                //                    }
                //                    if (child2 != null)
                //                    {
                //                        int count = 0;
                //                        var cap = child2.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                //                        IObjectSpace objSpace = Application.CreateObjectSpace();
                //                        IList<CompliantInitiation> lstTests = objSpace.GetObjects<CompliantInitiation>(CriteriaOperator.Parse(""));
                //                        if (lstTests != null && lstTests.Count > 0)
                //                        {
                //                            //count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsSDMSTest).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                //                            count = lstTests.Where(i => i.Status == CompliantInitiation.NCAStatus.PendingVerification).Count();
                //                        }
                //                        //var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                //                        if (count > 0)
                //                        {
                //                            child2.Caption = cap[0] + " (" + count + ")";
                //                        }
                //                        else
                //                        {
                //                            child2.Caption = cap[0];
                //                        }
                //                    }
                //                }
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
        private void NonconformityCloseAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "NonConformityInitiation_DetailView_PendingVerification")
                {
                    NonConformityInitiation objNCI = View.CurrentObject as NonConformityInitiation;
                    if (objNCI != null && objNCI.NCAID != null)
                    {
                        objNCI.Status = NonConformityInitiation.NCAStatus.Closed;
                        Application.ShowViewStrategy.ShowMessage("Closed successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                        ObjectSpace.CommitChanges();
                        View.Close();
                    }
                }
                else if (View.Id == "CompliantInitiation_DetailView_Verification")
                {
                    CompliantInitiation objNCI = View.CurrentObject as CompliantInitiation;
                    if (objNCI != null && objNCI.CCID != null)
                    {
                        objNCI.Status = CompliantInitiation.NCAStatus.Closed;
                        Application.ShowViewStrategy.ShowMessage("Closed successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                        ObjectSpace.CommitChanges();
                        View.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void History_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();

                if (View.Id == "CompliantInitiation_ListView_Verification")
                {
                    CollectionSource cs = new CollectionSource(objectSpace, typeof(CompliantInitiation));
                    //cs.Criteria["Filter"] = CriteriaOperator.Parse("[Status] > 2");
                    Frame.SetView(Application.CreateListView("CompliantInitiation_ListView_History", cs, true));
                }
                else
                if (View.Id == "NonConformityInitiation_ListView_PendingVerification")
                {
                    CollectionSource cs = new CollectionSource(objectSpace, typeof(NonConformityInitiation));
                    //cs.Criteria["Filter"] = CriteriaOperator.Parse("[Status] > 3");
                    Frame.SetView(Application.CreateListView("NonConformityInitiation_ListView_History", cs, true));
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
