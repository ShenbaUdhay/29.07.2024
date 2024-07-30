using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Web;
using DevExpress.Persistent.Base;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Configuration;

public class WebDashboardCustomizeController : ObjectViewController<DetailView, IDashboardData>//<DetailView>
{
    MessageTimer timer = new MessageTimer();
    #region Constructor
    public WebDashboardCustomizeController()
    {
        // TargetObjectType = typeof(IDashboardData);
        TargetViewId = "DashboardViewer_DetailView;" + "DashboardData_ListView;";
    }
    #endregion

    #region DefaultEvents
    protected override void OnActivated()
    {
        base.OnActivated();
        try
        {
            if (View != null && View is DetailView)
            {
                WebDashboardViewerViewItem dashboardViewerViewItem =
                  View.FindItem("DashboardViewer") as WebDashboardViewerViewItem;
                if (dashboardViewerViewItem != null)
                {
                    if (dashboardViewerViewItem.DashboardControl != null)
                    {
                        SetHeight(dashboardViewerViewItem.DashboardControl);
                    }
                    dashboardViewerViewItem.ControlCreated += DashboardViewerViewItem_ControlCreated;
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
        base.OnDeactivated();
        try
        {
            WebDashboardViewerViewItem dashboardViewerViewItem =
       View.FindItem("DashboardViewer") as WebDashboardViewerViewItem;
            if (dashboardViewerViewItem != null)
            {
                dashboardViewerViewItem.ControlCreated -= DashboardViewerViewItem_ControlCreated;
            }
        }
        catch (Exception ex)
        {
            Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        }


    }
    #endregion

    #region Function
    private void SetHeight(ASPxDashboard dashboardControl)
    {
        try
        {
            {
                dashboardControl.Height = 760;
                dashboardControl.SetConnectionStringsProvider(new DevExpress.DataAccess.Web.ConfigFileConnectionStringsProvider());
            }
        }
        catch (Exception ex)
        {
            Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        }
    }
    private void DashboardDesigner_ConfigureDataConnection(object sender, DashboardConfigureDataConnectionEventArgs e)
    {
        try
        {
            ConfigureDataConnection(e);
        }
        catch (Exception ex)
        {
            Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        }
    }
    private void Viewer_ConfigureDataConnection(object sender, DashboardConfigureDataConnectionEventArgs e)
    {
        try
        {
            ConfigureDataConnection(e);
        }
        catch (Exception ex)
        {
            Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        }
    }
    private void ConfigureDataConnection(DashboardConfigureDataConnectionEventArgs e)
    {
        try
        {
            if (e.ConnectionName != string.Empty)
            {
                e.ConnectionParameters = new MsSqlConnectionParameters();
                SetupSqlParameters((MsSqlConnectionParameters)e.ConnectionParameters);
            }
        }
        catch (Exception ex)
        {
            Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        }
    }
    public static void SetupSqlParameters(MsSqlConnectionParameters connectionParameters)
    {
        try
        {
            string strConnection = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string[] strConnect = strConnection.Split(';');
            string strServerName = string.Empty;
            string strDBName = string.Empty;
            string strUsername = string.Empty;
            string strPassword = string.Empty;
            //ServerName
            if (strConnect[0].Length > 0)
            {
                string[] strName = strConnect[0].Split('=');
                if (strName[1].Length > 0)
                {
                    strServerName = strName[1].Trim();
                }
            }
            //DataBaseName
            if (strConnect[1].Length > 0)
            {
                string[] strDataBaseName = strConnect[1].Split('=');
                if (strDataBaseName[1].Length > 0)
                {
                    strDBName = strDataBaseName[1].Trim();
                }
            }
            //User Name
            if (strConnect[3].Length > 0)
            {
                string[] strUserName = strConnect[2].Split('=');
                if (strUserName[1].Length > 0)
                {
                    strUsername = strUserName[1].Trim();
                }

            }
            //PassWor
            if (strConnect[4].Length > 0)
            {
                string[] strPass = strConnect[3].Split('=');
                if (strPass[1].Length > 0)
                {
                    strPassword = strPass[1].Trim();
                }
            }
            connectionParameters.DatabaseName = strDBName;
            connectionParameters.ServerName = strServerName;
            connectionParameters.UserName = strUsername;
            connectionParameters.Password = strPassword;
            connectionParameters.AuthorizationType = MsSqlAuthorizationType.SqlServer;
        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region Events
    private void DashboardViewerViewItem_ControlCreated(object sender, EventArgs e)
    {
        try
        {
            if (View != null && View is DetailView)
            {
                WebDashboardViewerViewItem dashboardViewerViewItem = sender as WebDashboardViewerViewItem;
                SetHeight(dashboardViewerViewItem.DashboardControl);
                dashboardViewerViewItem.DashboardControl.ConfigureDataConnection += Viewer_ConfigureDataConnection;
            }
        }
        catch (Exception ex)
        {
            Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        }
    }
    #endregion
}