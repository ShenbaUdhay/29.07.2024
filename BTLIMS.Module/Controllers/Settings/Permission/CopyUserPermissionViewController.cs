using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace LDM.Module.Controllers.Settings.Permission
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CopyUserPermissionViewController : ViewController
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        CopyPermissioninfo objCopyUserPermissionInfo = new CopyPermissioninfo();
        #endregion
        public CopyUserPermissionViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(CopyPermission);
            // SelectMultipleUser.TargetViewId=""
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            //if (View != null && View.CurrentObject != null)
            //{
            //    CopyPermission obj = (CopyPermission)View.CurrentObject;
            //    if (obj != null && obj.User !=null)
            //    {
            //        objCopyUserPermissionInfo.SelectedUser = obj.User.UserName;
            //    }
            //}
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            try
            {
                // Unsubscribe from previously subscribed events and release other references and resources.
                base.OnDeactivated();
                ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void objectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View != null && View.CurrentObject == e.Object && e.PropertyName == "User")
                {
                    if (View.ObjectTypeInfo.Type == typeof(CopyPermission))
                    {
                        CopyPermission ObjCopyNoOfSamples = (CopyPermission)e.Object;
                        //objCopyUserPermissionInfo.SelectedUser = ObjCopyNoOfSamples.User;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SelectMultipleUser_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                object objToShow = objspace.CreateObject(typeof(CustomSystemUser));
                List<Guid> UniqueObjects = new List<Guid>();
                if (objToShow != null)
                {

                    //CollectionSource cs = new CollectionSource(objspace, typeof(CustomSystemUser));
                    IList<UserNavigationPermission> us = ObjectSpace.GetObjects<UserNavigationPermission>();
                    foreach (UserNavigationPermission usd in us)
                    {
                        UniqueObjects.Add(usd.User.Oid);
                    }


                    CriteriaOperator css = !new InOperator("Oid", UniqueObjects);
                    CollectionSource cs = new CollectionSource(objspace, typeof(CustomSystemUser));
                    cs.Criteria["filter"] = css;
                    //cs.Criteria.Clear();
                    ListView CreateListView = Application.CreateListView("CustomSystemUser_LookupListView_Copy_NavigationPermission", cs, false);
                    e.Size = new Size(750, 500);
                    e.View = CreateListView;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SelectMultipleUser_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                objCopyUserPermissionInfo.lstSelectedUser = new List<string>();
                if (e.PopupWindowViewSelectedObjects.Count > 0)
                {
                    foreach (CustomSystemUser objCS in e.PopupWindowViewSelectedObjects)
                    {
                        Employee obj = (Employee)objCS;
                        if (objCopyUserPermissionInfo.lstSelectedUser != null && !objCopyUserPermissionInfo.lstSelectedUser.Contains(obj.UserName))
                        {
                            objCopyUserPermissionInfo.lstSelectedUser.Add(obj.UserName);
                        }
                    }
                }
                if (View != null && View.CurrentObject != null && View.Id == "CopyPermission_DetailView")
                {
                    CopyPermission obj = (CopyPermission)View.CurrentObject;
                    if (objCopyUserPermissionInfo.lstSelectedUser != null && objCopyUserPermissionInfo.lstSelectedUser.Count > 0)
                    {
                        obj.User = string.Join(",", objCopyUserPermissionInfo.lstSelectedUser);
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
