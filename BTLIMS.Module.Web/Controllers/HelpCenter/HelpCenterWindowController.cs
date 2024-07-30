using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Skins;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class HelpCenterWindowController : WindowController
    {
        MessageTimer timer = new MessageTimer();
        HelpCenterInfo hcInfo = new HelpCenterInfo();
        public HelpCenterWindowController()
        {
            InitializeComponent();
            // Target required Windows (via the TargetXXX properties) and create their Actions.
            TargetWindowType = WindowType.Main;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target Window.
            //HelpCenterAction.Active["ShowHelpCenterAction"] = false;
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void HelpCenterAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                #region ObjectSpace
                IObjectSpace os = Application.CreateObjectSpace();
                objectspaceinfo objectspaceinfo = new objectspaceinfo();
                objectspaceinfo.tempobjspace = os;
                objectspaceinfo.tempFrame = Frame;
                objectspaceinfo.tempapplication = Application;
                #endregion

                if (SecuritySystem.CurrentUser != null)
                {
                    Employee objemployee = (Employee)SecuritySystem.CurrentUser;
                    if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\")) == false)
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\"));
                    }
                    string strLocalFile = HttpContext.Current.Server.MapPath((@"~\HPCDetails.txt"));
                    //if (File.Exists(strLocalFile))
                    {
                        string url = HttpContext.Current.Request.Url.AbsoluteUri;
                        string strHCdetails = objemployee.DisplayName + "|" + objemployee.Oid.ToString() + "|" + BaseXafPage.CurrentTheme.ToString() + "|" + url + "|" + "HCP";
                        File.WriteAllText(strLocalFile, strHCdetails);
                    }
                }

                hcInfo.SelectedIndex = 0;
                hcInfo.strcrtTheme = BaseXafPage.CurrentTheme.ToString();
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objectSpace, typeof(HelpCenter));
                //DashboardView dv = Application.CreateDashboardView(Application.CreateObjectSpace(), "HelpCenter_Copy", true);
                //DashboardView dv = Application.CreateDashboardView(Application.CreateObjectSpace(), "HelpCenter_Copy", true);
                //DashboardView dv = Application.CreateDashboardView(Application.CreateObjectSpace(), "HelpCenter_Copy", false);
                //Frame.SetView(dv);
                //string viewUrl = ((WebApplication)Application).ViewUrlManager.GetUrl(dv.CreateShortcut());
                //string viewUrl = "https://btsoftproducts.app/AlpacaHelpcenter/Default.aspx?ViewID=HelpCenter_ListView_Articles"; ///*/*"http://localhost:2064/Default.aspx?ViewID=HelpCenter_ListView_Articles";//*/*/
                //string viewUrl = "https://btsoftproducts.com/AlpacaHelpcenter/Default.aspx?ViewID=HelpCenter_ListView_Articles"; ///*/*"http://localhost:2064/Default.aspx?ViewID=HelpCenter_ListView_Articles";//*/*/
                if (System.Configuration.ConfigurationManager.AppSettings["Helpcenterurl"] != null)
                {
                    string viewUrl = System.Configuration.ConfigurationManager.AppSettings["Helpcenterurl"];
                WebWindow.CurrentRequestWindow.RegisterStartupScript(HelpCenterAction.Id, string.Format("window.open('{0}','_blank');", viewUrl));
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("Sorry! The help center link is not accessible.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
                ////WebWindow.CurrentRequestWindow.RegisterStartupScript(HelpCenterAction.Id, string.Format("window.open('HelpCenter.aspx','_blank');"));
                //e.Cancel = true;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }


        private void SalesAnalyticsAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (SecuritySystem.CurrentUser != null)
                {
                    Employee objemployee = (Employee)SecuritySystem.CurrentUser;
                string viewUrl = ConfigurationManager.AppSettings["SalesAnalysisSite"]; 
                    var key = "b14ca5898a4e4133bbce2ea2315a1916";
                    string current_datetimeformated = (DateTime.Now).ToString("yyyyMMddHHmmss");
                    var encryptedString = EncryptString(key, (objemployee.DisplayName + current_datetimeformated));
                    SetData_SalesAnalytics_UserAccessToken(objemployee.Oid, encryptedString);
                    WebWindow.CurrentRequestWindow.RegisterStartupScript(SalesAnalyticsAction.Id, string.Format("window.open('{0}','_blank');", viewUrl + "?tkn=" + encryptedString));
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SetData_SalesAnalytics_UserAccessToken(Guid UserOid, string Token)
        {       
            try
            {
                string ConnectionString_SalesAnalytic = ConfigurationManager.ConnectionStrings["ConnectionString_SalesAnalytics"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConnectionString_SalesAnalytic))
                {
                    using (SqlCommand cmd = new SqlCommand("ASP_UserAccessToken_SP", connection))
                    {
                        connection.Open();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "SetAccessToken");
                        cmd.Parameters.AddWithValue("@UserOid", UserOid);
                        cmd.Parameters.AddWithValue("@Token", Token);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        connection.Close();
                        if (rowsAffected > 0)
                        {
                        }
                        else
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "SalesAnalysisDashboard");
                //return null;
            }
        }


        private string EncryptString(string key, string plainText)
        {

            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);

        }

    }
}
