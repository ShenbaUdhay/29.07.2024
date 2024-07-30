using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Image = System.Web.UI.WebControls.Image;

namespace BTWEB.Web
{
    public partial class CustomLogin : TemplateContent
    {
        public override DevExpress.ExpressApp.Templates.IActionContainer DefaultContainer
        {
            get { return null; }
        }
        public override void SetStatus(ICollection<string> statusMessages)
        {

        }

        public override object ViewSiteControl
        {
            get { return null; }
        }

        Exception lastLoginException = null;

        protected void LoginUser_Authenticate(object sender, System.Web.UI.WebControls.AuthenticateEventArgs e)
        {

            lastLoginException = null;
            bool authenticated = false;
            try
            {
                using (IObjectSpace os = WebApplication.Instance.CreateObjectSpace(typeof(PermissionPolicyUser)))
                {
                    AuthenticationStandard authentication = (AuthenticationStandard)((SecurityStrategyBase)WebApplication.Instance.Security).Authentication;
                    Guard.ArgumentNotNull(authentication, "authentication");
                    AuthenticationStandardLogonParameters logonParameters = (AuthenticationStandardLogonParameters)authentication.LogonParameters;
                    Guard.ArgumentNotNull(logonParameters, "logonParameters");
                    logonParameters.UserName = LoginUser.UserName;
                    logonParameters.Password = LoginUser.Password;
                    authenticated = authentication.Authenticate(os) != null;//Dennis: You can use a custom authentication algorithm here;
                    if (authenticated)
                    {
                        string strSiteName = System.Web.Hosting.HostingEnvironment.SiteName;
                        string strRememberMe = String.Format("{0}", Request.Form["Logon$LoginUser$chkRememberMe"]);
                        if (strRememberMe.ToLower() == "on")
                        {
                            Response.Cookies[strSiteName + "UserName"].Expires = DateTime.Now.AddDays(30);
                            Response.Cookies[strSiteName + "Password"].Expires = DateTime.Now.AddDays(30);
                        }
                        else
                        {
                            Response.Cookies[strSiteName + "UserName"].Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies[strSiteName + "Password"].Expires = DateTime.Now.AddDays(-1);
                        }
                        Response.Cookies[strSiteName + "UserName"].Value = LoginUser.UserName;
                        Response.Cookies[strSiteName + "Password"].Value = LoginUser.Password;
                    }
                }
            }
            catch (Exception exp)
            {
                authenticated = false;
                lastLoginException = exp;
            }
            e.Authenticated = authenticated;
        }

        protected void LoginUser_LoggedIn(object sender, EventArgs e)
        {
            //WebApplication.Instance.Start();
            //using (IObjectSpace os = WebApplication.Instance.CreateObjectSpace(typeof(PermissionPolicyUser)))
            //{
            //   SecuritySystem.Logon(os);
            //}
            //((BTLIMSAspNetApplication)WebApplication.Instance).DoLogon();
        }

        protected void LoginUser_LoginError(object sender, EventArgs e)
        {
            if (lastLoginException != null)
            {
                //DoSomething(lastLoginException);
            }
        }

        public class AuthenticationStandardForFullyCustomLogin : AuthenticationStandard
        {
            public override bool AskLogonParametersViaUI
            {
                get
                {
                    return false;
                }
            }
        }

        protected void LoginImage_PreRender(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Image img = (Image)sender;
            if (img != null && !string.IsNullOrEmpty(constr))
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand com = new SqlCommand("Get_Loginpageimage_SP", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        using (SqlDataReader dr = com.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr["Attach"] != null && dr["Attach"].GetType() != typeof(DBNull))
                                {
                                    using (MemoryStream ms = new MemoryStream((byte[])dr["Attach"]))
                                    {
                                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                                        string ext = new ImageFormatConverter().ConvertToString(image.RawFormat);
                                        img.ImageUrl = string.Format("data:image/" + ext + ";base64,{0}", Convert.ToBase64String(ms.ToArray()));
                                    }
                                }
                            }
                        }
                        con.Close();
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strSiteName = System.Web.Hosting.HostingEnvironment.SiteName;
                //if (Request.Cookies["SPAUserName"] != null && Request.Cookies["SPAPassword"] != null)
                if (Request.Cookies[strSiteName + "UserName"] != null && Request.Cookies[strSiteName + "Password"] != null)
                {
                    string strUserName = String.Format("{0}", Request.Cookies[strSiteName + "UserName"].Value);
                    string strPassword = String.Format("{0}", Request.Cookies[strSiteName + "Password"].Value);
                    LoginUser.UserName = strUserName;
                    //((TextBox)LoginUser.FindControl("Password")).Attributes.Add("value", strPassword);
                }
            }
        }

        protected void Product_Load(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            if (lbl != null)
            {
                string name = ConfigurationManager.AppSettings["ProductName"];
                if (!string.IsNullOrEmpty(name))
                {
                    lbl.Text = "Welcome to ALPACA " + name;
                }
                else
                {
                    lbl.Text = "Welcome to ALPACA LIMS";
                }
            }
        }

        protected void BannerImage_PreRender(object sender, EventArgs e)
        {
            Image img = (Image)sender;
            if (img != null)
            {
                string name = ConfigurationManager.AppSettings["ProductName"];
                if (!string.IsNullOrEmpty(name))
                {
                    if (name == "CCWRD TOrC")
                    {
                        img.ImageUrl = "~/Images/TOrCLoginWhite.png";
                    }
                    else
                    {
                        img.ImageUrl = "~/Images/NewAlpacaLogoWhite.png";
                        //img.ImageUrl = "~/Images/LoginWhite.png";
                    }
                }
                else
                {
                    img.ImageUrl = "~/Images/NewAlpacaLogoWhite.png";
                    //img.ImageUrl = "~/Images/LoginWhite.png";
                }
            }
        }

        protected void Version_Load(object sender, EventArgs e)
        {
            #region VersionNumber
            VersionControlInfo objVersion = new VersionControlInfo();
            System.Reflection.Assembly assem;
            System.Reflection.AssemblyName assemname;
            System.Version assemVersion;
            assem = System.Reflection.Assembly.GetExecutingAssembly();
            assemname = assem.GetName();
            assemVersion = assemname.Version;
            //var strVersionNumber = assemname.Version.ToString();
            var strVersionNumber = string.Empty;
            List<string> VersionNumber = assemname.Version.ToString().Split('.').ToList();
            if (VersionNumber != null && VersionNumber.Count > 0)
            {
                if (VersionNumber.Count == 4)
                {
                    VersionNumber.RemoveAt(3);
                    strVersionNumber = string.Join(".", VersionNumber.ToArray());
                }
            }
            objVersion.VersionNumber = strVersionNumber;
            #endregion
            Label lbl = (Label)sender;
            if (lbl != null)
            {
                lbl.Text = "Version " + objVersion.VersionNumber;
            }
        }
    }
}