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
using DevExpress.Persistent.Base;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Export;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Accounting.Receivables;
using Modules.BusinessObjects.Accounts;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Seting;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;

namespace XCRM.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ProspectsController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        ModificationsController mdcSave;
        public String strCurrentUserId = string.Empty;
        LeadInfo.ProspectsInfo prospectsInfo = new LeadInfo.ProspectsInfo();
        NavigationItemsInfo navigationInfo = new NavigationItemsInfo();
        DataTable dt;
        string strTopic = string.Empty;
        string strPotentialCustomer = string.Empty;
        string strClientCode = string.Empty;
        DateTime followUpDate;
        string strProductVersion = string.Empty;
        DateTime demoDate;
        string strCategory = string.Empty;
        DateTime quoteDate;
        int probablity = 0;
        string rating = string.Empty;
        string strOwner = string.Empty;
        string strSourceLead = string.Empty;
        string strStatus = string.Empty;
        string strMobilePhone = string.Empty;
        string strHomePhone = string.Empty;
        string strOtherPhone = string.Empty;
        string strEmail = string.Empty;
        string strFax = string.Empty;
        string strWebsite = string.Empty;
        string strCity = string.Empty;
        string strCountry = string.Empty;
        string strState = string.Empty;
        string strStrret1 = string.Empty;
        string strStreet2 = string.Empty;
        string strPreferredContactMethod = string.Empty;
        string strZip = string.Empty;
        string strOfficePhone = String.Empty;
        string strRegion = string.Empty;
        string strAccount = string.Empty;
        string strProspect = string.Empty;
        string strRemark = string.Empty;
        string strClientNumber = string.Empty;
        string strIndustry = string.Empty;
        bool CanDisableControls = false;
        NoteInfo objNote = new NoteInfo();
        string strFormName = string.Empty;
        string strprimaryContact = string.Empty;
        SimpleAction btncloseProspect;
        public ProspectsController()
        {
            InitializeComponent();
            TargetViewId = "CRMContact_DetailView_Copy_Opportunity;" + "Customer_DetailView;" + "CRMProspects_DetailView_Closed;" + "CRMProspects_DetailView;" + "Prospects_DetailView;" + "CloseCRMProspcts_Popup_DetailView_Copy;" + "CRMProspects_ListView_Copy_Closed;" + "CRMProspects_ListView;" + "CRMQuote_DetailView_CRMProspects;" + "CRMProspects_ListView_MyOpenLeads_Copy;";
            //popupImportFile.TargetViewId = "CRMOpportunity_ListView_Copy_Open";0
            popupImportFile.TargetViewId = "CRMProspects_ListView";
            // Target required Views (via the TargetXXX properties) and create their Actions.
            ReactivateOpportunity.TargetObjectsCriteria = "Status='" + ProspectsStatus.Cancelled +/* "' And Status='" + ProspectsStatus.Won +*/ "'";
            ReactivateOpportunity.TargetViewId = "CRMProspects_DetailView_Closed;";
            //ReactivateOpportunity.TargetObjectsCriteria = "[IsClose] = True";

            ReactivateOpportunity.Executed += ReactivateOpportunity_Executed;
            Rollback.TargetObjectsCriteria = "Status='" + ProspectsStatus.Won +/* "' And Status='" + ProspectsStatus.Won +*/ "'";
            Rollback.TargetViewId = "CRMProspects_DetailView_Closed";
            Rollback.Executed += Rollback_Executed;
            //closeProspects.TargetViewId = "CRMProspects_DetailView;";
            //closeProspects.Executing += CloseProspects_Executing;
            //closeProspects.TargetObjectsCriteria = "Status='" + ProspectsStatus.None + "'";
            //closeProspects.Executed += CloseProspects_Executed;

            btncloseProspect = new SimpleAction(this, "btncloseProspect", PredefinedCategory.RecordEdit)
            {
                Caption = "Close Prospects"
            };
            btncloseProspect.TargetViewId = "CRMProspects_DetailView;";
            btncloseProspect.TargetObjectsCriteria = "Status='" + ProspectsStatus.None + "'";
            btncloseProspect.Executing += BtncloseProspect_Executing; ;
            btncloseProspect.Execute += BtncloseProspect_Execute;

            btncloseProspect.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            //btncloseProspect.ImageName = "Action_New";


            //closeProspects.TargetViewId = "CRMProspects_DetailView;"; // + "CloseCRMProspects_Popup_DetailView_Copy;" + "Popup_CloseProspects_DetailView_Copy;" + "CRMProspects_ListView_MyOpenLeads_Copy;";

        }

        private void Rollback_Executed(object sender, ActionBaseEventArgs e)
        {

            try
            {
                IObjectSpace objspa = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspa, typeof(CRMProspects));
                cs.Criteria["filter"] = CriteriaOperator.Parse("[Status] <> 'None'");
                ListView lstprospect = Application.CreateListView("CRMProspects_ListView_Copy_Closed", cs, false);
                Frame.SetView(lstprospect);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void BtncloseProspect_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace(typeof(CRMProspects));
                CRMProspects objProspects = os.GetObject((CRMProspects)View.CurrentObject);
                if (objProspects == null)
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Saveandcontinue"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void BtncloseProspect_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace(typeof(CRMProspects));
                CRMProspects objprospect = (CRMProspects)View.CurrentObject;//os.FindObject<CRMProspects>(CriteriaOperator.Parse("[Oid] = ?",));
                objprospect.ActualRevenue = objprospect.Amount;
                DetailView dvprospect = Application.CreateDetailView(View.ObjectSpace, "CloseCRMProspects_popup_DetailView_Copy", false, objprospect);
                dvprospect.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                ShowViewParameters showViewParameters = new ShowViewParameters(dvprospect);
                showViewParameters.CreatedView = dvprospect;
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.CloseOnCurrentObjectProcessing = false;
                dc.Accepting += Dc_Acceptingprospect;
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_Acceptingprospect(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                CRMProspects objCRMProspects = (CRMProspects)View.CurrentObject;
                Customer objCustomer = ObjectSpace.GetObject<Customer>(objCRMProspects.Customer);
                if (objCRMProspects != null)
                {
                    //objCRMProspects.IsClose = true;
                    string strID;
                    if (objCRMProspects.Owner != null)
                    {
                        strID = objCRMProspects.Owner.Oid.ToString();
                    }
                    else
                    {
                        strID = strCurrentUserId;
                    }
                    Frame.GetController<EmailController>().CheckUserEmailPermission(strID, "Prospects Created", objCRMProspects, "");
                    if (objCRMProspects.Status == ProspectsStatus.Won)
                    {
                        Tuple<bool, string> SaveReponse = SaveProspects(objCRMProspects, objCustomer);
                        if (!SaveReponse.Item1)
                        {
                            e.Cancel = true;
                        }
                        else if (SaveReponse.Item1)
                        {

                        }
                        if (objCustomer != null && objCustomer.Account != null)
                        {

                        }
                    }
                    else
                    {
                        Tuple<bool, string> SaveReponse = SaveProspects(objCRMProspects, objCustomer);
                        if (!SaveReponse.Item1)
                        {
                            e.Cancel = true;
                        }
                        else if (SaveReponse.Item1)
                        {

                        }
                        if (objCustomer != null && objCustomer.Account != null)
                        {

                        }
                    }
                    if (objCRMProspects.PrimaryContact != null)
                    {
                        Contact objContact = objCRMProspects.Session.FindObject<Contact>(CriteriaOperator.Parse("[Oid]=?", objCRMProspects.PrimaryContact.Oid));
                        if (objContact != null)
                        {
                            objContact.Prospects = objCRMProspects;
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

        private void CloseProspects_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace(typeof(CRMProspects));
                CRMProspects objProspects = os.GetObject((CRMProspects)View.CurrentObject);
                if (objProspects == null)
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Saveandcontinue"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ReactivateOpportunity_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                IObjectSpace objspa = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspa, typeof(CRMProspects));
                cs.Criteria["filter"] = CriteriaOperator.Parse("[Status] <> 'None'");
                ListView lstprospect = Application.CreateListView("CRMProspects_ListView_Copy_Closed", cs, false);
                Frame.SetView(lstprospect);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }





        private void CloseProspects_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                IObjectSpace objspa = Application.CreateObjectSpace();
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages/LDMMessages", "ProspectCloseSuccess"), InformationType.Success, 3000, InformationPosition.Top);
                View.Refresh();
                CollectionSource cs = new CollectionSource(objspa, typeof(CRMProspects));
                cs.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 'None'");
                ListView lstprospect = Application.CreateListView("CRMProspects_ListView", cs, false);
                Frame.SetView(lstprospect);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                if (View.Id == "CRMProspects_DetailView" || View.Id == "CRMProspects_ListView_Copy_Open" || View.Id == "CRMProspects_ListView" || View.Id == "CRMProspects_ListView_MyOpenLeads_Copy")
                {
                    //this.closeProspects.Active.SetItemValue("ObjectType", false);
                    Employee currentUser = (Employee)SecuritySystem.CurrentUser;
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        //this.closeProspects.Active.SetItemValue("ObjectType", true);
                    }
                    else
                    {
                        if (navigationInfo.ClickedNavigationItem == "MyOpenProspects")
                        {
                            foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "MyOpenProspects" && i.Write == true) != null)
                                {
                                    btncloseProspect.Active["ObjectType"] = true;
                                }
                            }
                        }
                        else if (navigationInfo.ClickedNavigationItem == "OpenProspects")
                        {
                            foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "OpenProspects" && i.Write == true) != null)
                                {
                                    btncloseProspect.Active["ObjectType"] = true;
                                }
                            }
                        }
                        else if (navigationInfo.ClickedNavigationItem == "CRMProspects_ListView")
                        {
                            foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "CRMProspects_ListView" && i.Write == true) != null)
                                {
                                    btncloseProspect.Active["ObjectType"] = true;
                                }
                            }
                        }
                    }
                }
                mdcSave = Frame.GetController<ModificationsController>();
                mdcSave.SaveAction.Executing += SaveAction_Executing;
                mdcSave.SaveAndCloseAction.Executing += SaveAndCloseAction_Executing;
                mdcSave.SaveAndNewAction.Executing += SaveAndNewAction_Executing;
                //closeOpportunity.TargetObjectsCriteria = "Status=##XCRM.Module.BusinessObjects.LeadStatus,None#";
                ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                strCurrentUserId = Application.Security.UserId.ToString();


                //if (View.Id == "CRMQuote_DetailView_CRMProspects")
                //{
                //    Modules.BusinessObjects.Accounts.CRMQuote obj = (Modules.BusinessObjects.Accounts.CRMQuote)View.CurrentObject;
                //    if (obj != null && View.ObjectSpace.IsNewObject(obj) == true)
                //    {
                //        obj.PotentialCustomers = prospectsInfo.PotentialCustomer;
                //    }
                //}
                objNote.FormName = "Prospects";
                strFormName = objNote.FormName;

                if (Frame is DevExpress.ExpressApp.Web.PopupWindow)
                {
                    DevExpress.ExpressApp.Web.PopupWindow popupWindow = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                    if (popupWindow != null)
                    {
                        popupWindow.RefreshParentWindowOnCloseButtonClick = true;// This is for the cross (X) button of ASPxPopupControl.  
                        DialogController dc = popupWindow.GetController<DialogController>();
                        if (dc != null)
                        {
                            dc.Accepting += Dc_Accepting;
                            dc.SaveOnAccept = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Perform various tasks depending on the target View.

            //if(View!=null && View.CurrentObject != null)
            //{
            //    Opportunity closeOpportunity = (Opportunity)View.CurrentObject;
            //    closeOpportunity.Status = OpportunityStatus.None;
            //    closeOpportunity.CloseDate = DateTime.Now.Date;
            //}

            //var myOpenOpportunity = new ChoiceActionItem();
            //var OpenOpportunity = new ChoiceActionItem();
            //var ClosedOpportunity = new ChoiceActionItem();

            //opportunityFilterChoiceAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            //opportunityFilterChoiceAction.TargetViewType = ViewType.ListView;
            //opportunityFilterChoiceAction.Items.Add(new ChoiceActionItem("My Open Opportunity", myOpenOpportunity));
            //opportunityFilterChoiceAction.Items.Add(new ChoiceActionItem("Open Opportunity", OpenOpportunity));
            //opportunityFilterChoiceAction.Items.Add(new ChoiceActionItem("Closed Opportunity", ClosedOpportunity));
            //opportunityFilterChoiceAction.SelectedIndex = 2;
            //opportunityFilterChoiceAction.Category = "View";
            //opportunityFilterChoiceAction.TargetObjectType = typeof(CRMOpportunity);
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
                if (e.PopupFrame.View.Id == "CRMQuote_DetailView")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(400);
                    e.Height = new System.Web.UI.WebControls.Unit(1200);
                    e.Handled = true;
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
                if (View.Id != null && View.CurrentObject != null && View.Id == "Customer_DetailView")
                {
                    Customer objCustomer = (Customer)View.CurrentObject;
                    if (objCustomer != null)
                    {
                        if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(CRMProspects))
                        {
                            CRMProspects objOppotunity = (CRMProspects)Application.MainWindow.View.CurrentObject;
                            if (objOppotunity != null)
                            {
                                //objOppotunity.Street1 = objCustomer.BillToStreet1;
                                //objOppotunity.Street2 = objCustomer.BillToStreet2;
                                //objOppotunity.City = objCustomer.BillToCity;
                                //objOppotunity.State = objCustomer.BillToState;
                                //objOppotunity.Country = objCustomer.BillToCountry;
                                //objOppotunity.Zip = objCustomer.BillToZip;
                                if (objOppotunity.PrimaryContact == null)
                                {
                                    objOppotunity.OfficePhone = objCustomer.OfficePhone;
                                    objOppotunity.OtherPhone = objCustomer.OtherPhone;
                                    objOppotunity.MobilePhone = objCustomer.MobilePhone;
                                    objOppotunity.Email = objCustomer.Email;
                                    objOppotunity.Fax = objCustomer.Fax;
                                    objOppotunity.HomePhone = objCustomer.HomePhone;
                                    objOppotunity.WebSite = objCustomer.WebSite;
                                }
                            }
                        }
                    }
                }

                //if (View.Id != null && View.CurrentObject != null && View.Id == "CRMContact_DetailView_Copy_Opportunity")
                //{
                //    CRMContact objContact = (CRMContact)View.CurrentObject;
                //    CRMProspects objProspects = (CRMProspects)Application.MainWindow.View.CurrentObject;
                //    if (objContact != null)
                //    {
                //        if (objProspects != null)
                //        {
                //            objProspects.OfficePhone = objContact.OfficePhone;
                //            objProspects.OtherPhone = objContact.OtherPhone;
                //            objProspects.MobilePhone = objContact.MobilePhone;
                //            objProspects.Email = objContact.Email;
                //            objProspects.Fax = objContact.Fax;
                //            objProspects.HomePhone = objContact.HomePhone;
                //            objProspects.WebSite = objContact.WebSite;
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

        //private void Dc_ViewClosing(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if(View.Id !=null && View.CurrentObject !=null && View.Id == "Customer_DetailView")
        //        {
        //            Customer objCustomer = (Customer)View.CurrentObject;
        //            if (objCustomer != null)
        //            {
        //                CRMOpportunity objOppotunity = (CRMOpportunity)Application.MainWindow.View.CurrentObject;
        //                if (objOppotunity != null)
        //                {
        //                    objOppotunity.Street1 = objCustomer.BillToStreet1;
        //                    objOppotunity.Street2 = objCustomer.BillToStreet2;
        //                    objOppotunity.City = objCustomer.BillToCity;
        //                    objOppotunity.State = objCustomer.BillToState;
        //                    objOppotunity.Country = objCustomer.BillToCountry;
        //                    objOppotunity.Zip = objCustomer.BillToZip;

        //                    if (objOppotunity.PrimaryContact==null)
        //                    {
        //                        objOppotunity.OfficePhone = objCustomer.CustOfficePhone;
        //                        objOppotunity.OtherPhone = objCustomer.CustOtherPhone;
        //                        objOppotunity.MobilePhone = objCustomer.CustMobilePhone;
        //                        objOppotunity.Email = objCustomer.CustEmail;
        //                        objOppotunity.Fax = objCustomer.CustFax;
        //                        objOppotunity.HomePhone = objCustomer.CustHomePhone;
        //                        objOppotunity.WebSite = objCustomer.CustWebSite; 
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void SaveAndNewAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "CRMProspects_DetailView")
                {
                    CRMProspects prospects = ObjectSpace.GetObject((CRMProspects)View.CurrentObject);
                    Customer objCustomer = ObjectSpace.GetObject<Customer>(prospects.Customer);
                    if (prospects != null)
                    {
                        Tuple<bool, string> SaveReponse = SaveProspects(prospects, objCustomer);
                        if (!SaveReponse.Item1)
                        {
                            e.Cancel = true;
                        }
                    }
                    //Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.SetItemValue("Hide", false);
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
                bool bolFollow = false;
                if (View != null && View.CurrentObject != null && View.Id == "CRMProspects_DetailView")
                {
                    if (e.PropertyName == "FollowUpDate")
                    {
                        CRMProspects objAcc = (CRMProspects)View.CurrentObject;
                        if (objAcc.FollowUpDate != null && objAcc.FollowUpDate != DateTime.MinValue)
                        {
                            foreach (Notes objN in objAcc.Notes)
                            {
                                if (objN.FollowUpDate.ToString() == e.PropertyName.ToString())
                                {
                                    bolFollow = true;
                                }
                            }
                            if (bolFollow == false)
                            {
                                CRMActivity objActivity = ObjectSpace.FindObject<CRMActivity>(CriteriaOperator.Parse("ProspectsID='" + objAcc.Oid + "'"));
                                if (objActivity == null)
                                {
                                    CRMActivity objAct = ObjectSpace.CreateObject<CRMActivity>();
                                    string strdate = objAcc.FollowUpDate.ToShortDateString();
                                    string strStartdate = strdate + " " + "10:00 AM";
                                    string strEnddate = strdate + " " + "02:00 PM";
                                    objAct.ProspectsID = objAcc;
                                    //DateTime dt = DateTime.ParseExact(strdate, "MM/dd/yyyy hh:mm", System.Globalization.CultureInfo.InvariantCulture);
                                    Employee objUser = ObjectSpace.GetObjectByKey<Employee>(new Guid(strCurrentUserId));
                                    objAct.Owner = objUser;
                                    //objAct.Subject = "Follow Up Calls, " + objUser.UserName;
                                    objAct.Subject = "Follow Up Calls, " + objUser.FirstName;
                                    objAct.StartDateOn = Convert.ToDateTime(strStartdate);
                                    objAct.EndDateOn = Convert.ToDateTime(strEnddate);
                                    //objAct.Description = objUser.UserName + ", Please give a follow up call to " + objAcc.Name + " Today!";
                                    objAct.Description = objUser.FirstName + ", Please give a follow up call to " + objAcc.Name;
                                }
                                else
                                {
                                    // CRMActivity objAct = ObjectSpace.CreateObject<CRMActivity>();
                                    string strdate = objAcc.FollowUpDate.ToShortDateString();
                                    string strStartdate = strdate + " " + "10:00 AM";
                                    string strEnddate = strdate + " " + "02:00 PM";
                                    objActivity.ProspectsID = objAcc;
                                    //DateTime dt = DateTime.ParseExact(strdate, "MM/dd/yyyy hh:mm", System.Globalization.CultureInfo.InvariantCulture);
                                    Employee objUser = ObjectSpace.GetObjectByKey<Employee>(new Guid(strCurrentUserId));
                                    objActivity.Owner = objUser;
                                    //objActivity.Subject = "Follow Up Calls, " + objUser.UserName;
                                    objActivity.Subject = "Follow Up Calls, " + objUser.FirstName;
                                    objActivity.StartDateOn = Convert.ToDateTime(strStartdate);
                                    objActivity.EndDateOn = Convert.ToDateTime(strEnddate);
                                    objActivity.Description =
                                                   //objUser.UserName + ", Please give a follow up call to " + objAcc.Name + " Today!";
                                                   objUser.FirstName + ", Please give a follow up call to " + objAcc.Name;
                                }
                                ObjectSpace.CommitChanges();
                                //ObjectSpace.Refresh();
                            }
                        }
                    }
                    if (e.PropertyName == "PrimaryContact")
                    {
                        CRMProspects objOppotunity = (CRMProspects)View.CurrentObject;
                        if (objOppotunity != null && objOppotunity.PrimaryContact != null)
                        {
                            objOppotunity.OfficePhone = objOppotunity.PrimaryContact.OfficePhone;
                            objOppotunity.OtherPhone = objOppotunity.PrimaryContact.OtherPhone;
                            objOppotunity.MobilePhone = objOppotunity.PrimaryContact.MobilePhone;
                            objOppotunity.Email = objOppotunity.PrimaryContact.Email;
                            objOppotunity.Fax = objOppotunity.PrimaryContact.Fax;
                            objOppotunity.HomePhone = objOppotunity.PrimaryContact.HomePhone;
                            //objOppotunity.WebSite = objOppotunity.PrimaryContact.we;
                        }
                        else
                        {
                            objOppotunity.OfficePhone = string.Empty;
                            objOppotunity.OtherPhone = string.Empty;
                            objOppotunity.MobilePhone = string.Empty;
                            objOppotunity.Email = string.Empty;
                            objOppotunity.Fax = string.Empty;
                            objOppotunity.HomePhone = string.Empty;
                            objOppotunity.WebSite = string.Empty;
                        }
                    }
                    if (e.PropertyName == "PotentialCustomer")
                    {
                        CRMProspects objOppotunity = (CRMProspects)View.CurrentObject;
                        if (objOppotunity != null)
                        {
                            if (objOppotunity.PotentialCustomer != null)
                            {
                                //objOppotunity.Street1 = objOppotunity.PotentialCustomer.BillToStreet1;
                                //objOppotunity.Street2 = objOppotunity.PotentialCustomer.BillToStreet2;
                                //objOppotunity.City = objOppotunity.PotentialCustomer.BillToCity;
                                //objOppotunity.State = objOppotunity.PotentialCustomer.BillToState;
                                //objOppotunity.Country = objOppotunity.PotentialCustomer.BillToCountry;
                                //objOppotunity.Zip = objOppotunity.PotentialCustomer.BillToZip;

                                objOppotunity.OfficePhone = objOppotunity.PotentialCustomer.OfficePhone;
                                objOppotunity.OtherPhone = objOppotunity.PotentialCustomer.OtherPhone;
                                objOppotunity.MobilePhone = objOppotunity.PotentialCustomer.MobilePhone;
                                objOppotunity.Email = objOppotunity.PotentialCustomer.Email;
                                objOppotunity.Fax = objOppotunity.PotentialCustomer.Fax;
                                objOppotunity.HomePhone = objOppotunity.PotentialCustomer.HomePhone;
                                objOppotunity.WebSite = objOppotunity.PotentialCustomer.WebSite;
                                ASPxGridLookupPropertyEditor editor = ((DetailView)View).FindItem("PrimaryContact") as ASPxGridLookupPropertyEditor;
                                if (editor != null)
                                {
                                    //editor.CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Customer.Oid] = ?", objOppotunity.PotentialCustomer.Oid);
                                    //if (objOppotunity.SourceLead != null)
                                    //{
                                    //    editor.CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Customer.Oid] = ? Or [SourceLead.Oid]=?", objOppotunity.PotentialCustomer.Oid, objOppotunity.SourceLead.Oid);
                                    //    editor.RefreshDataSource();
                                    //    editor.Refresh();
                                    //}
                                    //else
                                    if (objOppotunity.PotentialCustomer != null)
                                    {
                                        editor.CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Customer.Oid] = ?", objOppotunity.PotentialCustomer.Oid);
                                        editor.RefreshDataSource();
                                        editor.Refresh();
                                    }
                                }
                            }
                            else
                            {

                                objOppotunity.Street1 = string.Empty;
                                objOppotunity.Street2 = string.Empty;
                                objOppotunity.City = null;
                                objOppotunity.State = null;
                                objOppotunity.Country = null;
                                objOppotunity.Zip = string.Empty;

                                objOppotunity.OfficePhone = string.Empty;
                                objOppotunity.OtherPhone = string.Empty;
                                objOppotunity.MobilePhone = string.Empty;
                                objOppotunity.Email = string.Empty;
                                objOppotunity.Fax = string.Empty;
                                objOppotunity.HomePhone = string.Empty;
                                objOppotunity.WebSite = string.Empty;
                            }
                            objOppotunity.PrimaryContact = null;
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

        private void SaveAndCloseAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "CRMProspects_DetailView")
                {
                    CRMProspects objProspects = ObjectSpace.GetObject((CRMProspects)View.CurrentObject);
                    Customer objCustomer = ObjectSpace.GetObject<Customer>(objProspects.Customer);
                    if (objProspects != null)
                    {
                        Tuple<bool, string> SaveReponse = SaveProspects(objProspects, objCustomer);
                        if (!SaveReponse.Item1)
                        {
                            e.Cancel = true;
                        }
                    }
                    //Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.SetItemValue("Hide", false);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private Tuple<bool, string> SaveProspects(CRMProspects objProspects, Customer objCustomer)
        {
            try
            {
                bool CanSave = false;
                string strResponeMessage = string.Empty;

                if (/*(objProspects.SourceLead != null && !string.IsNullOrEmpty(objProspects.SourceLead.CompanyName)) ||*/ objProspects.PrimaryContact != null || !string.IsNullOrEmpty(objProspects.Street1)
                    || !string.IsNullOrEmpty(objProspects.Street2) || objProspects.City != null || objProspects.State != null
                    || objProspects.Country != null || !string.IsNullOrEmpty(objProspects.Zip))
                {
                    IList<CRMProspects> lstOpportunities = ObjectSpace.GetObjects<CRMProspects>();
                    if (lstOpportunities != null && lstOpportunities.Count > 0)
                    {
                        CRMProspects valProspects = lstOpportunities.FirstOrDefault(i =>
                                                    //((i.SourceLead == null && objProspects.SourceLead == null) || (i.SourceLead != null && objProspects.SourceLead != null && ((string.IsNullOrEmpty(i.SourceLead.CompanyName) == true && string.IsNullOrEmpty(objProspects.SourceLead.CompanyName) == true) || (string.IsNullOrEmpty(i.SourceLead.CompanyName) == false &&
                                                    //string.IsNullOrEmpty(objProspects.SourceLead.CompanyName) == false && Regex.Replace(i.SourceLead.CompanyName.ToUpper(), @"\s", string.Empty) == Regex.Replace(objProspects.SourceLead.CompanyName.ToUpper(), @"\s", string.Empty))))) &&
                                                    ((i.PrimaryContact == null && objProspects.PrimaryContact == null) || (i.PrimaryContact != null && objProspects.PrimaryContact != null && Regex.Replace(i.PrimaryContact.FullName.ToUpper(), @"\s", string.Empty) == Regex.Replace(objProspects.PrimaryContact.FullName.ToUpper(), @"\s", string.Empty))) &&
                                                    ((string.IsNullOrEmpty(i.Street1) == true && string.IsNullOrEmpty(objProspects.Street1) == true) || (string.IsNullOrEmpty(i.Street1) == false && string.IsNullOrEmpty(objProspects.Street1) == false && Regex.Replace(i.Street1.ToUpper(), @"\s", string.Empty) == Regex.Replace(objProspects.Street1.ToUpper(), @"\s", string.Empty))) &&
                                                    ((string.IsNullOrEmpty(i.Street2) == true && string.IsNullOrEmpty(objProspects.Street2) == true) || (string.IsNullOrEmpty(i.Street2) == false && string.IsNullOrEmpty(objProspects.Street2) == false && Regex.Replace(i.Street2.ToUpper(), @"\s", string.Empty) == Regex.Replace(objProspects.Street2.ToUpper(), @"\s", string.Empty))) &&
                                                    ((i.City != null && string.IsNullOrEmpty(i.City.CityName) == true && objProspects.City != null && string.IsNullOrEmpty(objProspects.City.CityName) == true) || (i.City != null && string.IsNullOrEmpty(i.City.CityName) == false && objProspects.City != null && string.IsNullOrEmpty(objProspects.City.CityName) == false && Regex.Replace(i.City.CityName.ToUpper(), @"\s", string.Empty) == Regex.Replace(objProspects.City.CityName.ToUpper(), @"\s", string.Empty))) &&
                                                    ((i.State != null && string.IsNullOrEmpty(i.State.LongName) == true && objProspects.State != null && string.IsNullOrEmpty(objProspects.State.LongName) == true) || (i.State != null && string.IsNullOrEmpty(i.State.LongName) == false && objProspects.State != null && string.IsNullOrEmpty(objProspects.State.LongName) == false && Regex.Replace(i.State.LongName.ToUpper(), @"\s", string.Empty) == Regex.Replace(objProspects.State.LongName.ToUpper(), @"\s", string.Empty))) &&
                                                    ((i.Country != null && string.IsNullOrEmpty(i.Country.EnglishLongName) == true && objProspects.Country != null && string.IsNullOrEmpty(objProspects.Country.EnglishLongName) == true) || (i.Country != null && string.IsNullOrEmpty(i.Country.EnglishLongName) == false && objProspects.Country != null && string.IsNullOrEmpty(objProspects.Country.EnglishLongName) == false && Regex.Replace(i.Country.EnglishLongName.ToUpper(), @"\s", string.Empty) == Regex.Replace(objProspects.Country.EnglishLongName.ToUpper(), @"\s", string.Empty))) &&
                                                    ((string.IsNullOrEmpty(i.Zip) == true && string.IsNullOrEmpty(objProspects.Zip) == true) || (string.IsNullOrEmpty(i.Zip) == false && string.IsNullOrEmpty(objProspects.Zip) == false && Regex.Replace(i.Zip.ToUpper(), @"\s", string.Empty) == Regex.Replace(objProspects.Zip.ToUpper(), @"\s", string.Empty))) &&
                                                    i.Oid != objProspects.Oid
                                                    );
                        if (valProspects != null)
                        {
                            CanSave = false;
                        }
                        else
                        {
                            CanSave = true;
                        }
                    }
                    else
                    {
                        CanSave = true;
                    }
                }
                else
                {
                    CanSave = true;
                }

                if (CanSave)
                {
                    string strID;
                    if (objProspects.Owner != null)
                    {
                        strID = objProspects.Owner.Oid.ToString();
                    }
                    else
                    {
                        strID = strCurrentUserId;
                    }
                    if (View.ObjectSpace.IsNewObject(objProspects))
                    {
                        Frame.GetController<EmailController>().CheckUserEmailPermission(strID, "Prospects Created", objProspects, "");
                    }
                    if (objProspects.Status == ProspectsStatus.Won)
                    {
                        Customer findcustomer = ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("[CustomerName]='" + objProspects.Name + "'"));
                        //Customer findcustomer = ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("[CustomerName]='" + objProspects.Name + "' and [Address] ='" + objProspects.Street1 + "' and [Address1] ='" + objProspects.Street2 + "' and [City.CityName]='" + objProspects.City.CityName + "' and [State.LongName]='" + objProspects.State.LongName + "' and [Country.EnglishLongName]='" + objProspects.Country.EnglishLongName + "' and [Zip] ='" + objProspects.Zip + "'"));
                        if (findcustomer == null)
                        {
                            IObjectSpace accountObjectSpace = Application.CreateObjectSpace(typeof(Customer));
                            Customer account = ObjectSpace.CreateObject<Customer>();

                            if (objProspects != null)
                            {
                                account.CustomerName = objProspects.Prospects;
                                account.ClientNumber = objProspects.ClientNumber;
                                //account.SICCode = objProspects.SICCode;
                                //account.NumberOfEmployees = objProspects.NumberOfEmployees;
                                account.WebSite = objProspects.WebSite;
                                account.Address = objProspects.Street1;
                                account.Address1 = objProspects.Street2;
                                account.City = objProspects.City;
                                account.State = objProspects.State;
                                account.Country = objProspects.Country;
                                account.Zip = objProspects.Zip;
                                account.OfficePhone = objProspects.OfficePhone;
                                account.OtherPhone = objProspects.OtherPhone;
                                account.MobilePhone = objProspects.MobilePhone;
                                account.HomePhone = objProspects.HomePhone;
                                account.Email = objProspects.Email;
                                account.Fax = objProspects.Fax;
                                account.Owner = objProspects.Owner;
                                account.Industry = objProspects.Industry;
                                account.Account = objProspects.Account;
                                //account.PreferredContactMethod = objProspects.PreferredContactMethod;
                                //account.Prospect = objProspects;
                                //if (!string.IsNullOrEmpty(objProspects.Industry))
                                //{
                                //    IObjectSpace oskeyval = Application.CreateObjectSpace();
                                //    KeyValue objkeyval = oskeyval.FindObject<KeyValue>(CriteriaOperator.Parse("[Name] = ?", objProspects.Industry));
                                //    if(objkeyval != null)
                                //    {
                                //        account.Industry = ObjectSpace.GetObjectByKey<KeyValue>(objkeyval);
                                //    }
                                //    else
                                //    {
                                //        KeyValue crtkeyval = oskeyval.CreateObject<KeyValue>();
                                //        crtkeyval.Name = objProspects.Industry;
                                //        oskeyval.CommitChanges();
                                //        account.Industry = ObjectSpace.GetObjectByKey<KeyValue>(objkeyval);
                                //    }
                                //}

                                account.fCategory = objProspects.Category;

                                //account.ClientCode = objProspects.ClientCode;
                                account.PrimaryContact = objProspects.PrimaryContact;
                                if (objProspects.PrimaryContact != null)
                                {
                                    account.OfficePhone = objProspects.PrimaryContact.OfficePhone;
                                    account.OtherPhone = objProspects.PrimaryContact.OtherPhone;
                                    account.MobilePhone = objProspects.PrimaryContact.MobilePhone;
                                    account.HomePhone = objProspects.PrimaryContact.HomePhone;
                                    account.Email = objProspects.PrimaryContact.Email;
                                    account.Fax = objProspects.PrimaryContact.Fax;
                                }
                                Frame.GetController<EmailController>().CheckUserEmailPermission(strID, "New Account Notification", objProspects, "Prospects to Account");


                            }
                            ObjectSpace.CommitChanges();
                            ListPropertyEditor lvNotes = ((DetailView)Application.MainWindow.View).FindItem("Notes") as ListPropertyEditor;
                            if (lvNotes != null && lvNotes.ListView != null)
                            {
                                foreach (Notes objNots in ((ListView)lvNotes.ListView).CollectionSource.List.Cast<Notes>().ToList())
                                {
                                    objNots.Customer = lvNotes.ListView.ObjectSpace.GetObjectByKey<Customer>(account.Oid);
                                }
                                lvNotes.ListView.ObjectSpace.CommitChanges();
                            }
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ProspectCloseSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                        else
                        {
                            objProspects.Status = ProspectsStatus.None;
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CustomerNameUnique"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        }
                        if (objProspects.Prospects != null)
                        {
                            Contact findContact = null;
                            //Contact findContact = ObjectSpace.FindObject<Contact>(CriteriaOperator.Parse("[FirstName]='" + objProspects.PrimaryContact.FirstName + "'"));
                            if (objProspects.PrimaryContact == null)
                            {
                                findContact = ObjectSpace.FindObject<Contact>(CriteriaOperator.Parse("[FirstName]='" + objProspects.Prospects + "'"));
                            }
                            else
                            {
                                findContact = ObjectSpace.FindObject<Contact>(CriteriaOperator.Parse("[FirstName]='" + objProspects.PrimaryContact.FirstName + "'"));
                            }
                            if (findContact == null)
                            {
                                Contact contact = ObjectSpace.CreateObject<Contact>();
                                contact.FirstName = objProspects.Prospects;
                                contact.OfficePhone = objProspects.OfficePhone;
                                contact.MobilePhone = objProspects.MobilePhone;
                                contact.OtherPhone = objProspects.OtherPhone;
                                contact.HomePhone = objProspects.HomePhone;
                                contact.Email = objProspects.Email;
                                contact.Fax = objProspects.Fax;
                                contact.Street1 = objProspects.Street1;
                                contact.Street2 = objProspects.Street2;
                                contact.City = objProspects.City;
                                contact.State = objProspects.State;
                                contact.Country = objProspects.Country;
                                contact.Zip = objProspects.Zip;
                                //contact.ShipToStreet1 = objProspects.BillToStreet1;
                                //contact.ShipToStreet2 = objProspects.BillToStreet2;
                                //contact.ShipToCity = objProspects.BillToCity;
                                //contact.ShipToState = objProspects.BillToState;
                                //contact.ShipToCountry = objProspects.BillToCountry;
                                //contact.ShipToZip = objProspects.BillToZip;
                                //contact.Owner = objProspects.Owner;
                                //contact.Status = CustomerStatus.Active;
                                //contact.JobTitle = objProspects.Topic;
                                if (objProspects.Topic != null)
                                {
                                    contact.JobTitle = ObjectSpace.GetObject(objProspects.Topic.Topics);
                                }
                                //contact.PreferredContactMethod = objProspects.pre;
                                //contact.SourceLead = objCRMProspects.SourceLead;
                                Customer contactAccount = ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("[CustomerName]='" + objProspects.Name + "'"));
                                if (contactAccount != null)
                                {
                                    contact.Customer = contactAccount;
                                    contactAccount.PrimaryContact = contact;
                                }
                                ObjectSpace.CommitChanges();
                            }

                        }
                    }
                    else
                    {
                        CRMProspects cRMProspects = View.CurrentObject as CRMProspects;
                        if (cRMProspects.PrimaryContact != null)
                        {
                            View.ObjectSpace.ReloadObject(cRMProspects.PrimaryContact);
                            ObjectSpace.CommitChanges();

                        }
                        if (objProspects.Status == ProspectsStatus.Cancelled)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ProspectCloseSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                    }

                    if (objProspects.PrimaryContact != null)
                    {
                        Contact objContact = ObjectSpace.FindObject<Contact>(CriteriaOperator.Parse("[Oid]=?", objProspects.PrimaryContact.Oid));
                        if (objContact != null)
                        {
                            if (strFormName == "Prospects" && objContact.Prospects == null && objContact.Customer == null)
                            {
                                objContact.Prospects = objProspects;
                            }
                        }
                    }

                    //if (Opportunity.Status == OpportunityStatus.Won)
                    //{
                    //    disablecontrols(enablestate: false);
                    //    Frame.GetController<WebModificationsController>().EditAction.Active.SetItemValue("ShowOpportunityEditAction", false);
                    //}
                }
                else
                {
                    strResponeMessage = CaptionHelper.GetLocalizedText(@"Messages", "CannotSaveProspects");
                }
                return new Tuple<bool, string>(CanSave, strResponeMessage);
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                return new Tuple<bool, string>(false, ex.Message);
            }
        }

        private void SaveAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "CRMProspects_DetailView")
                {
                    CRMProspects objProspects = ObjectSpace.GetObject((CRMProspects)View.CurrentObject);
                    Customer objCustomer = ObjectSpace.GetObject<Customer>(objProspects.Customer);
                    if (objProspects != null)
                    {
                        Tuple<bool, string> SaveReponse = SaveProspects(objProspects, objCustomer);
                        if (!SaveReponse.Item1)
                        {
                            e.Cancel = true;
                        }
                    }
                    //Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.SetItemValue("Hide", false);
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
                Boolean bolNotes = false;
                //Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.SetItemValue("Hide", true);
                if (View != null && View.ObjectTypeInfo.Type == typeof(CRMProspects) && View is DetailView)
                {
                    if (View.CurrentObject != null)
                    {
                        CRMProspects cRMProspects = (CRMProspects)View.CurrentObject;
                        if (cRMProspects != null && cRMProspects.Status == ProspectsStatus.None)
                        {
                            Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.SetItemValue("prosdelete", true);
                        }
                        else if (cRMProspects != null && cRMProspects.Status != ProspectsStatus.None)
                        {
                            Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.SetItemValue("prosdelete", false);
                        }
                    }
                }
                if (View != null && View.CurrentObject != null && View.Id == "CRMProspects_DetailView")
                {
                    CRMProspects objAcc = (CRMProspects)View.CurrentObject;
                    if (objAcc != null)
                    {
                        if (objAcc.PotentialCustomer != null)
                        {
                            prospectsInfo.PotentialCustomer = objAcc.PotentialCustomer.ToString();

                            if (objAcc.PotentialCustomer != null)
                            {
                                objNote.Customer = objAcc.PotentialCustomer.ToString();
                                ASPxGridLookupPropertyEditor editor = ((DetailView)View).FindItem("PrimaryContact") as ASPxGridLookupPropertyEditor;
                                if (editor != null && editor.CollectionSource != null)
                                {
                                    //if (objAcc.SourceLead != null)
                                    //{
                                    //    editor.CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Customer.Oid] = ? Or [SourceLead.Oid]=?", objAcc.PotentialCustomer.Oid, objAcc.SourceLead.Oid);
                                    //    editor.RefreshDataSource();
                                    //    editor.Refresh();
                                    //}
                                    //else
                                    if (objAcc.PotentialCustomer != null)
                                    {
                                        editor.CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Customer.Oid] = ?", objAcc.PotentialCustomer.Oid);
                                        editor.RefreshDataSource();
                                        editor.Refresh();
                                    }
                                }
                            }
                            else
                            {
                                objNote.Customer = string.Empty;
                            }

                        }
                        //  


                        //ASPxGridLookupPropertyEditor editor = ((DetailView)View).FindItem("PrimaryContact") as ASPxGridLookupPropertyEditor;
                        //if (editor != null)
                        //{
                        //    editor.CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Customer.Oid] = ?", objAcc.Oid);
                        //    editor.RefreshDataSource();
                        //    editor.Refresh();
                        //}

                        //if (CanDisableControls && objAcc.Status == OpportunityStatus.Won)
                        //{
                        //    disablecontrols(enablestate: false);
                        //    Frame.GetController<WebModificationsController>().EditAction.Active.SetItemValue("ShowOpportunityEditAction", false);
                        //    CanDisableControls = false;
                        //}
                        //else
                        //{
                        //    disablecontrols(enablestate: true);
                        //    CanDisableControls = false;
                        //}
                    }

                    foreach (Notes objN in objAcc.Notes)
                    {
                        bolNotes = true;
                        break;
                    }
                    if (bolNotes == true)
                    {
                        DateTime? smallest = objAcc.Notes.Min(a => a.FollowUpDate);
                        objAcc.FollowUpDate = (DateTime)smallest;
                    }
                }
                //if (View != null && View.CurrentObject != null && View.Id == "CRMContact_DetailView_Copy_Prospects")
                //{
                //    // IObjectSpace objectSpace = Application.MainWindow.View.ObjectSpace ;
                //    CRMProspects prospects = (CRMProspects)Application.MainWindow.View.CurrentObject;
                //    if (prospects != null)
                //    {
                //        CRMContact objContact = (CRMContact)View.CurrentObject;
                //        if (objContact != null)
                //        {
                //            objContact.Customer = ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("Oid=?", prospects.Customer.Oid));
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

        private void Customer_Save(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void disablecontrols(bool enablestate)
        //{
        //    try
        //    {
        //        foreach (ViewItem item in ((DetailView)View).Items)
        //        {
        //            if (item.GetType() == typeof(ASPxStringPropertyEditor))
        //            {
        //                ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
        //                if (propertyEditor != null && propertyEditor.Editor != null)
        //                {
        //                    propertyEditor.AllowEdit.SetItemValue("enbstate", enablestate);
        //                }
        //            }
        //            else if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
        //            {
        //                ASPxCheckedLookupStringPropertyEditor propertyEditor = item as ASPxCheckedLookupStringPropertyEditor;
        //                if (propertyEditor != null && propertyEditor.Editor != null)
        //                {
        //                    propertyEditor.AllowEdit.SetItemValue("enbstate", enablestate);
        //                }
        //            }
        //            else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
        //            {
        //                ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
        //                if (propertyEditor != null && propertyEditor.Editor != null)
        //                {
        //                    propertyEditor.AllowEdit.SetItemValue("enbstate", enablestate);
        //                }
        //            }
        //            else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
        //            {
        //                ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
        //                if (propertyEditor != null && propertyEditor.Editor != null)
        //                {
        //                    propertyEditor.AllowEdit.SetItemValue("enbstate", enablestate);
        //                }
        //            }
        //            else if (item.GetType() == typeof(FileDataPropertyEditor))
        //            {
        //                FileDataPropertyEditor propertyEditor = item as FileDataPropertyEditor;
        //                if (propertyEditor != null && propertyEditor.Editor != null)
        //                {
        //                    propertyEditor.AllowEdit.SetItemValue("enbstate", enablestate);
        //                }
        //            }
        //            else if (item.GetType() == typeof(ASPxEnumPropertyEditor))
        //            {
        //                ASPxEnumPropertyEditor propertyEditor = item as ASPxEnumPropertyEditor;
        //                if (propertyEditor != null && propertyEditor.Editor != null)
        //                {
        //                    propertyEditor.AllowEdit.SetItemValue("enbstate", enablestate);
        //                }
        //            }
        //            else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
        //            {
        //                ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
        //                if (propertyEditor != null && propertyEditor.Editor != null)
        //                {
        //                    propertyEditor.AllowEdit.SetItemValue("enbstate", enablestate);
        //                }
        //            }
        //            else if (item.GetType() == typeof(ASPxIntPropertyEditor))
        //            {
        //                ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
        //                if (propertyEditor != null && propertyEditor.Editor != null)
        //                {
        //                    propertyEditor.AllowEdit.SetItemValue("enbstate", enablestate);
        //                }
        //            }
        //            else if (item.GetType() == typeof(ListPropertyEditor))
        //            {
        //                ListPropertyEditor propertyEditor = item as ListPropertyEditor;
        //                if (propertyEditor != null && propertyEditor.ListView != null && propertyEditor.ListView.Editor != null)
        //                {
        //                    propertyEditor.AllowEdit.SetItemValue("enbstate", enablestate);
        //                    propertyEditor.ListView.AllowEdit.SetItemValue("enbstate", enablestate);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            try
            {
                base.OnDeactivated();
                if (View != null && View.ObjectTypeInfo.Type == typeof(CRMProspects) && View is DetailView)
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.SetItemValue("prosdelete", true);
                }
                ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
                objNote.FormName = string.Empty;
                mdcSave = Frame.GetController<ModificationsController>();
                mdcSave.SaveAction.Executing -= SaveAction_Executing;
                mdcSave.SaveAndCloseAction.Executing -= SaveAndCloseAction_Executing;
                mdcSave.SaveAndNewAction.Executing -= SaveAndNewAction_Executing;
                //ShowOpportunityEditAction
                if (View.Id == "CRMProspects_DetailView" && Frame.GetController<WebModificationsController>().EditAction.Active.Contains("ShowOpportunityEditAction"))
                {
                    Frame.GetController<WebModificationsController>().EditAction.Active.RemoveItem("ShowOpportunityEditAction");
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CloseProspects_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace(typeof(CRMProspects));
                CRMProspects objProspects = os.GetObject((CRMProspects)View.CurrentObject);
                if (objProspects != null)
                {
                    objProspects.ActualRevenue = objProspects.Amount;
                    //obj.Status = OpportunityStatus.Won;
                    e.View = Application.CreateDetailView(os, "CloseCRMProspects_popup_DetailView_Copy", true, objProspects);
                    ((DetailView)e.View).ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void popupImportFile_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace sheetObjectSpace = Application.CreateObjectSpace(typeof(ItemsFileUpload));
                ItemsFileUpload spreadSheet = (ItemsFileUpload)sheetObjectSpace.CreateObject<ItemsFileUpload>();
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

        private void popupImportFile_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                ResourceManager rmEnglish = new ResourceManager("Resources.LocalizationProspectsEnglish", Assembly.Load("App_GlobalResources"));
                ResourceManager rmChinese = new ResourceManager("Resources.LocalizationProspectsChinesh", Assembly.Load("App_GlobalResources"));
                ItemsFileUpload itemsFile = (ItemsFileUpload)e.PopupWindowViewCurrentObject;
                if (itemsFile.InputFile != null)
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
                    WorksheetCollection worksheets = workbook.Worksheets;
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

                    foreach (DataRow row in dt.Rows)
                    {
                        var isEmpty = row.ItemArray.All(c => c is DBNull);
                        if (!isEmpty)
                        {
                            List<string> errorlist = new List<string>();

                            if (dt.Columns.Contains(rmChinese.GetString("SourceLead")) && !row.IsNull(rmChinese.GetString("SourceLead")))
                            {
                                strSourceLead = row[rmChinese.GetString("SourceLead")].ToString();

                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("SourceLead")) && !row.IsNull(rmEnglish.GetString("SourceLead")))
                            {
                                strSourceLead = row[rmEnglish.GetString("SourceLead")].ToString();

                            }
                            else
                            {
                                strSourceLead = string.Empty;
                            }

                            if (dt.Columns.Contains(rmChinese.GetString("PrimaryContact")) && !row.IsNull(rmChinese.GetString("PrimaryContact")))
                            {
                                strprimaryContact = row[rmChinese.GetString("PrimaryContact")].ToString();

                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("PrimaryContact")))
                            {
                                strprimaryContact = row[rmEnglish.GetString("PrimaryContact")].ToString();

                            }
                            else
                            {
                                strprimaryContact = string.Empty;
                            }

                            if (dt.Columns.Contains(rmChinese.GetString("City")) && !row.IsNull(rmChinese.GetString("City")))
                            {
                                strCity = row[rmChinese.GetString("City")].ToString();

                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("City")) && !row.IsNull(rmEnglish.GetString("City")))
                            {
                                strCity = row[rmEnglish.GetString("City")].ToString();

                            }
                            else
                            {
                                strCity = string.Empty;
                            }
                            if (dt.Columns.Contains(rmChinese.GetString("State")) && !row.IsNull(rmChinese.GetString("State")))
                            {
                                strState = row[rmChinese.GetString("State")].ToString();

                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("State")) && !row.IsNull(rmEnglish.GetString("State")))
                            {
                                strState = row[rmEnglish.GetString("State")].ToString();

                            }
                            else
                            {
                                strState = string.Empty;
                            }
                            if (dt.Columns.Contains(rmChinese.GetString("Country")) && !row.IsNull(rmChinese.GetString("Country")))
                            {
                                strCountry = row[rmChinese.GetString("Country")].ToString();

                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("Country")) && !row.IsNull(rmEnglish.GetString("Country")))
                            {
                                strCountry = row[rmEnglish.GetString("Country")].ToString();

                            }
                            else
                            {
                                strCountry = string.Empty;
                            }
                            if (dt.Columns.Contains(rmChinese.GetString("Zip")) && !row.IsNull(rmChinese.GetString("Zip")))
                            {
                                strZip = row[rmChinese.GetString("Zip")].ToString();

                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("Zip")) && !row.IsNull(rmEnglish.GetString("Zip")))
                            {
                                strZip = row[rmEnglish.GetString("Zip")].ToString();

                            }
                            else
                            {
                                strZip = string.Empty;
                            }
                            if (dt.Columns.Contains(rmChinese.GetString("Street1")) && !row.IsNull(rmChinese.GetString("Street1")))
                            {
                                strStrret1 = row[rmChinese.GetString("Street1")].ToString();

                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("Street1")) && !row.IsNull(rmEnglish.GetString("Street1")))
                            {
                                strStrret1 = row[rmEnglish.GetString("Street1")].ToString();

                            }
                            else
                            {
                                strStrret1 = string.Empty;
                            }
                            if (dt.Columns.Contains(rmChinese.GetString("Street2")) && !row.IsNull(rmChinese.GetString("Street2")))
                            {
                                strStreet2 = row[rmChinese.GetString("Street2")].ToString();

                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("Street2")) && !row.IsNull(rmEnglish.GetString("Street2")))
                            {
                                strStreet2 = row[rmEnglish.GetString("Street2")].ToString();

                            }
                            else
                            {
                                strStreet2 = string.Empty;
                            }
                            if (!string.IsNullOrEmpty(strSourceLead) || (!string.IsNullOrEmpty(strprimaryContact) && !string.IsNullOrEmpty(strStrret1)) || !string.IsNullOrEmpty(strStreet2)
                    || !string.IsNullOrEmpty(strCity) || !string.IsNullOrEmpty(strState)
                    || !string.IsNullOrEmpty(strCountry) || !string.IsNullOrEmpty(strZip))
                            {
                                CRMProspects objCRMProspects = null;
                                IList<CRMProspects> lstProspects = ObjectSpace.GetObjects<CRMProspects>();
                                if (lstProspects != null && lstProspects.Count > 0)
                                {
                                    objCRMProspects = lstProspects.FirstOrDefault(i =>
                                                     ((i.PrimaryContact == null && string.IsNullOrEmpty(strprimaryContact) == true) || (i.PrimaryContact != null && string.IsNullOrEmpty(strprimaryContact) == false && Regex.Replace(i.PrimaryContact.FullName.ToUpper(), @"\s", string.Empty) == Regex.Replace(strprimaryContact.ToUpper(), @"\s", string.Empty))) &&
                                                    ((string.IsNullOrEmpty(i.Street1) == true && string.IsNullOrEmpty(strStrret1) == true) || (string.IsNullOrEmpty(i.Street1) == false && string.IsNullOrEmpty(strStrret1) == false && Regex.Replace(i.Street1.ToUpper(), @"\s", string.Empty) == Regex.Replace(strStrret1.ToUpper(), @"\s", string.Empty))) &&
                                                    ((string.IsNullOrEmpty(i.Street2) == true && string.IsNullOrEmpty(strStreet2) == true) || (string.IsNullOrEmpty(i.Street2) == false && string.IsNullOrEmpty(strStreet2) == false && Regex.Replace(i.Street2.ToUpper(), @"\s", string.Empty) == Regex.Replace(strStreet2.ToUpper(), @"\s", string.Empty))) &&

                                                    //((i.City != null && string.IsNullOrEmpty(i.City.CityName) == true && objProspects.City != null && string.IsNullOrEmpty(objProspects.City.CityName) == true) || (i.City != null && string.IsNullOrEmpty(i.City.CityName) == false && objProspects.City != null && string.IsNullOrEmpty(objProspects.City.CityName) == false && Regex.Replace(i.City.CityName.ToUpper(), @"\s", string.Empty) == Regex.Replace(objProspects.City.CityName.ToUpper(), @"\s", string.Empty))) &&
                                                    //((i.State != null && string.IsNullOrEmpty(i.State.LongName) == true && objProspects.State != null && string.IsNullOrEmpty(objProspects.State.LongName) == true) || (i.State != null && string.IsNullOrEmpty(i.State.LongName) == false && objProspects.State != null && string.IsNullOrEmpty(objProspects.State.LongName) == false && Regex.Replace(i.State.LongName.ToUpper(), @"\s", string.Empty) == Regex.Replace(objProspects.State.LongName.ToUpper(), @"\s", string.Empty))) &&
                                                    //((i.Country != null && string.IsNullOrEmpty(i.Country.EnglishLongName) == true && objProspects.Country != null && string.IsNullOrEmpty(objProspects.Country.EnglishLongName) == true) || (i.Country != null && string.IsNullOrEmpty(i.Country.EnglishLongName) == false && objProspects.Country != null && string.IsNullOrEmpty(objProspects.Country.EnglishLongName) == false && Regex.Replace(i.Country.EnglishLongName.ToUpper(), @"\s", string.Empty) == Regex.Replace(objProspects.Country.EnglishLongName.ToUpper(), @"\s", string.Empty))) &&

                                                    ((i.City != null && string.IsNullOrEmpty(i.City.CityName) == true && string.IsNullOrEmpty(strCity) == true) || (i.City != null && string.IsNullOrEmpty(i.City.CityName) == false && string.IsNullOrEmpty(strCity) == false && Regex.Replace(i.City.CityName.ToUpper(), @"\s", string.Empty) == Regex.Replace(strCity.ToUpper(), @"\s", string.Empty))) &&
                                                    ((i.State != null && string.IsNullOrEmpty(i.State.LongName) == true && string.IsNullOrEmpty(strState) == true) || (i.State != null && string.IsNullOrEmpty(i.State.LongName) == false && string.IsNullOrEmpty(strState) == false && Regex.Replace(i.State.LongName.ToUpper(), @"\s", string.Empty) == Regex.Replace(strState.ToUpper(), @"\s", string.Empty))) &&
                                                    ((i.Country != null && string.IsNullOrEmpty(i.Country.EnglishLongName) == true && string.IsNullOrEmpty(strCountry) == true) || (i.Country != null && string.IsNullOrEmpty(i.Country.EnglishLongName) == false && string.IsNullOrEmpty(strCountry) == false && Regex.Replace(i.Country.EnglishLongName.ToUpper(), @"\s", string.Empty) == Regex.Replace(strCountry.ToUpper(), @"\s", string.Empty))) &&
                                                    ((string.IsNullOrEmpty(i.Zip) == true && string.IsNullOrEmpty(strZip) == true) || (string.IsNullOrEmpty(i.Zip) == false && string.IsNullOrEmpty(strZip) == false && Regex.Replace(i.Zip.ToUpper(), @"\s", string.Empty) == Regex.Replace(strZip.ToUpper(), @"\s", string.Empty)))
                                                    );
                                }

                                if (objCRMProspects == null)
                                {
                                    objCRMProspects = ObjectSpace.CreateObject<CRMProspects>();
                                    if (dt.Columns.Contains(rmChinese.GetString("Topic")) && !row.IsNull(rmChinese.GetString("Topic")))
                                    {
                                        strTopic = row[rmChinese.GetString("Topic")].ToString();

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Topic")) && !row.IsNull(rmEnglish.GetString("Topic")))
                                    {
                                        strTopic = row[rmEnglish.GetString("Topic")].ToString();

                                    }
                                    else
                                    {
                                        strTopic = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("PotentialCustomer")) && !row.IsNull(rmChinese.GetString("PotentialCustomer")))
                                    {
                                        strPotentialCustomer = row[rmChinese.GetString("PotentialCustomer")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("PotentialCustomer")) && !row.IsNull(rmEnglish.GetString("PotentialCustomer")))
                                    {
                                        strPotentialCustomer = row[rmEnglish.GetString("PotentialCustomer")].ToString();
                                    }
                                    else
                                    {
                                        strPotentialCustomer = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("ClientCode")) && !row.IsNull(rmChinese.GetString("ClientCode")))
                                    {
                                        strClientCode = row[rmChinese.GetString("ClientCode")].ToString();

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("ClientCode")) && !row.IsNull(rmEnglish.GetString("ClientCode")))
                                    {
                                        strClientCode = row[rmEnglish.GetString("ClientCode")].ToString();

                                    }
                                    else
                                    {
                                        strClientCode = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("FollowUpDate")) && !row.IsNull(rmChinese.GetString("FollowUpDate")))
                                    {
                                        if (row[rmChinese.GetString("FollowUpDate")].GetType() == typeof(DateTime))
                                        {
                                            followUpDate = Convert.ToDateTime(row[rmChinese.GetString("FollowUpDate")]);
                                        }
                                        else if (row[rmChinese.GetString("FollowUpDate")].GetType() == typeof(string))
                                        {
                                            string strFollowUpDate = row[rmChinese.GetString("FollowUpDate")].ToString();
                                            if (strFollowUpDate.Contains("/"))
                                            {
                                                string strdate = row[rmChinese.GetString("FollowUpDate")].ToString();
                                                if (strdate != string.Empty)
                                                {
                                                    followUpDate = DateTime.ParseExact(strdate, "M/dd/yyyy", null);
                                                }
                                                //string[] arrFollowUpDate = strFollowUpDate.Split('/');
                                                //if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                //{
                                                //    if (arrFollowUpDate[0].Length <= 2)
                                                //    {
                                                //        //followUpDate = Convert.ToDateTime(strFollowUpDate);
                                                //        followUpDate = DateTime.ParseExact(arrFollowUpDate[0], "MM/dd/yyyy", null);
                                                //    }
                                                //    else
                                                //    {
                                                //        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                //        followUpDate = date;
                                                //    }
                                                //}
                                            }
                                            else if (strFollowUpDate.Contains("-"))
                                            {
                                                string[] arrFollowUpDate = strFollowUpDate.Split('-');
                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                {
                                                    if (arrFollowUpDate[0].Length <= 2)
                                                    {
                                                        followUpDate = Convert.ToDateTime(strFollowUpDate);
                                                    }
                                                    else
                                                    {
                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                        followUpDate = date;
                                                    }
                                                }
                                            }
                                            else if (strFollowUpDate.Contains("."))
                                            {
                                                string[] arrFollowUpDate = strFollowUpDate.Split('.');
                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                {
                                                    if (arrFollowUpDate[0].Length <= 2)
                                                    {
                                                        followUpDate = Convert.ToDateTime(strFollowUpDate);
                                                    }
                                                    else
                                                    {
                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                        followUpDate = date;
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("FollowUpDate")) && !row.IsNull(rmEnglish.GetString("FollowUpDate")))
                                    {
                                        if (row[rmEnglish.GetString("FollowUpDate")].GetType() == typeof(DateTime))
                                        {
                                            followUpDate = Convert.ToDateTime(row[rmEnglish.GetString("FollowUpDate")]);
                                        }
                                        else if (row[rmEnglish.GetString("FollowUpDate")].GetType() == typeof(string))
                                        {
                                            string strFollowUpDate = row[rmEnglish.GetString("FollowUpDate")].ToString();
                                            if (strFollowUpDate.Contains("/"))
                                            {
                                                //string Format = "yyyy-MM-dd HH:mm:ss";
                                                //CultureInfo provider = new CultureInfo("en-US");
                                                //string strdate = row[rmEnglish.GetString("FollowUpDate")].ToString();
                                                if (strFollowUpDate != string.Empty)
                                                {
                                                    //followUpDate = DateTime.ParseExact(strdate, "M/dd/yyyy", null);
                                                    followUpDate = DateTime.ParseExact(strFollowUpDate.Trim(), "M/d/yyyy", null);
                                                    //followUpDate = DateTime.ParseExact(strFollowUpDate, Format, provider, DateTimeStyles.None);
                                                }
                                                //string[] arrFollowUpDate = strFollowUpDate.Split('/');
                                                //if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                //{
                                                //    if (arrFollowUpDate[0].Length <= 2)
                                                //    {
                                                //        followUpDate = Convert.ToDateTime(strFollowUpDate);
                                                //    }
                                                //    else
                                                //    {
                                                //        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                //        followUpDate = date;
                                                //    }
                                                //}
                                            }
                                            else if (strFollowUpDate.Contains("-"))
                                            {
                                                string[] arrFollowUpDate = strFollowUpDate.Split('-');
                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                {
                                                    if (arrFollowUpDate[0].Length <= 2)
                                                    {
                                                        followUpDate = Convert.ToDateTime(strFollowUpDate);
                                                    }
                                                    else
                                                    {
                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                        followUpDate = date;
                                                    }
                                                }
                                            }
                                            else if (strFollowUpDate.Contains("."))
                                            {
                                                string[] arrFollowUpDate = strFollowUpDate.Split('.');
                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                {
                                                    if (arrFollowUpDate[0].Length <= 2)
                                                    {
                                                        followUpDate = Convert.ToDateTime(strFollowUpDate);
                                                    }
                                                    else
                                                    {
                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                        followUpDate = date;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (dt.Columns.Contains(rmChinese.GetString("ProductVersion")) && !row.IsNull(rmChinese.GetString("ProductVersion")))
                                    {
                                        strProductVersion = row[rmChinese.GetString("ProductVersion")].ToString();

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("ProductVersion")) && !row.IsNull(rmEnglish.GetString("ProductVersion")))
                                    {
                                        strProductVersion = row[rmEnglish.GetString("ProductVersion")].ToString();

                                    }
                                    else
                                    {
                                        strProductVersion = string.Empty;
                                    }

                                    if (dt.Columns.Contains(rmChinese.GetString("DemoDate")) && !row.IsNull(rmChinese.GetString("DemoDate")))
                                    {
                                        if (row[rmChinese.GetString("DemoDate")].GetType() == typeof(DateTime))
                                        {
                                            demoDate = Convert.ToDateTime(row[rmChinese.GetString("DemoDate")]);
                                        }
                                        else if (row[rmChinese.GetString("DemoDate")].GetType() == typeof(string))
                                        {
                                            string strFollowUpDate = row[rmChinese.GetString("DemoDate")].ToString();
                                            if (strFollowUpDate.Contains("/"))
                                            {
                                                string strdate = row[rmChinese.GetString("DemoDate")].ToString();
                                                if (strdate != string.Empty)
                                                {
                                                    demoDate = DateTime.ParseExact(strdate, "yyyy-MM-dd HH:mm:ss", null);
                                                }
                                                //string[] arrFollowUpDate = strFollowUpDate.Split('/');
                                                //if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                //{
                                                //    if (arrFollowUpDate[0].Length <= 2)
                                                //    {
                                                //        demoDate = Convert.ToDateTime(strFollowUpDate);
                                                //    }
                                                //    else
                                                //    {
                                                //        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                //        demoDate = date;
                                                //    }
                                                //}
                                            }
                                            else if (strFollowUpDate.Contains("-"))
                                            {
                                                string[] arrFollowUpDate = strFollowUpDate.Split('-');
                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                {
                                                    if (arrFollowUpDate[0].Length <= 2)
                                                    {
                                                        demoDate = Convert.ToDateTime(strFollowUpDate);
                                                    }
                                                    else
                                                    {
                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                        demoDate = date;
                                                    }
                                                }
                                            }
                                            else if (strFollowUpDate.Contains("."))
                                            {
                                                string[] arrFollowUpDate = strFollowUpDate.Split('.');
                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                {
                                                    if (arrFollowUpDate[0].Length <= 2)
                                                    {
                                                        demoDate = Convert.ToDateTime(strFollowUpDate);
                                                    }
                                                    else
                                                    {
                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                        demoDate = date;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("DemoDate")) && !row.IsNull(rmEnglish.GetString("DemoDate")))
                                    {
                                        if (row[rmEnglish.GetString("DemoDate")].GetType() == typeof(DateTime))
                                        {
                                            demoDate = Convert.ToDateTime(row[rmEnglish.GetString("DemoDate")]);
                                        }
                                        else if (row[rmEnglish.GetString("DemoDate")].GetType() == typeof(string))
                                        {
                                            string strFollowUpDate = row[rmEnglish.GetString("DemoDate")].ToString();
                                            if (strFollowUpDate.Contains("/"))
                                            {
                                                string strdate = row[rmEnglish.GetString("DemoDate")].ToString();
                                                if (strdate != string.Empty)
                                                {
                                                    demoDate = DateTime.ParseExact(strdate.Trim(), "M/d/yyyy", null);
                                                }
                                                //string[] arrFollowUpDate = strFollowUpDate.Split('/');
                                                //if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                //{
                                                //    if (arrFollowUpDate[0].Length <= 2)
                                                //    {
                                                //        demoDate = Convert.ToDateTime(strFollowUpDate);
                                                //    }
                                                //    else
                                                //    {
                                                //        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                //        demoDate = date;
                                                //    }
                                                //}
                                            }
                                            else if (strFollowUpDate.Contains("-"))
                                            {
                                                string[] arrFollowUpDate = strFollowUpDate.Split('-');
                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                {
                                                    if (arrFollowUpDate[0].Length <= 2)
                                                    {
                                                        demoDate = Convert.ToDateTime(strFollowUpDate);
                                                    }
                                                    else
                                                    {
                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                        demoDate = date;
                                                    }
                                                }
                                            }
                                            else if (strFollowUpDate.Contains("."))
                                            {
                                                string[] arrFollowUpDate = strFollowUpDate.Split('.');
                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                {
                                                    if (arrFollowUpDate[0].Length <= 2)
                                                    {
                                                        demoDate = Convert.ToDateTime(strFollowUpDate);
                                                    }
                                                    else
                                                    {
                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                        demoDate = date;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (dt.Columns.Contains(rmChinese.GetString("Category")) && !row.IsNull(rmChinese.GetString("Category")))
                                    {
                                        strCategory = (row[rmChinese.GetString("Category")].ToString());

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Category")) && !row.IsNull(rmEnglish.GetString("Category")))
                                    {
                                        strCategory = (row[rmEnglish.GetString("Category")].ToString());

                                    }
                                    else
                                    {
                                        strCategory = string.Empty;
                                    }


                                    if (dt.Columns.Contains(rmChinese.GetString("QuoteDate")) && !row.IsNull(rmChinese.GetString("QuoteDate")))
                                    {
                                        if (row[rmChinese.GetString("QuoteDate")].GetType() == typeof(DateTime))
                                        {
                                            quoteDate = Convert.ToDateTime(row[rmChinese.GetString("QuoteDate")]);
                                        }
                                        else if (row[rmChinese.GetString("QuoteDate")].GetType() == typeof(string))
                                        {
                                            string strFollowUpDate = row[rmChinese.GetString("QuoteDate")].ToString();
                                            if (strFollowUpDate.Contains("/"))
                                            {
                                                string strdate = row[rmChinese.GetString("QuoteDate")].ToString();
                                                if (strdate != string.Empty)
                                                {
                                                    quoteDate = DateTime.ParseExact(strdate, "M/d/yyyy", null);
                                                }
                                                //string[] arrFollowUpDate = strFollowUpDate.Split('/');
                                                //if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                //{
                                                //    if (arrFollowUpDate[0].Length <= 2)
                                                //    {
                                                //        quoteDate = Convert.ToDateTime(strFollowUpDate);
                                                //    }
                                                //    else
                                                //    {
                                                //        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                //        quoteDate = date;
                                                //    }
                                                //}
                                            }
                                            else if (strFollowUpDate.Contains("-"))
                                            {
                                                string[] arrFollowUpDate = strFollowUpDate.Split('-');
                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                {
                                                    if (arrFollowUpDate[0].Length <= 2)
                                                    {
                                                        quoteDate = Convert.ToDateTime(strFollowUpDate);
                                                    }
                                                    else
                                                    {
                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                        quoteDate = date;
                                                    }
                                                }
                                            }
                                            else if (strFollowUpDate.Contains("."))
                                            {
                                                string[] arrFollowUpDate = strFollowUpDate.Split('.');
                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                {
                                                    if (arrFollowUpDate[0].Length <= 2)
                                                    {
                                                        quoteDate = Convert.ToDateTime(strFollowUpDate);
                                                    }
                                                    else
                                                    {
                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                        quoteDate = date;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    else if (dt.Columns.Contains(rmEnglish.GetString("QuoteDate")) && !row.IsNull(rmEnglish.GetString("QuoteDate")))
                                    {
                                        if (row[rmEnglish.GetString("QuoteDate")].GetType() == typeof(DateTime))
                                        {
                                            quoteDate = Convert.ToDateTime(row[rmEnglish.GetString("QuoteDate")]);
                                        }
                                        else if (row[rmEnglish.GetString("QuoteDate")].GetType() == typeof(string))
                                        {
                                            string strFollowUpDate = row[rmEnglish.GetString("QuoteDate")].ToString();
                                            if (strFollowUpDate.Contains("/"))
                                            {
                                                string strdate = row[rmEnglish.GetString("QuoteDate")].ToString();
                                                if (strdate != string.Empty)
                                                {
                                                    quoteDate = DateTime.ParseExact(strdate.Trim(), "M/d/yyyy", null);
                                                }
                                                //string[] arrFollowUpDate = strFollowUpDate.Split('/');
                                                //if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                //{
                                                //    if (arrFollowUpDate[0].Length <= 2)
                                                //    {
                                                //        quoteDate = Convert.ToDateTime(strFollowUpDate);
                                                //    }
                                                //    else
                                                //    {
                                                //        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                //        quoteDate = date;
                                                //    }
                                                //}
                                            }
                                            else if (strFollowUpDate.Contains("-"))
                                            {
                                                string[] arrFollowUpDate = strFollowUpDate.Split('-');
                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                {
                                                    if (arrFollowUpDate[0].Length <= 2)
                                                    {
                                                        quoteDate = Convert.ToDateTime(strFollowUpDate);
                                                    }
                                                    else
                                                    {
                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                        quoteDate = date;
                                                    }
                                                }
                                            }
                                            else if (strFollowUpDate.Contains("."))
                                            {
                                                string[] arrFollowUpDate = strFollowUpDate.Split('.');
                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                {
                                                    if (arrFollowUpDate[0].Length <= 2)
                                                    {
                                                        quoteDate = Convert.ToDateTime(strFollowUpDate);
                                                    }
                                                    else
                                                    {
                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                        quoteDate = date;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (dt.Columns.Contains(rmChinese.GetString("Probability")) && !row.IsNull(rmChinese.GetString("Probability")))
                                    {
                                        probablity = Convert.ToInt32(row[rmChinese.GetString("Probability")]);

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Probability")) && !row.IsNull(rmEnglish.GetString("Probability")))
                                    {
                                        probablity = Convert.ToInt32(row[rmEnglish.GetString("Probability")]);

                                    }
                                    else
                                    {
                                        probablity = 0;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("Rating")) && !row.IsNull(rmChinese.GetString("Rating")))
                                    {
                                        rating = (row[rmChinese.GetString("Rating")].ToString());

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Rating")) && !row.IsNull(rmEnglish.GetString("Rating")))
                                    {
                                        rating = (row[rmEnglish.GetString("Rating")].ToString());

                                    }
                                    else
                                    {
                                        rating = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("Owner")) && !row.IsNull(rmChinese.GetString("Owner")))
                                    {
                                        strOwner = (row[rmChinese.GetString("Owner")].ToString());

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Owner")) && !row.IsNull(rmEnglish.GetString("Owner")))
                                    {
                                        strOwner = (row[rmEnglish.GetString("Owner")].ToString());

                                    }
                                    else
                                    {
                                        strOwner = string.Empty;
                                    }


                                    if (dt.Columns.Contains(rmChinese.GetString("Status")) && !row.IsNull(rmChinese.GetString("Status")))
                                    {
                                        strStatus = row[rmChinese.GetString("Status")].ToString();

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Status")) && !row.IsNull(rmEnglish.GetString("Status")))
                                    {
                                        strStatus = row[rmEnglish.GetString("Status")].ToString();

                                    }
                                    else
                                    {
                                        strStatus = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("OfficePhone")) && !row.IsNull(rmChinese.GetString("OfficePhone")))
                                    {
                                        strOfficePhone = row[rmChinese.GetString("OfficePhone")].ToString();

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("OfficePhone")) && !row.IsNull(rmEnglish.GetString("OfficePhone")))
                                    {
                                        strOfficePhone = row[rmEnglish.GetString("OfficePhone")].ToString();

                                    }
                                    else
                                    {
                                        strOfficePhone = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("MobilePhone")) && !row.IsNull(rmChinese.GetString("MobilePhone")))
                                    {
                                        strMobilePhone = row[rmChinese.GetString("MobilePhone")].ToString();

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("MobilePhone")) && !row.IsNull(rmEnglish.GetString("MobilePhone")))
                                    {
                                        strMobilePhone = row[rmEnglish.GetString("MobilePhone")].ToString();

                                    }
                                    else
                                    {
                                        strMobilePhone = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("HomePhone")) && !row.IsNull(rmChinese.GetString("HomePhone")))
                                    {
                                        strHomePhone = row[rmChinese.GetString("HomePhone")].ToString();

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("HomePhone")) && !row.IsNull(rmEnglish.GetString("HomePhone")))
                                    {
                                        strHomePhone = row[rmEnglish.GetString("HomePhone")].ToString();

                                    }
                                    else
                                    {
                                        strHomePhone = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("OtherPhone")) && !row.IsNull(rmChinese.GetString("OtherPhone")))
                                    {
                                        strOtherPhone = row[rmChinese.GetString("OtherPhone")].ToString();

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("OtherPhone")) && !row.IsNull(rmEnglish.GetString("OtherPhone")))
                                    {
                                        strOtherPhone = row[rmEnglish.GetString("OtherPhone")].ToString();

                                    }
                                    else
                                    {
                                        strOtherPhone = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("WebSite")) && !row.IsNull(rmChinese.GetString("WebSite")))
                                    {
                                        strWebsite = row[rmChinese.GetString("WebSite")].ToString();

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("WebSite")) && !row.IsNull(rmEnglish.GetString("WebSite")))
                                    {
                                        strWebsite = row[rmEnglish.GetString("WebSite")].ToString();

                                    }
                                    else
                                    {
                                        strWebsite = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("Email")) && !row.IsNull(rmChinese.GetString("Email")))
                                    {
                                        strEmail = row[rmChinese.GetString("Email")].ToString();

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Email")) && !row.IsNull(rmEnglish.GetString("Email")))
                                    {
                                        strEmail = row[rmEnglish.GetString("Email")].ToString();

                                    }
                                    else
                                    {
                                        strEmail = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("Fax")) && !row.IsNull(rmChinese.GetString("Fax")))
                                    {
                                        strFax = row[rmChinese.GetString("Fax")].ToString();

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Fax")) && !row.IsNull(rmEnglish.GetString("Fax")))
                                    {
                                        strFax = row[rmEnglish.GetString("Fax")].ToString();

                                    }
                                    else
                                    {
                                        strFax = string.Empty;
                                    }


                                    if (dt.Columns.Contains(rmChinese.GetString("PreferredContactMethod")) && !row.IsNull(rmChinese.GetString("PreferredContactMethod")))
                                    {
                                        strPreferredContactMethod = row[rmChinese.GetString("PreferredContactMethod")].ToString();

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("PreferredContactMethod")) && !row.IsNull(rmEnglish.GetString("PreferredContactMethod")))
                                    {
                                        strPreferredContactMethod = row[rmEnglish.GetString("PreferredContactMethod")].ToString();

                                    }
                                    else
                                    {
                                        strPreferredContactMethod = string.Empty;
                                    }

                                    if (dt.Columns.Contains(rmChinese.GetString("Prospect")) && !row.IsNull(rmChinese.GetString("Prospect")))
                                    {
                                        strProspect = row[rmChinese.GetString("Prospect")].ToString();

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Prospect")) && !row.IsNull(rmEnglish.GetString("Prospect")))
                                    {
                                        strProspect = row[rmEnglish.GetString("Prospect")].ToString();

                                    }
                                    else
                                    {
                                        strProspect = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("Account")) && !row.IsNull(rmChinese.GetString("Account")))
                                    {
                                        strAccount = row[rmChinese.GetString("Account")].ToString();

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Account")) && !row.IsNull(rmEnglish.GetString("Account")))
                                    {
                                        strAccount = row[rmEnglish.GetString("Account")].ToString();

                                    }
                                    else
                                    {
                                        strAccount = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("Industry")) && !row.IsNull(rmChinese.GetString("Industry")))
                                    {
                                        strIndustry = row[rmChinese.GetString("Industry")].ToString();

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Industry")) && !row.IsNull(rmEnglish.GetString("Industry")))
                                    {
                                        strIndustry = row[rmEnglish.GetString("Industry")].ToString();

                                    }
                                    else
                                    {
                                        strIndustry = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("ClientNumber")) && !row.IsNull(rmChinese.GetString("ClientNumber")))
                                    {
                                        strClientNumber = row[rmChinese.GetString("ClientNumber")].ToString();

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("ClientNumber")) && !row.IsNull(rmEnglish.GetString("ClientNumber")))
                                    {
                                        strClientNumber = row[rmEnglish.GetString("ClientNumber")].ToString();

                                    }
                                    else
                                    {
                                        strClientNumber = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("Remark")) && !row.IsNull(rmChinese.GetString("Remark")))
                                    {
                                        strRemark = row[rmChinese.GetString("Remark")].ToString();

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Remark")) && !row.IsNull(rmEnglish.GetString("Remark")))
                                    {
                                        strRemark = row[rmEnglish.GetString("Remark")].ToString();

                                    }
                                    else
                                    {
                                        strRemark = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("Region")) && !row.IsNull(rmChinese.GetString("Region")))
                                    {
                                        strRegion = row[rmChinese.GetString("Region")].ToString();

                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Region")) && !row.IsNull(rmEnglish.GetString("Region")))
                                    {
                                        strRegion = row[rmEnglish.GetString("Region")].ToString();

                                    }
                                    else
                                    {
                                        strRegion = string.Empty;
                                    }

                                    if (strTopic != null && strTopic.Length > 0 && strOwner != null && strOwner.Length > 0 && strCategory != null && strCategory.Length > 0 && strPotentialCustomer != null && strPotentialCustomer.Length > 0)
                                    {
                                        //Customer customer = ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("[CustomerName]='" + strPotentialCustomer + "'"));
                                        //if (customer != null)
                                        //{
                                        //    objCRMProspects.Customer = customer;
                                        //}
                                        //else
                                        //{
                                        //    IObjectSpace os = Application.CreateObjectSpace();
                                        //    Customer objCustom = os.CreateObject<Customer>();
                                        //    //objCustom.Owner = objCRMOpportunitys.Owner;
                                        //    //string strFirstName = objCustom.Owner.FirstName;
                                        //    Employee objUser = os.FindObject<Employee>(CriteriaOperator.Parse("[FirstName]=?", strOwner));
                                        //    if (objUser != null)
                                        //    {
                                        //        objCustom.Owner = objUser;
                                        //    }
                                        //    else
                                        //    {
                                        //        objCustom.Owner = null;
                                        //    }
                                        //    if (strPreferredContactMethod != null)
                                        //    {
                                        //        if (strPreferredContactMethod == "Email")
                                        //        {
                                        //            objCustom.PreferredContactMethod = PreferredContactMethod.Email;
                                        //        }
                                        //        else if (strPreferredContactMethod == "Phone")
                                        //        {
                                        //            objCustom.PreferredContactMethod = PreferredContactMethod.Phone;
                                        //        }
                                        //        else if (strPreferredContactMethod == "Fax")
                                        //        {
                                        //            objCustom.PreferredContactMethod = PreferredContactMethod.Fax;
                                        //        }
                                        //        else
                                        //        {
                                        //            objCustom.PreferredContactMethod = PreferredContactMethod.Any;
                                        //        }
                                        //    }
                                        //    objCustom.HomePhone = strHomePhone;
                                        //    objCustom.OfficePhone = strOfficePhone;
                                        //    objCustom.MobilePhone = strMobilePhone;
                                        //    objCustom.OtherPhone = strOfficePhone;
                                        //    objCustom.Fax = strFax;
                                        //    objCustom.Email = strEmail;
                                        //    objCustom.WebSite = strWebsite;
                                        //    //objCustom.Name = strPotentialCustomer;
                                        //    //objCustom.BillToCity = strCity;
                                        //    //objCustom.BillToCountry = strCountry;
                                        //    //objCustom.BillToState = strState;
                                        //    //objCustom.BillToStreet1 = strStrret1;
                                        //    //objCustom.BillToStreet2 = strStreet2;
                                        //    //objCustom.BillToZip = strZip;
                                        //    os.CommitChanges();
                                        //}

                                        Customer customerss = ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("[CustomerName]='" + strPotentialCustomer + "'"));
                                        if (customerss != null)
                                        {
                                            objCRMProspects.Customer = customerss;
                                        }
                                        objCRMProspects.Name = strTopic;
                                        objCRMProspects.ClientCode = strClientCode;
                                        if (followUpDate != DateTime.MinValue)
                                        {
                                            objCRMProspects.FollowUpDate = followUpDate;
                                        }
                                        if (demoDate != DateTime.MinValue)
                                        {
                                            objCRMProspects.DemoDate = demoDate;
                                        }
                                        if (quoteDate != DateTime.MinValue)
                                        {
                                            objCRMProspects.QuoteDate = quoteDate;
                                        }
                                        objCRMProspects.Probability = probablity;
                                        objCRMProspects.Prospects = strProspect;
                                        objCRMProspects.OfficePhone = strOfficePhone;
                                        objCRMProspects.OtherPhone = strOtherPhone;
                                        objCRMProspects.WebSite = strWebsite;
                                        objCRMProspects.HomePhone = strHomePhone;
                                        objCRMProspects.MobilePhone = strMobilePhone;
                                        objCRMProspects.Fax = strFax;
                                        objCRMProspects.Email = strEmail;
                                        objCRMProspects.Street1 = strStrret1;
                                        objCRMProspects.Street2 = strStreet2;
                                        objCRMProspects.Account = strAccount;
                                        //objCRMProspects.Industry = strIndustry;
                                        objCRMProspects.Remark = strRemark;
                                        objCRMProspects.ClientNumber = strClientNumber;

                                        if (!string.IsNullOrEmpty(strIndustry) && !string.IsNullOrWhiteSpace(strIndustry))
                                        {
                                            IObjectSpace objspacindus = Application.CreateObjectSpace();
                                            KeyValue objindus = objspacindus.FindObject<KeyValue>(CriteriaOperator.Parse("[Name] = ?", strIndustry));
                                            if (objindus != null)
                                            {
                                                objCRMProspects.Industry = ObjectSpace.GetObject(objindus);
                                            }
                                            //else
                                            //{
                                            //    KeyValue crtindus = objspacindus.CreateObject<KeyValue>();
                                            //    crtindus.Name = strIndustry;
                                            //    Company objcomp = ObjectSpace.FindObject<Company>(CriteriaOperator.Parse("[GCRecord IS NULL]"));
                                            //    if(objcomp != null)
                                            //    {
                                            //        crtindus.Company = objspacindus.GetObjectByKey<Company>(objcomp);
                                            //    }
                                            //    keyType objcomp = ObjectSpace.FindObject<keyType>(CriteriaOperator.Parse("[GCRecord IS NULL]"));
                                            //    if (objcomp != null)
                                            //    {
                                            //        crtindus.Company = objspacindus.GetObjectByKey<Company>(objcomp);
                                            //    }

                                            //    objspacindus.CommitChanges();
                                            //    objCRMProspects.Industry = ObjectSpace.GetObject(crtindus);
                                            //}
                                        }
                                        if (!string.IsNullOrEmpty(strCategory) && !string.IsNullOrWhiteSpace(strCategory))
                                        {
                                            IObjectSpace objspaccategory = Application.CreateObjectSpace();
                                            CustomerCategory objcategory = objspaccategory.FindObject<CustomerCategory>(CriteriaOperator.Parse("[Category] = ?", strCategory));
                                            if (objcategory != null)
                                            {
                                                objCRMProspects.Category = ObjectSpace.GetObject(objcategory);
                                            }
                                            else
                                            {
                                                CustomerCategory crtctgory = objspaccategory.CreateObject<CustomerCategory>();
                                                crtctgory.Category = strCategory;
                                                objspaccategory.CommitChanges();
                                                objCRMProspects.Category = ObjectSpace.GetObject(crtctgory);
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(strRegion) && !string.IsNullOrWhiteSpace(strRegion))
                                        {
                                            IObjectSpace objspacregion = Application.CreateObjectSpace();
                                            Region objregion = objspacregion.FindObject<Region>(CriteriaOperator.Parse("[Name] = ?", strRegion));
                                            if (objregion != null)
                                            {
                                                objCRMProspects.Region = ObjectSpace.GetObject(objregion);
                                            }
                                            else
                                            {
                                                Region crtregion = objspacregion.CreateObject<Region>();
                                                crtregion.RegionName = strRegion;
                                                objspacregion.CommitChanges();
                                                objCRMProspects.Region = ObjectSpace.GetObject(crtregion);
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(strCity) && !string.IsNullOrWhiteSpace(strCity))
                                        {
                                            IObjectSpace objspaccity = Application.CreateObjectSpace();
                                            City objcity = objspaccity.FindObject<City>(CriteriaOperator.Parse("[CityName] = ?", strCity));
                                            if (objcity != null)
                                            {
                                                objCRMProspects.City = ObjectSpace.GetObject(objcity);
                                            }
                                            else
                                            {
                                                City crtcity = objspaccity.CreateObject<City>();
                                                crtcity.CityName = strCity;
                                                objspaccity.CommitChanges();
                                                objCRMProspects.City = ObjectSpace.GetObject(crtcity);
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(strState) && !string.IsNullOrWhiteSpace(strState))
                                        {
                                            IObjectSpace objspacstate = Application.CreateObjectSpace();
                                            CustomState objstate = objspacstate.FindObject<CustomState>(CriteriaOperator.Parse("[LongName] = ? or [ShortName] = ?", strState, strState));
                                            if (objstate != null)
                                            {
                                                objCRMProspects.State = ObjectSpace.GetObject(objstate);
                                            }
                                            else
                                            {
                                                CustomState crtstate = objspacstate.CreateObject<CustomState>();
                                                crtstate.LongName = strState;
                                                objspacstate.CommitChanges();
                                                objCRMProspects.State = ObjectSpace.GetObject(crtstate);
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(strCountry) && !string.IsNullOrWhiteSpace(strCountry))
                                        {
                                            IObjectSpace objspaccntry = Application.CreateObjectSpace();
                                            CustomCountry objcntry = objspaccntry.FindObject<CustomCountry>(CriteriaOperator.Parse("[EnglishLongName] = ? or [EnglishShortName] = ? ", strCountry, strCountry));
                                            if (objcntry != null)
                                            {
                                                objCRMProspects.Country = ObjectSpace.GetObject(objcntry);
                                            }
                                            else
                                            {
                                                CustomCountry crtcntry = objspaccntry.CreateObject<CustomCountry>();
                                                crtcntry.EnglishLongName = strCountry;
                                                objspaccntry.CommitChanges();
                                                objCRMProspects.Country = ObjectSpace.GetObject(crtcntry);
                                            }
                                        }
                                        objCRMProspects.Zip = strZip;
                                        if (rating != null)
                                        {
                                            if (rating == "Hot")
                                            {
                                                objCRMProspects.Rating = ProspectsRating.Hot;
                                            }
                                            else if (rating == "Warm")
                                            {
                                                objCRMProspects.Rating = ProspectsRating.Warm;
                                            }
                                            else if (rating == "Cold")
                                            {
                                                objCRMProspects.Rating = ProspectsRating.Cold;
                                            }

                                        }
                                        Employee objEmployee = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("[FirstName]='" + strOwner + "'"));
                                        if (objEmployee != null)
                                        {
                                            objCRMProspects.Owner = ObjectSpace.GetObject(objEmployee);
                                        }
                                        else
                                        {
                                            objCRMProspects.Owner = null;
                                        }

                                        Contact findContact = ObjectSpace.FindObject<Contact>(CriteriaOperator.Parse("[FirstName]='" + strprimaryContact + "'"));
                                        if (findContact == null)
                                        {
                                            Contact contact = ObjectSpace.CreateObject<Contact>();
                                            contact.FirstName = strTopic;
                                            // contact.LastName = objCRMOpportunitys.la;
                                            contact.OfficePhone = objCRMProspects.OfficePhone;
                                            contact.MobilePhone = objCRMProspects.MobilePhone;
                                            contact.OtherPhone = objCRMProspects.OtherPhone;
                                            contact.HomePhone = objCRMProspects.HomePhone;
                                            contact.Email = objCRMProspects.Email;
                                            contact.Fax = objCRMProspects.Fax;
                                            contact.Street1 = objCRMProspects.Street1;
                                            contact.Street2 = objCRMProspects.Street2;
                                            contact.City = objCRMProspects.City;
                                            contact.State = objCRMProspects.State;
                                            contact.Country = objCRMProspects.Country;
                                            contact.Zip = objCRMProspects.Zip;
                                            //contact.ShipToStreet1 = objCRMProspects.BillToStreet1;
                                            //contact.ShipToStreet2 = objCRMProspects.BillToStreet2;
                                            //contact.ShipToCity = objCRMProspects.BillToCity;
                                            //contact.ShipToState = objCRMProspects.BillToState;
                                            //contact.ShipToCountry = objCRMProspects.BillToCountry;
                                            //contact.ShipToZip = objCRMProspects.BillToZip;
                                            //contact.Owner = objCRMProspects.Owner;
                                            //contact.Status = CustomerStatus.Active;
                                            //contact.Prospects = objCRMProspects;
                                            objCRMProspects.PrimaryContact = contact;
                                            ObjectSpace.CommitChanges();
                                        }
                                        else
                                        {
                                            objCRMProspects.PrimaryContact = findContact;
                                        }

                                        if (strStatus != null)
                                        {
                                            if (strStatus == "Won")
                                            {
                                                objCRMProspects.Status = ProspectsStatus.Won;
                                            }
                                            else if (strStatus == "Canceled")
                                            {
                                                objCRMProspects.Status = ProspectsStatus.Cancelled;
                                            }
                                            //else if (strStatus == "OutSold")
                                            //{
                                            //    objCRMOpportunitys.Status = OpportunityStatus.OutSold;
                                            //}
                                            else
                                            {
                                                objCRMProspects.Status = ProspectsStatus.None;
                                            }
                                        }
                                        ObjectSpace.CommitChanges();


                                        if (objCRMProspects.Status == ProspectsStatus.Won)
                                        {
                                            string strID;
                                            if (objCRMProspects.Owner != null)
                                            {
                                                strID = objCRMProspects.Owner.Oid.ToString();
                                            }
                                            else
                                            {
                                                strID = strCurrentUserId;
                                            }
                                            if (objCRMProspects.Customer != null)
                                            {
                                                Customer findAccount = ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("[CustomerName]='" + objCRMProspects.Name + "'"));
                                                if (findAccount == null)
                                                {
                                                    IObjectSpace accountObjectSpace = Application.CreateObjectSpace(typeof(Customer));
                                                    Customer account = ObjectSpace.CreateObject<Customer>();
                                                    if (objCRMProspects != null)
                                                    {
                                                        account.CustomerName = objCRMProspects.Name;
                                                        //account.SICCode = objCustomer.SICCode;
                                                        //account.NumberOfEmployees = objCustomer.NumberOfEmployees;
                                                        account.Account = objCRMProspects.Account;
                                                        account.ClientNumber = objCRMProspects.ClientNumber;
                                                        account.WebSite = objCRMProspects.WebSite;
                                                        account.Address = objCRMProspects.Street1;
                                                        account.Address1 = objCRMProspects.Street2;
                                                        account.City = objCRMProspects.City;
                                                        account.State = objCRMProspects.State;
                                                        account.Country = objCRMProspects.Country;
                                                        account.Zip = objCRMProspects.Zip;
                                                        account.OfficePhone = objCRMProspects.OfficePhone;
                                                        account.OtherPhone = objCRMProspects.OtherPhone;
                                                        account.MobilePhone = objCRMProspects.MobilePhone;
                                                        account.HomePhone = objCRMProspects.HomePhone;
                                                        account.Email = objCRMProspects.Email;
                                                        account.Fax = objCRMProspects.Fax;
                                                        account.Owner = objCRMProspects.Owner;
                                                        if (strPreferredContactMethod != null)
                                                        {
                                                            if (strPreferredContactMethod == "Email")
                                                            {
                                                                account.PreferredContactMethod = PreferredContactMethod.Email;
                                                            }
                                                            else if (strPreferredContactMethod == "Phone")
                                                            {
                                                                account.PreferredContactMethod = PreferredContactMethod.Phone;
                                                            }
                                                            else if (strPreferredContactMethod == "Fax")
                                                            {
                                                                account.PreferredContactMethod = PreferredContactMethod.Fax;
                                                            }
                                                            else
                                                            {
                                                                account.PreferredContactMethod = PreferredContactMethod.Any;
                                                            }
                                                        }
                                                        account.fCategory = objCRMProspects.Category;
                                                        //account.Industry = objCRMProspects.Industry;
                                                        //account.ClientCode = objCRMProspects.ClientCode;
                                                        //account.SourceProspects = objCRMProspects;
                                                        account.PrimaryContact = objCRMProspects.PrimaryContact;
                                                        Frame.GetController<EmailController>().CheckUserEmailPermission(strID, "New Account Notification", objCRMProspects, "Prospects to Account");
                                                        ObjectSpace.CommitChanges();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Customer customers = ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("[CustomerName]='" + strPotentialCustomer + "'"));
                                                if (customers != null)
                                                {
                                                    objCRMProspects.Customer = customers;
                                                }
                                                ObjectSpace.CommitChanges();
                                                //Customer objCustomer = ObjectSpace.GetObject<Customer>(objCRMProspects.Customer);
                                                Customer findAccount = ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("[CustomerName]='" + strPotentialCustomer + "'"));
                                                if (findAccount == null)
                                                {
                                                    IObjectSpace accountObjectSpace = Application.CreateObjectSpace(typeof(Customer));
                                                    Customer account = ObjectSpace.CreateObject<Customer>();

                                                    if (objCRMProspects != null)
                                                    {
                                                        account.CustomerName = objCRMProspects.Name;
                                                        //account.SICCode = objCustomer.SICCode;
                                                        //account.NumberOfEmployees = objCustomer.NumberOfEmployees;
                                                        account.WebSite = objCRMProspects.WebSite;
                                                        account.Address = objCRMProspects.Street1;
                                                        account.Address1 = objCRMProspects.Street2;
                                                        account.City = objCRMProspects.City;
                                                        account.State = objCRMProspects.State;
                                                        account.Country = objCRMProspects.Country;
                                                        account.Zip = objCRMProspects.Zip;
                                                        account.OfficePhone = objCRMProspects.OfficePhone;
                                                        account.OtherPhone = objCRMProspects.OtherPhone;
                                                        account.MobilePhone = objCRMProspects.MobilePhone;
                                                        account.HomePhone = objCRMProspects.HomePhone;
                                                        account.Email = objCRMProspects.Email;
                                                        account.Fax = objCRMProspects.Fax;
                                                        account.Owner = objCRMProspects.Owner;
                                                        if (strPreferredContactMethod != null)
                                                        {
                                                            if (strPreferredContactMethod == "Email")
                                                            {
                                                                account.PreferredContactMethod = PreferredContactMethod.Email;
                                                            }
                                                            else if (strPreferredContactMethod == "Phone")
                                                            {
                                                                account.PreferredContactMethod = PreferredContactMethod.Phone;
                                                            }
                                                            else if (strPreferredContactMethod == "Fax")
                                                            {
                                                                account.PreferredContactMethod = PreferredContactMethod.Fax;
                                                            }
                                                            else
                                                            {
                                                                account.PreferredContactMethod = PreferredContactMethod.Any;
                                                            }
                                                        }
                                                        account.fCategory = objCRMProspects.Category;
                                                        //account.Industry = objCRMProspects.Industry;
                                                        account.ClientCode = objCRMProspects.ClientCode;
                                                        //account.SourceProspects = objCRMProspects;
                                                        account.PrimaryContact = objCRMProspects.PrimaryContact;
                                                        ObjectSpace.CommitChanges();
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(strCategory))
                                        {
                                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Category"), InformationType.Error, 3000, InformationPosition.Top);
                                        }
                                        else
                                        if (string.IsNullOrEmpty(strTopic))
                                        {
                                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Topic"), InformationType.Error, 3000, InformationPosition.Top);
                                        }
                                        else
                                        if (string.IsNullOrEmpty(strOwner))
                                        {
                                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Owner"), InformationType.Error, 3000, InformationPosition.Top);
                                        }
                                        else
                                        if (string.IsNullOrEmpty(strPotentialCustomer))
                                        {
                                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "PotentialCustomer"), InformationType.Error, 3000, InformationPosition.Top);
                                        }
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotSaveProspects"), InformationType.Error, 3000, InformationPosition.Top);
                                }
                            }
                        }
                    }



                    //string strFilePath = HttpContext.Current.Server.MapPath(@"~\TestSpreadSheet\");
                    //string strLocalPath = HttpContext.Current.Server.MapPath(@"~\TestSpreadSheet\" + strFileName);
                    //if (Directory.Exists(strFilePath) == false)
                    //{
                    //    Directory.CreateDirectory(strFilePath);
                    //}
                    //byte[] file = itemsFile.File.Content;
                    //File.WriteAllBytes(strLocalPath, file);
                    //string fileExtension = Path.GetExtension(itemsFile.File.FileName);
                    //string connectionString = string.Empty;
                    //if (fileExtension == ".xlsx")
                    //{
                    //    connectionString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0 Xml;", strLocalPath);
                    //}
                    //else if (fileExtension == ".xls")
                    //{
                    //    connectionString = String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;", strLocalPath);
                    //}

                    //if (connectionString != string.Empty)
                    //{
                    //    using (var conn = new OleDbConnection(connectionString))
                    //    {
                    //        conn.Open();
                    //        List<string> sheets = new List<string>();
                    //        OleDbDataAdapter oleda = new OleDbDataAdapter();
                    //        DataTable sheetNameTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    //        foreach (DataRow drSheet in sheetNameTable.Rows)
                    //        {
                    //            if (drSheet["TABLE_NAME"].ToString().Contains("$"))
                    //            {
                    //                string s = drSheet["TABLE_NAME"].ToString();
                    //                sheets.Add(s.StartsWith("'") ? s.Substring(1, s.Length - 3) : s.Substring(0, s.Length - 1));
                    //            }
                    //        }

                    //        var cmd = conn.CreateCommand();
                    //        cmd.CommandText = String.Format(
                    //            @"SELECT * FROM [{0}]", sheets[0] + "$"
                    //            );

                    //        // variable name change
                    //        //packageunits.packageunit = new List<Packageunits>();
                    //        //packageunits.vendors = new List<Vendors>();
                    //        //   itemsltno.Items = new List<string>();
                    //        oleda = new OleDbDataAdapter(cmd);
                    //        using (dt = new DataTable())

                    //        {
                    //            oleda.Fill(dt);
                    //            foreach (DataRow row in dt.Rows)
                    //            {
                    //                var isEmpty = row.ItemArray.All(c => c is DBNull);
                    //                if (!isEmpty)
                    //                {

                    //                    List<string> errorlist = new List<string>();
                    //                    // IObjectSpace objSpace = Application.CreateObjectSpace();
                    //                    //CriteriaOperator crmOpportunityCriteria = CriteriaOperator.Parse("([Name]='" + row[0] + "' And[Category]='" + row[6] + "' And[Owner]='" + row[10] + "')");
                    //                    CriteriaOperator crmOpportunityCriteria = CriteriaOperator.Parse("[Name] = ?", row[0]);
                    //                    SaleBase objCRMOpportunity = (SaleBase)ObjectSpace.FindObject(typeof(SaleBase), crmOpportunityCriteria);
                    //                    if (objCRMOpportunity == null)
                    //                    {
                    //                        // IObjectSpace objSpace = Application.CreateObjectSpace();
                    //                        CRMOpportunity objCRMOpportunitys = ObjectSpace.CreateObject<CRMOpportunity>();
                    //                        if (dt.Columns.Contains(rmChinese.GetString("Topic")) && !row.IsNull(rmChinese.GetString("Topic")))
                    //                        {
                    //                            strTopic = row[rmChinese.GetString("Topic")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("Topic")) && !row.IsNull(rmEnglish.GetString("Topic")))
                    //                        {
                    //                            strTopic = row[rmEnglish.GetString("Topic")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strTopic = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("PotentialCustomer")) && !row.IsNull(rmChinese.GetString("PotentialCustomer")))
                    //                        {
                    //                            strPotentialCustomer = row[rmChinese.GetString("PotentialCustomer")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("PotentialCustomer")) && !row.IsNull(rmEnglish.GetString("PotentialCustomer")))
                    //                        {
                    //                            strPotentialCustomer = row[rmEnglish.GetString("PotentialCustomer")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strPotentialCustomer = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("ClientCode")) && !row.IsNull(rmChinese.GetString("ClientCode")))
                    //                        {
                    //                            strClientCode = row[rmChinese.GetString("ClientCode")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("ClientCode")) && !row.IsNull(rmEnglish.GetString("ClientCode")))
                    //                        {
                    //                            strClientCode = row[rmEnglish.GetString("ClientCode")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strClientCode = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("FollowUpDate")) && !row.IsNull(rmChinese.GetString("FollowUpDate")))
                    //                        {
                    //                            if (row[rmChinese.GetString("FollowUpDate")].GetType() == typeof(DateTime))
                    //                            {
                    //                                followUpDate = Convert.ToDateTime(row[rmChinese.GetString("FollowUpDate")]);
                    //                            }
                    //                            else if (row[rmChinese.GetString("FollowUpDate")].GetType() == typeof(string))
                    //                            {
                    //                                string strFollowUpDate = row[rmChinese.GetString("FollowUpDate")].ToString();
                    //                                followUpDate = DateTime.ParseExact(strFollowUpDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    //                            }

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("FollowUpDate")) && !row.IsNull(rmEnglish.GetString("FollowUpDate")))
                    //                        {
                    //                            if (row[rmEnglish.GetString("FollowUpDate")].GetType() == typeof(DateTime))
                    //                            {
                    //                                followUpDate = Convert.ToDateTime(row[rmEnglish.GetString("FollowUpDate")]);
                    //                            }
                    //                            else if (row[rmEnglish.GetString("FollowUpDate")].GetType() == typeof(string))
                    //                            {
                    //                                string strFollowUpDate = row[rmEnglish.GetString("FollowUpDate")].ToString();
                    //                                followUpDate = DateTime.ParseExact(strFollowUpDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    //                            }

                    //                        }


                    //                        if (dt.Columns.Contains(rmChinese.GetString("ProductVersion")) && !row.IsNull(rmChinese.GetString("ProductVersion")))
                    //                        {
                    //                            strProductVersion = row[rmChinese.GetString("ProductVersion")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("ProductVersion")) && !row.IsNull(rmEnglish.GetString("ProductVersion")))
                    //                        {
                    //                            strProductVersion = row[rmEnglish.GetString("ProductVersion")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strProductVersion = string.Empty;
                    //                        }

                    //                        if (dt.Columns.Contains(rmChinese.GetString("DemoDate")) && !row.IsNull(rmChinese.GetString("DemoDate")))
                    //                        {
                    //                            if (row[rmChinese.GetString("DemoDate")].GetType() == typeof(DateTime))
                    //                            {
                    //                                demoDate = Convert.ToDateTime(row[rmChinese.GetString("DemoDate")]);
                    //                            }
                    //                            else if (row[rmChinese.GetString("DemoDate")].GetType() == typeof(string))
                    //                            {
                    //                                string strDemoDate = row[rmChinese.GetString("DemoDate")].ToString();
                    //                                demoDate = DateTime.ParseExact(strDemoDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    //                            }

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("DemoDate")) && !row.IsNull(rmEnglish.GetString("DemoDate")))
                    //                        {
                    //                            if (row[rmEnglish.GetString("DemoDate")].GetType() == typeof(DateTime))
                    //                            {
                    //                                demoDate = Convert.ToDateTime(row[rmEnglish.GetString("DemoDate")]);
                    //                            }
                    //                            else if (row[rmEnglish.GetString("DemoDate")].GetType() == typeof(string))
                    //                            {
                    //                                string strDemoDate = row[rmEnglish.GetString("DemoDate")].ToString();
                    //                                demoDate = DateTime.ParseExact(strDemoDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    //                            }

                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("Category")) && !row.IsNull(rmChinese.GetString("Category")))
                    //                        {
                    //                            strCategory = (row[rmChinese.GetString("Category")].ToString());

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("Category")) && !row.IsNull(rmEnglish.GetString("Category")))
                    //                        {
                    //                            strCategory = (row[rmEnglish.GetString("Category")].ToString());

                    //                        }
                    //                        else
                    //                        {
                    //                            strCategory = string.Empty;
                    //                        }


                    //                        if (dt.Columns.Contains(rmChinese.GetString("QuoteDate")) && !row.IsNull(rmChinese.GetString("QuoteDate")))
                    //                        {
                    //                            if (row[rmChinese.GetString("QuoteDate")].GetType() == typeof(DateTime))
                    //                            {
                    //                                quoteDate = Convert.ToDateTime(row[rmChinese.GetString("QuoteDate")]);
                    //                            }
                    //                            else if (row[rmChinese.GetString("DemoDate")].GetType() == typeof(string))
                    //                            {
                    //                                string strquoteDate = row[rmChinese.GetString("QuoteDate")].ToString();
                    //                                quoteDate = DateTime.ParseExact(strquoteDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    //                            }

                    //                        }

                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("QuoteDate")) && !row.IsNull(rmEnglish.GetString("QuoteDate")))
                    //                        {
                    //                            if (row[rmEnglish.GetString("QuoteDate")].GetType() == typeof(DateTime))
                    //                            {
                    //                                quoteDate = Convert.ToDateTime(row[rmEnglish.GetString("QuoteDate")]);
                    //                            }
                    //                            else if (row[rmEnglish.GetString("QuoteDate")].GetType() == typeof(string))
                    //                            {
                    //                                string strquoteDate = row[rmEnglish.GetString("QuoteDate")].ToString();
                    //                                quoteDate = DateTime.ParseExact(strquoteDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    //                            }

                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("Probability")) && !row.IsNull(rmChinese.GetString("Probability")))
                    //                        {
                    //                            probablity = Convert.ToInt32(row[rmChinese.GetString("Probability")]);

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("Probability")) && !row.IsNull(rmEnglish.GetString("Probability")))
                    //                        {
                    //                            probablity = Convert.ToInt32(row[rmEnglish.GetString("Probability")]);

                    //                        }
                    //                        else
                    //                        {
                    //                            probablity = 0;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("Rating")) && !row.IsNull(rmChinese.GetString("Rating")))
                    //                        {
                    //                            rating = (row[rmChinese.GetString("Rating")].ToString());

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("Rating")) && !row.IsNull(rmEnglish.GetString("Rating")))
                    //                        {
                    //                            rating = (row[rmEnglish.GetString("Rating")].ToString());

                    //                        }
                    //                        else
                    //                        {
                    //                            rating = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("Owner")) && !row.IsNull(rmChinese.GetString("Owner")))
                    //                        {
                    //                            strOwner = (row[rmChinese.GetString("Owner")].ToString());

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("Owner")) && !row.IsNull(rmEnglish.GetString("Owner")))
                    //                        {
                    //                            strOwner = (row[rmEnglish.GetString("Owner")].ToString());

                    //                        }
                    //                        else
                    //                        {
                    //                            strOwner = string.Empty;
                    //                        }

                    //                        if (dt.Columns.Contains(rmChinese.GetString("SourceLead")) && !row.IsNull(rmChinese.GetString("SourceLead")))
                    //                        {
                    //                            strSourceLead = row[rmChinese.GetString("SourceLead")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("SourceLead")) && !row.IsNull(rmEnglish.GetString("SourceLead")))
                    //                        {
                    //                            strSourceLead = row[rmEnglish.GetString("SourceLead")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strSourceLead = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("Status")) && !row.IsNull(rmChinese.GetString("Status")))
                    //                        {
                    //                            strStatus = row[rmChinese.GetString("Status")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("Status")) && !row.IsNull(rmEnglish.GetString("Status")))
                    //                        {
                    //                            strStatus = row[rmEnglish.GetString("Status")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strStatus = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("OfficePhone")) && !row.IsNull(rmChinese.GetString("OfficePhone")))
                    //                        {
                    //                            strOfficePhone = row[rmChinese.GetString("OfficePhone")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("OfficePhone")) && !row.IsNull(rmEnglish.GetString("OfficePhone")))
                    //                        {
                    //                            strOfficePhone = row[rmEnglish.GetString("OfficePhone")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strOfficePhone = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("MobilePhone")) && !row.IsNull(rmChinese.GetString("MobilePhone")))
                    //                        {
                    //                            strMobilePhone = row[rmChinese.GetString("MobilePhone")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("MobilePhone")) && !row.IsNull(rmEnglish.GetString("MobilePhone")))
                    //                        {
                    //                            strMobilePhone = row[rmEnglish.GetString("MobilePhone")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strMobilePhone = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("HomePhone")) && !row.IsNull(rmChinese.GetString("HomePhone")))
                    //                        {
                    //                            strHomePhone = row[rmChinese.GetString("HomePhone")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("HomePhone")) && !row.IsNull(rmEnglish.GetString("HomePhone")))
                    //                        {
                    //                            strHomePhone = row[rmEnglish.GetString("HomePhone")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strHomePhone = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("OtherPhone")) && !row.IsNull(rmChinese.GetString("OtherPhone")))
                    //                        {
                    //                            strOtherPhone = row[rmChinese.GetString("OtherPhone")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("OtherPhone")) && !row.IsNull(rmEnglish.GetString("OtherPhone")))
                    //                        {
                    //                            strOtherPhone = row[rmEnglish.GetString("OtherPhone")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strOtherPhone = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("WebSite")) && !row.IsNull(rmChinese.GetString("WebSite")))
                    //                        {
                    //                            strWebsite = row[rmChinese.GetString("WebSite")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("WebSite")) && !row.IsNull(rmEnglish.GetString("WebSite")))
                    //                        {
                    //                            strWebsite = row[rmEnglish.GetString("WebSite")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strWebsite = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("Email")) && !row.IsNull(rmChinese.GetString("Email")))
                    //                        {
                    //                            strEmail = row[rmChinese.GetString("Email")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("Email")) && !row.IsNull(rmEnglish.GetString("Email")))
                    //                        {
                    //                            strEmail = row[rmEnglish.GetString("Email")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strEmail = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("Fax")) && !row.IsNull(rmChinese.GetString("Fax")))
                    //                        {
                    //                            strFax = row[rmChinese.GetString("Fax")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("Fax")) && !row.IsNull(rmEnglish.GetString("Fax")))
                    //                        {
                    //                            strFax = row[rmEnglish.GetString("Fax")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strFax = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("City")) && !row.IsNull(rmChinese.GetString("City")))
                    //                        {
                    //                            strCity = row[rmChinese.GetString("City")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("City")) && !row.IsNull(rmEnglish.GetString("City")))
                    //                        {
                    //                            strCity = row[rmEnglish.GetString("City")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strCity = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("State")) && !row.IsNull(rmChinese.GetString("State")))
                    //                        {
                    //                            strState = row[rmChinese.GetString("State")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("State")) && !row.IsNull(rmEnglish.GetString("State")))
                    //                        {
                    //                            strState = row[rmEnglish.GetString("State")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strState = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("Country")) && !row.IsNull(rmChinese.GetString("Country")))
                    //                        {
                    //                            strCountry = row[rmChinese.GetString("Country")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("Country")) && !row.IsNull(rmEnglish.GetString("Country")))
                    //                        {
                    //                            strCountry = row[rmEnglish.GetString("Country")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strCountry = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("Zip")) && !row.IsNull(rmChinese.GetString("Zip")))
                    //                        {
                    //                            strZip = row[rmChinese.GetString("Zip")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("Zip")) && !row.IsNull(rmEnglish.GetString("Zip")))
                    //                        {
                    //                            strZip = row[rmEnglish.GetString("Zip")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strZip = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("Street1")) && !row.IsNull(rmChinese.GetString("Street1")))
                    //                        {
                    //                            strStrret1 = row[rmChinese.GetString("Street1")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("Street1")) && !row.IsNull(rmEnglish.GetString("Street1")))
                    //                        {
                    //                            strStrret1 = row[rmEnglish.GetString("Street1")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strStrret1 = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("Street2")) && !row.IsNull(rmChinese.GetString("Street2")))
                    //                        {
                    //                            strStreet2 = row[rmChinese.GetString("Street2")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("Street2")) && !row.IsNull(rmEnglish.GetString("Street2")))
                    //                        {
                    //                            strStreet2 = row[rmEnglish.GetString("Street2")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strStreet2 = string.Empty;
                    //                        }
                    //                        if (dt.Columns.Contains(rmChinese.GetString("PreferredContactMethod")) && !row.IsNull(rmChinese.GetString("PreferredContactMethod")))
                    //                        {
                    //                            strPreferredContactMethod = row[rmChinese.GetString("PreferredContactMethod")].ToString();

                    //                        }
                    //                        else if (dt.Columns.Contains(rmEnglish.GetString("PreferredContactMethod")) && !row.IsNull(rmEnglish.GetString("PreferredContactMethod")))
                    //                        {
                    //                            strPreferredContactMethod = row[rmEnglish.GetString("PreferredContactMethod")].ToString();

                    //                        }
                    //                        else
                    //                        {
                    //                            strPreferredContactMethod = string.Empty;
                    //                        }
                    //                        if (strTopic != null && strTopic.Length > 0 && strOwner != null && strOwner.Length > 0 && strCategory != null && strCategory.Length > 0 && strPotentialCustomer != null && strPotentialCustomer.Length > 0)
                    //                        {


                    //                            Customer customer = ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("[Name]='" + strPotentialCustomer + "'"));
                    //                            if (customer != null)
                    //                            {
                    //                                objCRMOpportunitys.Customer = customer;
                    //                            }
                    //                            else
                    //                            {
                    //                                IObjectSpace os = Application.CreateObjectSpace();
                    //                                Customer objCustom = os.CreateObject<Customer>();

                    //                                objCustom.Owner = objCRMOpportunitys.Owner;
                    //                                if (strPreferredContactMethod != null)
                    //                                {
                    //                                    if (strPreferredContactMethod == "Email")
                    //                                    {
                    //                                        objCustom.PreferredContactMethod = PreferredContactMethod.Email;
                    //                                    }
                    //                                    else if (strPreferredContactMethod == "Phone")
                    //                                    {
                    //                                        objCustom.PreferredContactMethod = PreferredContactMethod.Phone;
                    //                                    }
                    //                                    else if (strPreferredContactMethod == "Fax")
                    //                                    {
                    //                                        objCustom.PreferredContactMethod = PreferredContactMethod.Fax;
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        objCustom.PreferredContactMethod = PreferredContactMethod.Any;
                    //                                    }
                    //                                }

                    //                                objCustom.CustHomePhone = strHomePhone;
                    //                                objCustom.CustOfficePhone = strOfficePhone;
                    //                                objCustom.CustMobilePhone = strMobilePhone;
                    //                                objCustom.CustOtherPhone = strOfficePhone;
                    //                                objCustom.CustFax = strFax;
                    //                                objCustom.CustEmail = strEmail;
                    //                                objCustom.CustWebSite = strWebsite;

                    //                                objCustom.Name = strPotentialCustomer;
                    //                                objCustom.BillToCity = strCity;
                    //                                objCustom.BillToCountry = strCountry;
                    //                                objCustom.BillToState = strState;
                    //                                objCustom.BillToStreet1 = strStrret1;
                    //                                objCustom.BillToStreet2 = strStreet2;
                    //                                objCustom.BillToZip = strZip;
                    //                                os.CommitChanges();
                    //                            }

                    //                            //  if (objLeadSource != null)
                    //                            // objCRMOpportunitys.Customer = strPotentialCustomer;
                    //                            Customer customerss = ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("[Name]='" + strPotentialCustomer + "'"));
                    //                            if (customerss != null)
                    //                            {
                    //                                objCRMOpportunitys.Customer = customerss;
                    //                            }
                    //                            objCRMOpportunitys.Name = strTopic;
                    //                            objCRMOpportunitys.ClientCode = strClientCode;
                    //                            if (followUpDate != DateTime.MinValue)
                    //                            {
                    //                                objCRMOpportunitys.FollowUpDate = followUpDate;
                    //                            }
                    //                            if (demoDate != DateTime.MinValue)
                    //                            {
                    //                                objCRMOpportunitys.DemoDate = demoDate;
                    //                            }
                    //                            if (quoteDate != DateTime.MinValue)
                    //                            {
                    //                                objCRMOpportunitys.QuoteDate = quoteDate;
                    //                            }


                    //                            objCRMOpportunitys.Category = strCategory;

                    //                            objCRMOpportunitys.Probability = probablity;
                    //                            objCRMOpportunitys.Industry = strProductVersion;
                    //                            if (rating != null)
                    //                            {
                    //                                if (rating == "Hot")
                    //                                {
                    //                                    objCRMOpportunitys.Rating = OpportunityRating.Hot;
                    //                                }
                    //                                else if (rating == "Warm")
                    //                                {
                    //                                    objCRMOpportunitys.Rating = OpportunityRating.Warm;
                    //                                }
                    //                                else if (rating == "Cold")
                    //                                {
                    //                                    objCRMOpportunitys.Rating = OpportunityRating.Cold;
                    //                                }

                    //                            }
                    //                            DCUser objDCUser = ObjectSpace.FindObject<DCUser>(CriteriaOperator.Parse("[FirstName]='" + strOwner + "'"));
                    //                            if (objDCUser != null)
                    //                            {
                    //                                objCRMOpportunitys.Owner = objDCUser;
                    //                            }
                    //                            else
                    //                            {
                    //                                objCRMOpportunitys.Owner = null;
                    //                            }


                    //                            if (strStatus != null)
                    //                            {
                    //                                if (strStatus == "Won")
                    //                                {
                    //                                    objCRMOpportunitys.Status = OpportunityStatus.Won;
                    //                                }
                    //                                else if (strStatus == "Canceled")
                    //                                {
                    //                                    objCRMOpportunitys.Status = OpportunityStatus.Canceled;
                    //                                }
                    //                                //else if (strStatus == "OutSold")
                    //                                //{
                    //                                //    objCRMOpportunitys.Status = OpportunityStatus.OutSold;
                    //                                //}
                    //                                else
                    //                                {
                    //                                    objCRMOpportunitys.Status = OpportunityStatus.None;
                    //                                }
                    //                            }
                    //                            ObjectSpace.CommitChanges();


                    //                            if (objCRMOpportunitys.Status == OpportunityStatus.Won)
                    //                            {
                    //                                string strID;
                    //                                if (objCRMOpportunitys.Owner != null)
                    //                                {
                    //                                    strID = objCRMOpportunitys.Owner.Oid.ToString();
                    //                                }
                    //                                else
                    //                                {
                    //                                    strID = strCurrentUserId;
                    //                                }
                    //                                if (objCRMOpportunitys.Customer != null)
                    //                                {
                    //                                    Customer objCustomer = ObjectSpace.GetObject<Customer>(objCRMOpportunitys.Customer);
                    //                                    CRMAccount findAccount = ObjectSpace.FindObject<CRMAccount>(CriteriaOperator.Parse("[AccountName]='" + objCustomer.Name + "'"));


                    //                                    if (findAccount == null)
                    //                                    {
                    //                                        IObjectSpace accountObjectSpace = Application.CreateObjectSpace(typeof(CRMAccount));
                    //                                        CRMAccount account = ObjectSpace.CreateObject<CRMAccount>();

                    //                                        if (objCustomer != null)
                    //                                        {
                    //                                            account.AccountName = objCustomer.Name;
                    //                                            //account.SICCode = objCustomer.SICCode;
                    //                                            //account.NumberOfEmployees = objCustomer.NumberOfEmployees;
                    //                                            account.WebSite = objCustomer.CustWebSite;
                    //                                            account.BillToStreet1 = objCustomer.BillToStreet1;
                    //                                            account.BillToStreet2 = objCustomer.BillToStreet2;
                    //                                            account.BillToCity = objCustomer.BillToCity;
                    //                                            account.BillToState = objCustomer.BillToState;
                    //                                            account.BillToCountry = objCustomer.BillToCountry;
                    //                                            account.BillToZip = objCustomer.BillToZip;
                    //                                            account.ShipToStreet1 = objCustomer.BillToStreet1;
                    //                                            account.ShipToStreet2 = objCustomer.BillToStreet2;
                    //                                            account.ShipToCity = objCustomer.BillToCity;
                    //                                            account.ShipToState = objCustomer.BillToState;
                    //                                            account.ShipToCountry = objCustomer.BillToCountry;
                    //                                            account.ShipToZip = objCustomer.BillToZip;
                    //                                            account.OfficePhone = objCustomer.CustOfficePhone;
                    //                                            account.OtherPhone = objCustomer.CustOtherPhone;
                    //                                            account.MobilePhone = objCustomer.CustMobilePhone;
                    //                                            account.HomePhone = objCustomer.CustHomePhone;
                    //                                            account.Email = objCustomer.CustEmail;
                    //                                            account.Fax = objCustomer.CustFax;
                    //                                            account.Owner = objCustomer.Owner;
                    //                                            account.PreferredContactMethod = objCustomer.PreferredContactMethod;
                    //                                            account.SourceLead = objCRMOpportunitys.SourceLead;
                    //                                            account.Category = objCRMOpportunitys.Category;
                    //                                            account.Industry = objCRMOpportunitys.Industry;
                    //                                            account.ClientCode = objCRMOpportunitys.ClientCode;
                    //                                            Frame.GetController<EmailController>().CheckUserEmailPermission(strID, "New Account Notification", objCRMOpportunitys, "Opportunity to Account");
                    //                                            ObjectSpace.CommitChanges();
                    //                                        }
                    //                                    }

                    //                                    CRMContact findContact = ObjectSpace.FindObject<CRMContact>(CriteriaOperator.Parse("[FirstName]='" + objCustomer.Name + "'"));
                    //                                    if (findContact == null)
                    //                                    {
                    //                                        IObjectSpace contacttObjectSpace = Application.CreateObjectSpace(typeof(CRMContact));
                    //                                        CRMContact contact = ObjectSpace.CreateObject<CRMContact>();

                    //                                        contact.FirstName = objCustomer.Name;
                    //                                        contact.LastName = objCustomer.Name;
                    //                                        contact.OfficePhone = objCustomer.CustOfficePhone;
                    //                                        contact.MobilePhone = objCustomer.CustMobilePhone;
                    //                                        contact.OtherPhone = objCustomer.CustOtherPhone;
                    //                                        contact.HomePhone = objCustomer.CustHomePhone;
                    //                                        contact.Email = objCustomer.CustEmail;
                    //                                        contact.Fax = objCustomer.CustFax;
                    //                                        contact.BillToStreet1 = objCustomer.BillToStreet1;
                    //                                        contact.BillToStreet2 = objCustomer.BillToStreet2;
                    //                                        contact.BillToCity = objCustomer.BillToCity;
                    //                                        contact.BillToState = objCustomer.BillToState;
                    //                                        contact.BillToCountry = objCustomer.BillToCountry;
                    //                                        contact.BillToZip = objCustomer.BillToZip;
                    //                                        contact.ShipToStreet1 = objCustomer.BillToStreet1;
                    //                                        contact.ShipToStreet2 = objCustomer.BillToStreet2;
                    //                                        contact.ShipToCity = objCustomer.BillToCity;
                    //                                        contact.ShipToState = objCustomer.BillToState;
                    //                                        contact.ShipToCountry = objCustomer.BillToCountry;
                    //                                        contact.ShipToZip = objCustomer.BillToZip;
                    //                                        contact.Owner = objCustomer.Owner;
                    //                                        contact.Status = CustomerStatus.Active;
                    //                                        //contact.JobTitle = objCustomer.JobTitle;
                    //                                        contact.PreferredContactMethod = objCustomer.PreferredContactMethod;
                    //                                        contact.SourceLead = objCRMOpportunitys.SourceLead;
                    //                                        CRMAccount contactAccount = ObjectSpace.FindObject<CRMAccount>(CriteriaOperator.Parse("[AccountName]='" + objCustomer.Name + "'"));
                    //                                        if (contactAccount != null)
                    //                                        {
                    //                                            contact.CRMAccount = contactAccount;
                    //                                            contactAccount.PrimaryContact = contact;
                    //                                        }
                    //                                        ObjectSpace.CommitChanges();
                    //                                    }
                    //                                }

                    //                                else
                    //                                {
                    //                                    Customer customers = ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("[Name]='" + strPotentialCustomer + "'"));
                    //                                    if (customers != null)
                    //                                    {
                    //                                        objCRMOpportunitys.Customer = customers;
                    //                                    }
                    //                                    ObjectSpace.CommitChanges();
                    //                                    Customer objCustomer = ObjectSpace.GetObject<Customer>(objCRMOpportunitys.Customer);
                    //                                    CRMAccount findAccount = ObjectSpace.FindObject<CRMAccount>(CriteriaOperator.Parse("[AccountName]='" + objCustomer.Name + "'"));
                    //                                    if (findAccount == null)
                    //                                    {
                    //                                        IObjectSpace accountObjectSpace = Application.CreateObjectSpace(typeof(CRMAccount));
                    //                                        CRMAccount account = ObjectSpace.CreateObject<CRMAccount>();

                    //                                        if (objCustomer != null)
                    //                                        {
                    //                                            account.AccountName = objCustomer.Name;
                    //                                            //account.SICCode = objCustomer.SICCode;
                    //                                            //account.NumberOfEmployees = objCustomer.NumberOfEmployees;
                    //                                            account.WebSite = objCustomer.CustWebSite;
                    //                                            account.BillToStreet1 = objCustomer.BillToStreet1;
                    //                                            account.BillToStreet2 = objCustomer.BillToStreet2;
                    //                                            account.BillToCity = objCustomer.BillToCity;
                    //                                            account.BillToState = objCustomer.BillToState;
                    //                                            account.BillToCountry = objCustomer.BillToCountry;
                    //                                            account.BillToZip = objCustomer.BillToZip;
                    //                                            account.ShipToStreet1 = objCustomer.BillToStreet1;
                    //                                            account.ShipToStreet2 = objCustomer.BillToStreet2;
                    //                                            account.ShipToCity = objCustomer.BillToCity;
                    //                                            account.ShipToState = objCustomer.BillToState;
                    //                                            account.ShipToCountry = objCustomer.BillToCountry;
                    //                                            account.ShipToZip = objCustomer.BillToZip;
                    //                                            account.OfficePhone = objCustomer.CustOfficePhone;
                    //                                            account.OtherPhone = objCustomer.CustOtherPhone;
                    //                                            account.MobilePhone = objCustomer.CustMobilePhone;
                    //                                            account.HomePhone = objCustomer.CustHomePhone;
                    //                                            account.Email = objCustomer.CustEmail;
                    //                                            account.Fax = objCustomer.CustFax;
                    //                                            account.Owner = objCustomer.Owner;
                    //                                            account.PreferredContactMethod = objCustomer.PreferredContactMethod;
                    //                                            account.SourceLead = objCRMOpportunitys.SourceLead;
                    //                                            account.Category = objCRMOpportunitys.Category;
                    //                                            account.Industry = objCRMOpportunitys.Industry;
                    //                                            account.ClientCode = objCRMOpportunitys.ClientCode;
                    //                                            ObjectSpace.CommitChanges();
                    //                                        }


                    //                                    }
                    //                                    CRMContact findContact = ObjectSpace.FindObject<CRMContact>(CriteriaOperator.Parse("[FirstName]='" + objCustomer.Name + "'"));
                    //                                    if (findContact == null)
                    //                                    {
                    //                                        IObjectSpace contacttObjectSpace = Application.CreateObjectSpace(typeof(CRMContact));
                    //                                        CRMContact contact = ObjectSpace.CreateObject<CRMContact>();

                    //                                        contact.FirstName = objCustomer.Name;
                    //                                        contact.LastName = objCustomer.Name;
                    //                                        contact.OfficePhone = objCustomer.CustOfficePhone;
                    //                                        contact.MobilePhone = objCustomer.CustMobilePhone;
                    //                                        contact.OtherPhone = objCustomer.CustOtherPhone;
                    //                                        contact.HomePhone = objCustomer.CustHomePhone;
                    //                                        contact.Email = objCustomer.CustEmail;
                    //                                        contact.Fax = objCustomer.CustFax;
                    //                                        contact.BillToStreet1 = objCustomer.BillToStreet1;
                    //                                        contact.BillToStreet2 = objCustomer.BillToStreet2;
                    //                                        contact.BillToCity = objCustomer.BillToCity;
                    //                                        contact.BillToState = objCustomer.BillToState;
                    //                                        contact.BillToCountry = objCustomer.BillToCountry;
                    //                                        contact.BillToZip = objCustomer.BillToZip;
                    //                                        contact.ShipToStreet1 = objCustomer.BillToStreet1;
                    //                                        contact.ShipToStreet2 = objCustomer.BillToStreet2;
                    //                                        contact.ShipToCity = objCustomer.BillToCity;
                    //                                        contact.ShipToState = objCustomer.BillToState;
                    //                                        contact.ShipToCountry = objCustomer.BillToCountry;
                    //                                        contact.ShipToZip = objCustomer.BillToZip;
                    //                                        contact.Owner = objCustomer.Owner;
                    //                                        contact.Status = CustomerStatus.Active;
                    //                                        //contact.JobTitle = objCustomer.JobTitle;
                    //                                        contact.PreferredContactMethod = objCustomer.PreferredContactMethod;
                    //                                        contact.SourceLead = objCRMOpportunitys.SourceLead;
                    //                                        CRMAccount contactAccount = ObjectSpace.FindObject<CRMAccount>(CriteriaOperator.Parse("[AccountName]='" + objCustomer.Name + "'"));
                    //                                        if (contactAccount != null)
                    //                                        {
                    //                                            contact.CRMAccount = contactAccount;
                    //                                            contactAccount.PrimaryContact = contact;
                    //                                        }
                    //                                        ObjectSpace.CommitChanges();
                    //                                    }

                    //                                }
                    //                            }




                    //                        }
                    //                        else
                    //                        {
                    //                            if (string.IsNullOrEmpty(strCategory))
                    //                            {
                    //                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages/Lead", "Category"), InformationType.Error, 3000, InformationPosition.Top);
                    //                            }
                    //                            else
                    //                            if (string.IsNullOrEmpty(strTopic))
                    //                            {
                    //                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages/Lead", "Topic"), InformationType.Error, 3000, InformationPosition.Top);
                    //                            }
                    //                            else
                    //                            if (string.IsNullOrEmpty(strOwner))
                    //                            {
                    //                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages/Lead", "Owner"), InformationType.Error, 3000, InformationPosition.Top);
                    //                            }
                    //                            else
                    //                            if (string.IsNullOrEmpty(strPotentialCustomer))
                    //                            {
                    //                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages/Lead", "PotentialCustomer"), InformationType.Error, 3000, InformationPosition.Top);
                    //                            }
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages/Lead", "Topics"), InformationType.Error, 3000, InformationPosition.Top);
                    //                    }
                    //                }
                    //            }

                    //        }
                    //        conn.Close();
                    //    }

                    //}

                }
         ((ListView)View).Refresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }

        private void closeProspects_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                CRMProspects objCRMProspects = (CRMProspects)e.PopupWindowViewCurrentObject;
                Customer objCustomer = ObjectSpace.GetObject<Customer>(objCRMProspects.Customer);
                if (objCRMProspects != null)
                {
                    //objCRMProspects.IsClose = true;
                    string strID;
                    if (objCRMProspects.Owner != null)
                    {
                        strID = objCRMProspects.Owner.Oid.ToString();
                    }
                    else
                    {
                        strID = strCurrentUserId;
                    }
                    Frame.GetController<EmailController>().CheckUserEmailPermission(strID, "Prospects Created", objCRMProspects, "");
                    if (objCRMProspects.Status == ProspectsStatus.Won)
                    {
                        if (objCustomer != null && objCustomer.Account != null)
                        {

                        }
                    }
                    if (objCRMProspects.PrimaryContact != null)
                    {
                        Contact objContact = objCRMProspects.Session.FindObject<Contact>(CriteriaOperator.Parse("[Oid]=?", objCRMProspects.PrimaryContact.Oid));
                        if (objContact != null)
                        {
                            objContact.Prospects = objCRMProspects;
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

        private void ReactivateProspects_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {

                CRMProspects objProspects = (CRMProspects)View.CurrentObject;
                //Contact objContact = objProspects.Session.FindObject<Contact>(CriteriaOperator.Parse("[Oid]=?", objProspects.PrimaryContact.Oid));

                Employee objUser = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                if (objProspects != null)
                {
                    if (objProspects.Owner == objUser || objUser.IsActive == true)
                    {

                        objProspects.Status = ProspectsStatus.None;
                        //objProspects.IsClose = false;
                        objProspects.CloseDate = null;
                        //objProspects.PrimaryContact = null;                        
                        ObjectSpace.CommitChanges();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages/LDMMessages", "reactivatesuccess"), InformationType.Success, 3000, InformationPosition.Top);
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages", "CannotReactivateProspects"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
                View.ObjectSpace.Refresh();





            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Rollback_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objspa = Application.CreateObjectSpace();
                CRMProspects objProspects = (CRMProspects)View.CurrentObject;
                Customer account = ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("[CustomerName]=?", objProspects.Name));
                if (objProspects != null)
                {
                    objProspects.Status = ProspectsStatus.None;

                    if (account != null)
                    {
                        ObjectSpace.Delete(account);

                    }
                    if (objProspects.PrimaryContact != null)
                    {
                        Contact objContact = ObjectSpace.FindObject<Contact>(CriteriaOperator.Parse("[Oid]=?", objProspects.PrimaryContact.Oid));
                        if (objContact != null)
                        {
                            ObjectSpace.Delete(objContact);

                        }
                    }
                    if (objProspects.Notes != null && account != null)
                    {
                        Notes objNots = ObjectSpace.FindObject<Notes>(CriteriaOperator.Parse("[Customer]=?", account.Oid));
                        if (objNots != null)
                        {
                            ObjectSpace.Delete(objNots);

                        }
                    }


                }

                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages", "Cannot Rollback"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }

                ObjectSpace.CommitChanges();
                View.ObjectSpace.Refresh();
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages/LDMMessages", "Rollbacksuccess"), InformationType.Success, 3000, InformationPosition.Top);
            }

            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }

}
