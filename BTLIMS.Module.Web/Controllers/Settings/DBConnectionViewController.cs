using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Data.SqlClient;

namespace LDM.Module.Web.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DBConnectionViewController : ViewController
    {
        DBConnectioninfo dbconinfo = new DBConnectioninfo();
        MessageTimer timer = new MessageTimer();
        public DBConnectionViewController()
        {
            InitializeComponent();
            TargetViewId = "DBConnection_DetailView;";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void ConnectionTest_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (e.SelectedObjects.Count == 1)
                {
                    DBConnection dBConnection = (DBConnection)View.CurrentObject;
                    if (dBConnection != null)
                    {
                        string strConnectionString = "Data Source=" + dBConnection.ServerName.Trim() + ";Initial Catalog=" + dBConnection.DataBaseName.Trim() + ";User ID=" + dBConnection.UserName.Trim() + ";Password=" + dBConnection.Password.Trim() + ";Integrated Security=False;"; ;
                        SqlConnection newCon = new SqlConnection();
                        newCon.ConnectionString = strConnectionString; // m_con.ConnectionString;
                        try
                        {
                            newCon.Open();
                            newCon.Close();
                            Application.ShowViewStrategy.ShowMessage("DB connection testing successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                        catch (SqlException ex)
                        {
                            throw (ex);
                            //Application.ShowViewStrategy.ShowMessage("DB connection testing successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void btnpasswordshown_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                //if(View != null && View is DetailView && View.ObjectTypeInfo.Type == typeof(DBConnection))
                //{
                //    foreach(ViewItem item in ((DetailView)View).Items.Where(i =>  i.Id =="Password"))
                //    {
                //        ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                //        if (propertyEditor != null && propertyEditor.Editor != null)
                //        {
                //            if(propertyEditor.IsPassword)
                //            {
                //                propertyEditor.IsPassword = false;
                //            }
                //            else
                //            {
                //                propertyEditor.IsPassword = true;
                //            }
                //        }
                //    }
                //    View.Refresh();
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void btnNew_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
