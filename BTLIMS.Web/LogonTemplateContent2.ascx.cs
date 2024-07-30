using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Templates;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Label = System.Web.UI.WebControls.Label;
//using Label = System.Windows.Forms.Label;

namespace Labmaster.Web
{
    public partial class LogonTemplateContent2 : TemplateContent, IXafPopupWindowControlContainer
    {
        public override IActionContainer DefaultContainer
        {
            get { return null; }
        }
        public override void SetStatus(ICollection<string> statusMessages)
        {
        }
        public override object ViewSiteControl
        {
            get
            {
                return viewSiteControl;
            }
        }
        public XafPopupWindowControl XafPopupWindowControl
        {
            get { return PopupWindowControl; }
        }

        public Func<object, object, object> ContentTemplate { get; private set; }
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
        protected void Owns_Load(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            if (lbl != null)
            {
                lbl.Text = "Powered by BTSOFT";
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] whitethemelist = { "BlackGlass", "Office2010Black", "PlasticBlue", "RedWine" };
            string theme = string.Empty;
            if (HttpContext.Current.Items["DXTheme"] != null)
            {
                theme = HttpContext.Current.Items["DXTheme"].ToString();
            }
            else
            {
                DevExpress.Web.ThemesConfigurationSection themesSection = ((DevExpress.Web.ThemesConfigurationSection)System.Configuration.ConfigurationManager.GetSection("devExpress/themes"));
                if (themesSection != null)
                {
                    theme = themesSection.Theme;
                }
            }

            if (!string.IsNullOrEmpty(theme) && whitethemelist.Contains(theme))
            {
                //LogInImageControl.ImageUrl = "~/Images/LoginWhite.png";
                LogInImageControl.ImageUrl = "~/Images/NewAlpacaLogoWhite.png";
                Owns.ForeColor = Color.Gray;
                Version.ForeColor = Color.Gray;
                dbConnectionLink.ForeColor = Color.Gray;
                //content.Image.ImageName = "Add.png";
            }
            else
            {
                //LogInImageControl.ImageUrl = "~/Images/LoginBlack.png";
                LogInImageControl.ImageUrl = "~/Images/NewAlpacaLogoBlack.png";
                Owns.ForeColor = Color.Gray;
                Version.ForeColor = Color.Gray;
                dbConnectionLink.ForeColor = Color.Gray;
                //content.Image.ImageName = "LDM.png";
            }
            string url = HttpContext.Current.Request.Url.OriginalString;
            if (!string.IsNullOrEmpty(url) && !url.Contains("AlpacaLims_demo"))
            {
                dbConnectionLink.Visible = false;
            }
            else
            {
                dbConnectionLink.Visible = true;
            }


            //string url = HttpContext.Current.Request.Url.OriginalString;
            //if (!string.IsNullOrEmpty(url) && !url.Contains("AlpacaLims_demo"))
            //{
            //    DBsetupComboBox.Visible = false;
            //}
            //else
            //{
            //    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            //    if (!string.IsNullOrEmpty(connectionString))
            //    {
            //        string connectedDB = string.Empty;
            //        string strservername = string.Empty;
            //        string strdatabase = string.Empty;
            //        string strusername = string.Empty;
            //        string strpassword = string.Empty;
            //        string[] strarrconnectionstring = connectionString.Split(';');
            //        strservername = strarrconnectionstring[0].Split('=').GetValue(1).ToString();
            //        strdatabase = strarrconnectionstring[1].Split('=').GetValue(1).ToString();
            //        strusername = strarrconnectionstring[2].Split('=').GetValue(1).ToString();
            //        strpassword = strarrconnectionstring[3].Split('=').GetValue(1).ToString();
            //        SqlConnection sqlcon = new SqlConnection(connectionString);
            //        SqlCommand sqlcmd = new SqlCommand("Select Title from DBConnection Where ServerName ='" + strservername + "' And DataBaseName ='" + strdatabase + "' And UserName = '" + strusername + "' And Password='" + strpassword + "' And GCRecord is Null", sqlcon);
            //        SqlDataAdapter sqladapter = new SqlDataAdapter(sqlcmd);
            //        DataTable Datatable = new DataTable();
            //        sqladapter.Fill(Datatable);
            //        foreach (DataRow dr in Datatable.Rows)
            //        {
            //            DBsetupComboBox.Text = dr["Title"].ToString();
            //            connectedDB = dr["Title"].ToString();
            //        }
            //        string selectDBCon = "Select Title from DBConnection";
            //        SqlConnection con = new SqlConnection(connectionString);
            //        SqlCommand cmd = new SqlCommand(selectDBCon, con);
            //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            //        DataTable DT = new DataTable();
            //        adapter.Fill(DT);
            //        foreach (DataRow dr in DT.Rows)
            //        {
            //            if (connectedDB.ToUpper() != dr["Title"].ToString().ToUpper())
            //            {
            //                DBsetupComboBox.Items.Add(dr["Title"].ToString());
            //            }
            //        }
            //    }
            //}
        }

        //protected void DBConnection_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (DBsetupComboBox.SelectedItem != null)
        //    {
        //        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //        string seldbcon = "Select * from DBConnection Where Title ='" + DBsetupComboBox.SelectedItem.Text + "'";
        //        SqlConnection con = new SqlConnection(connectionString);
        //        SqlCommand cmd = new SqlCommand(seldbcon, con);
        //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //        DataTable DT = new DataTable();
        //        adapter.Fill(DT);
        //        string strservername = string.Empty;
        //        string strdbname = string.Empty;
        //        string strusername = string.Empty;
        //        string strpassword = string.Empty;
        //        foreach (DataRow dr in DT.Rows)
        //        {
        //            foreach (DataColumn dtcol in dr.Table.Columns)
        //            {
        //                if (dtcol.ToString() == "ServerName")
        //                {
        //                    strservername = dr["ServerName"].ToString();
        //                }
        //                else
        //                if (dtcol.ToString() == "DataBaseName")
        //                {
        //                    strdbname = dr["DataBaseName"].ToString();
        //                }
        //                else
        //                if (dtcol.ToString() == "UserName")
        //                {
        //                    strusername = dr["UserName"].ToString();
        //                }
        //                else
        //                if (dtcol.ToString() == "Password")
        //                {
        //                    strpassword = dr["Password"].ToString();
        //                }
        //            }
        //        }
        //        string strLocalFile = HttpContext.Current.Server.MapPath(@"~\Web.config");
        //        if (File.Exists(strLocalFile) == true)
        //        {

        //            string strselectconn = "|Data Source=" + strservername + ";Initial Catalog=" + strdbname + ";User ID=" + strusername + ";Password=" + strpassword + ";Integrated Security=False;|";

        //            string strWebConfigConn = File.ReadAllText(strLocalFile);
        //            string[] strsplitconn = strWebConfigConn.Split(new string[] { "connectionString=" }, StringSplitOptions.None);
        //            string[] strspliconn2 = strsplitconn[1].Split(new string[] { "<!--Masterconnectionstring-->" }, StringSplitOptions.None);
        //            string strfinalwebcon = strsplitconn[0] + "connectionString=" + strselectconn.Replace('|', '"') + "/> <!--Masterconnectionstring-->" + strspliconn2[1];
        //            //string strLocalFile1 = HttpContext.Current.Server.MapPath(@"~\Web1.config");
        //            File.WriteAllText(strLocalFile, strfinalwebcon);
        //        }

        //        WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        //    }

        //}
    }
}
