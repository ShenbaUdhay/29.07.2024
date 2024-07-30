using BTLIMS.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Controllers.Public
{
    public partial class NavigationViewHideController : WindowController
    {
        private ShowNavigationItemController navigationController;
        NavigationItemsInfo navigationItemsInfo = new NavigationItemsInfo();
        List<DefaultSetting> lstdefsetting;
        MessageTimer timer = new MessageTimer();
        public NavigationViewHideController()
        {
            InitializeComponent();
            TargetWindowType = WindowType.Main;
        }
        protected override void OnFrameAssigned()
        {
            try
            {
                base.OnFrameAssigned();
                navigationController = Frame.GetController<ShowNavigationItemController>();
                if (navigationController != null)
                {
                    navigationController.ItemsInitialized += new EventHandler<EventArgs>(HideItemWindowController_ItemsInitialized);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void HideItemWindowController_ItemsInitialized(object sender, EventArgs e)
        {
            try
            {
                FuncCurrentUserIsAdministrative obj = new FuncCurrentUserIsAdministrative();
                List<string> allNavigationItems = new List<string>();
                List<string> VisibleNavigationItems = new List<string>();
                navigationItemsInfo.InVisibleNavigationItems = new List<string>();
                lstdefsetting = new List<DefaultSetting>();
                object val = obj.Evaluate();
                if ((string)val == "0")
                {
                    navigationItemsInfo.UserIsAdministrator = false;
                    if (SecuritySystem.CurrentUser != null)
                    {
                        IObjectSpace objectspace = Application.CreateObjectSpace();
                        XPCollection<PermissionPolicyRole> currentUserRoles = ((Employee)SecuritySystem.CurrentUser).Roles;
                        allNavigationItems = objectspace.GetObjects<NavigationItem>().Cast<NavigationItem>().Select(i => i.NavigationId).Distinct().ToList();
                        foreach (PermissionPolicyRole currentUserRole in currentUserRoles)
                        {
                            CustomSystemRole role = objectspace.GetObjectByKey<CustomSystemRole>(currentUserRole.Oid);
                            if (role != null && role.RoleNavigationPermission != null && !objectspace.IsDeletedObject(role.RoleNavigationPermission) && role.RoleNavigationPermission.RoleNavigationPermissionDetails.Count > 0)
                            {
                                List<string> tempVisibleNavigationItems = role.RoleNavigationPermission.RoleNavigationPermissionDetails.Where(i => i.Navigate == true && i.NavigationItem != null).AsEnumerable().Select(i => i.NavigationItem.NavigationId).Distinct().ToList();
                                //VisibleNavigationItems = tempVisibleNavigationItems.Except(VisibleNavigationItems).ToList();
                                foreach (string strnavigation in tempVisibleNavigationItems)
                                {
                                    if (!VisibleNavigationItems.Contains(strnavigation))
                                    {
                                        VisibleNavigationItems.Add(strnavigation);
                                    }
                                }
                            }
                        }
                        navigationItemsInfo.InVisibleNavigationItems = allNavigationItems.Except(VisibleNavigationItems).ToList();
                    }
                    HideItemByCaption(navigationController.ShowNavigationItemAction.Items);
                }
                else
                {
                    navigationItemsInfo.UserIsAdministrator = true;
                    //IObjectSpace os = Application.CreateObjectSpace();
                    //Session currentSession = ((XPObjectSpace)(os)).Session;
                    //UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    //IList<WorkflowConfig> ICMWorkFlow = uow.GetObjects(uow.GetClassInfo(typeof(WorkflowConfig)), CriteriaOperator.Parse("GCRecord ISW Null && [ActivationOn] = 1"), new SortingCollection(), 0, 0, false, true).Cast<WorkflowConfig>().ToList();
                    //if (ICMWorkFlow.FirstOrDefault(i => i.ActivationOn == true) != null)
                    //{
                    //    RequisitionApproval = true;
                    //}
                    //OrderingItemSetup ICMOrderingItem = (OrderingItemSetup)uow.FindObject<OrderingItemSetup>(CriteriaOperator.Parse("GCRecord IS Null"));
                    //if (ICMOrderingItem.OrderingItemon == true)
                    //{
                    //    OrderingItems = true;
                    //}
                    HideItemByCaption(navigationController.ShowNavigationItemAction.Items);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void HideItemByCaption(ChoiceActionItemCollection items)
        {
            try
            {
                if (lstdefsetting.Count == 0)
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    Session currentSession = ((XPObjectSpace)(os)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    lstdefsetting = uow.GetObjects(uow.GetClassInfo(typeof(DefaultSetting)), null, null, 0, 0, false, true).Cast<DefaultSetting>().ToList();
                }
                foreach (ChoiceActionItem item in items.Where(i => i.Active == true))
                {
                    if (item.Items.Count > 0)
                    {
                        HideItemByCaption(item.Items);
                    }
                    string itemid = item.Id.Trim();
                    IList<DefaultSetting> lstsetting = lstdefsetting.Where(a => a.NavigationItemNameID == itemid).ToList();
                    if (lstsetting != null && lstsetting.Count > 0)
                    {
                        foreach (DefaultSetting objdefset in lstsetting)
                        {
                            if (objdefset != null && objdefset.NavigationItemNameID == "LabwareSettings")
                            {
                                objdefset.NavigationCaption = "Instrument Management";
                            }
                            if (objdefset != null && objdefset.Select == false)
                            {
                                item.Active["HideMyDesktop"] = false;
                                continue;
                            }
                            else if (objdefset != null && !string.IsNullOrEmpty(objdefset.NavigationCaption))
                            {
                                item.Caption = objdefset.NavigationCaption;
                            }
                            //else if (objdefset != null && !string.IsNullOrEmpty(objdefset.NavigationItemName) && string.IsNullOrEmpty(objdefset.NavigationCaption))
                            //{
                            //    item.Caption = objdefset.NavigationItemName;
                            //}
                        }
                    }

                    if (!navigationItemsInfo.UserIsAdministrator && navigationItemsInfo.InVisibleNavigationItems != null)
                    {
                        if (navigationItemsInfo.InVisibleNavigationItems.Contains(item.Id))
                        {
                            item.Active["DisableNavigation"] = false;
                            continue;
                        }
                    }

                    if (item.Id == "RequisitionApproval")
                    {
                        DefaultSetting objDefaultSetting = lstdefsetting.Where(a => a.ReportApproval == true && a.ModuleName == "Inventory Management").FirstOrDefault();
                        if (objDefaultSetting == null)
                        {
                            item.Active["HideRequisitionApproval"] = false;
                            continue;
                        }
                    }

                    if ((item.Id == "NavigationSetting" || item.Id == "ConcurrentUsers") && SecuritySystem.CurrentUserName != "Service")
                    {
                        item.Active["HideNavigationSetting"] = false;
                        continue;
                    }

                    if (item.Id == "Job ID Format" && SecuritySystem.CurrentUserName != "Service")
                    {
                        item.Active["HideNavigationSetting"] = false;
                        continue;
                    }
                    if (item.Id == "SampleSourceSetup" && SecuritySystem.CurrentUserName != "Service")
                    {
                        item.Active["HideNavigationSetting"] = false;
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}
