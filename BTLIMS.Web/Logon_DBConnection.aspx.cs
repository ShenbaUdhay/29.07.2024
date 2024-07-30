using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;


namespace LDM.Web
{
    public partial class Logon_DBConnection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindTitles();
                BindServerNames();
                BindDatabaseNames();
            }
        }
        private void BindTitles()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select Distinct Title from DBConnection";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    // Bind the rest of the titles from the database
                    ddlTitle.DataSource = command.ExecuteReader();
                    ddlTitle.DataTextField = "Title";
                    ddlTitle.DataValueField = "Title";
                    ddlTitle.DataBind();
                }
            }
        }
        [System.Web.Services.WebMethod]
        public static List<string> GetServerNames(string title)
        {
            List<string> serverNames = new List<string>();
            serverNames.Add("Server1");
            return serverNames;
        }

        [System.Web.Services.WebMethod]
        public static List<string> GetDatabaseNames(string serverName)
        {
            List<string> databaseNames = new List<string>();
            databaseNames.Add("Database1");
            return databaseNames;
        }
        private void BindServerNames()
        {
            string selectedTitle = ddlTitle.SelectedValue;
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"select Distinct ServerName from DBConnection WHERE Title = '{selectedTitle}'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    ddlServerName.DataSource = command.ExecuteReader();
                    ddlServerName.DataTextField = "ServerName";
                    ddlServerName.DataValueField = "ServerName";
                    ddlServerName.DataBind();
                }
            }
        }
        private void BindDatabaseNames()
        {
            string selectedServerName = ddlServerName.SelectedValue;
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"select Distinct DataBaseName from DBConnection where ServerName = '{selectedServerName}'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    ddlDataBaseName.DataSource = command.ExecuteReader();
                    ddlDataBaseName.DataTextField = "DataBaseName";
                    ddlDataBaseName.DataValueField = "DataBaseName";
                    ddlDataBaseName.DataBind();
                }
            }
        }
        protected void Title_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindServerNames();
            BindDatabaseNames();
        }

        protected void ServerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDatabaseNames();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string title = ddlTitle.SelectedValue;
            if (title == "N/A")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "InvalidTitleScript", "alert('Please select a valid title.');", true);
                return;
            }
            else
            {
                string projectName = title;
                SetConfigValue("ProjectName", projectName);
            }
            string serverName = ddlServerName.SelectedValue;
            string databaseName = ddlDataBaseName.SelectedValue;
            string userName = txtUserName.Text;
            if (string.IsNullOrEmpty(userName))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "InvalidUserNameScript", "alert('Please enter the User Id');", true);
                return;
            }
            string password = txtPassword.Text;
            if (string.IsNullOrEmpty(password))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "InvalidTitleScript", "alert('Please enter the password.');", true);
                return;
            }
            string newConnectionString = $"Data Source={serverName};Initial Catalog={databaseName};User ID={userName};Password={password};Integrated Security=False;";
            var configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            var connectionString = configuration.ConnectionStrings.ConnectionStrings["ConnectionString"];

            if (connectionString != null)
            {
                connectionString.ConnectionString = newConnectionString;
                configuration.Save();
                ConfigurationManager.RefreshSection("connectionStrings");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ClosePopupScript", "window.close();", true);
            }
        }
        private void SetConfigValue(string key, string value)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement setting = config.AppSettings.Settings[key];
            if (setting == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                setting.Value = value;
            }
            config.Save(ConfigurationSaveMode.Modified);
        }

    }
}