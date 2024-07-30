using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Notifications;
using DevExpress.Persistent.Base.General;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using System;

public class DeleteOnDismissController : ObjectViewController<DetailView, NotificationsObject>
{
    MessageTimer timer = new MessageTimer();

    private NotificationsService service;
    protected override void OnActivated()
    {
        try
        {
            base.OnActivated();
            service = Application.Modules.FindModule<NotificationsModule>().NotificationsService;
            NotificationsDialogViewController notificationsDialogViewController =
            Frame.GetController<NotificationsDialogViewController>();
            if (service != null && notificationsDialogViewController != null)
            {
                notificationsDialogViewController.Dismiss.Executing += Dismiss_Executing;
                notificationsDialogViewController.Dismiss.Executed += Dismiss_Executed;
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
            NotificationsDialogViewController notificationsDialogViewController =
            Frame.GetController<NotificationsDialogViewController>();
            if (notificationsDialogViewController != null)
            {
                notificationsDialogViewController.Dismiss.Executing -= Dismiss_Executing;
                notificationsDialogViewController.Dismiss.Executed -= Dismiss_Executed;
            }
            base.OnDeactivated();
        }
        catch (Exception ex)
        {
            Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        }
    }
    private void Dismiss_Executing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        service.ItemsProcessed += Service_ItemsProcessed;
    }
    private void Service_ItemsProcessed(object sender, DevExpress.Persistent.Base.General.NotificationItemsEventArgs e)
    {
        try
        {
            IObjectSpace space = Application.CreateObjectSpace(typeof(ICMAlert));
            foreach (INotificationItem item in e.NotificationItems)
            {
                if (item.NotificationSource is ICMAlert)
                {
                    space.Delete(space.GetObject(item.NotificationSource));
                }
            }
            space.CommitChanges();
        }
        catch (Exception ex)
        {
            Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        }
    }
    private void Dismiss_Executed(object sender, DevExpress.ExpressApp.Actions.ActionBaseEventArgs e)
    {
        try
        {
            service.ItemsProcessed -= Service_ItemsProcessed;
        }
        catch (Exception ex)
        {
            Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        }
    }
}