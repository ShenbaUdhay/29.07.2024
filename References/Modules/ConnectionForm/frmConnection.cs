using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;

namespace Modules.ConnectionForm
{
    public partial class frmConnection : Form
    {
        bool bolStart = false;
        //csLoginBL objBL = new csLoginBL();
        //csLoginInfo objInfo = new csLoginInfo();
        XmlDocument xmlDoc = new XmlDocument();
        XmlNodeList nodes = default(XmlNodeList);
        XmlNodeList bnodes = default(XmlNodeList);
        string strSDMSServerName = string.Empty;
        string strSDMSDataBaseName = string.Empty;
        string strSDMSUserName = string.Empty;
        string strSDMSPassword = string.Empty;
        public static string strConnection = string.Empty;
        string strOld_SDMS_ServerName = string.Empty;
        string strOld_SDMS_DataBaseName = string.Empty;

        //ApplicationGlobal objAppGbl = new ApplicationGlobal();
        //Multilanguage.IMultiLang objML;
        public frmConnection(bool _bolStart)
        {
            //SDMS_ResultEntry.Multi_Language_Support.MultiLanguageModule.SetMultiLanguageObject(this, ref objML);
            InitializeComponent();
            lblError.Text = string.Empty;
            //bolStart = _bolStart;
        }


        private void frmConnection_Load(object sender, EventArgs e)
        {
            try
            {
                //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                // string strConnection = config.ConnectionStrings.ConnectionStrings[1].ConnectionString;

                lblError.Visible = false;
                if (ConFigFileExists() == true)
                {

                    ReadXmlFile_SDMSConc();
                    txtServerName.Text = strSDMSServerName;
                    txtDataBase.Text = strSDMSDataBaseName;
                    txtUserName.Text = strSDMSUserName;
                    txtPassword.Text = strSDMSPassword;

                    strOld_SDMS_ServerName = strSDMSServerName;
                    strOld_SDMS_DataBaseName = strSDMSDataBaseName;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {

                if (Validation() == true)
                {
                    lblError.Visible = false;
                    if (CheckSDMSConnection() == true)
                    {
                        strSDMSDataBaseName = txtDataBase.Text.Trim();
                        strSDMSServerName = txtServerName.Text.Trim();
                        strSDMSUserName = txtUserName.Text.Trim();
                        strSDMSPassword = txtPassword.Text.Trim();
                        //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        ////ConfigurationManager.ConnectionStrings[1].ConnectionString;
                        //config.ConnectionStrings.ConnectionStrings[1].ConnectionString = "Data Source=" + txtServerName.Text.Trim() + ";Initial Catalog= " + txtDataBase.Text.Trim() + ";Persist Security Info=True;User ID= " + txtUserName.Text.Trim() + ";Password=" + txtPassword.Text.Trim();
                        //config.Save(ConfigurationSaveMode.Modified);
                        //ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString = "Data Source=" + txtServerName.Text.Trim() + ";Initial Catalog= " + txtDataBase.Text.Trim() + ";Persist Security Info=True;User ID= " + txtUserName.Text.Trim() + ";Password=" + txtPassword.Text.Trim();

                        //ConfigurationManager.RefreshSection("ConnectionStrings");
                        WriteConfigXML_SDMS(Application.StartupPath + "\\config.xml", strSDMSServerName, strSDMSDataBaseName, strSDMSUserName, strSDMSPassword);
                        //WriteConfigXML_SDMS(Application.StartupPath + "\\SDMS\\config.xml",txtServerName.Text.Trim(), txtDataBase.Text.Trim(), txtUserName.Text.Trim(), txtPassword.Text.Trim());
                        // WriteConfigXML_SDMS(Application.StartupPath + "\\DynamicDesigner\\config.xml", txtServerName.Text.Trim(), txtDataBase.Text.Trim(), txtUserName.Text.Trim(), txtPassword.Text.Trim());
                        if (strSDMSServerName.Trim().ToUpper() != strOld_SDMS_ServerName.Trim().ToUpper() || strSDMSDataBaseName.Trim().ToUpper() != strOld_SDMS_DataBaseName.Trim().ToUpper())
                        {
                            Application.Restart();
                        }
                        else
                        {
                            this.Close();
                        }
                        //if (WriteConfigXML_SDMS(txtServerName.Text.Trim(), txtDataBase.Text.Trim(), txtUserName.Text.Trim(), txtPassword.Text.Trim()) == true)
                        //{
                        //    if (strSDMSServerName.Trim().ToUpper() != strOld_SDMS_ServerName.Trim().ToUpper() || strSDMSDataBaseName.Trim().ToUpper() != strOld_SDMS_DataBaseName.Trim().ToUpper())
                        //    {
                        //        Application.Restart();
                        //    }
                        //    else
                        //    {
                        //        this.Close();
                        //    }

                        //    //else
                        //    //    {
                        //    //        //MessageBox.Show(objML.GetMessageBoxText("SDMS_DBConnection"), objML.GetMessageBoxText("SDMS_DB"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        //    //        this.Close();
                        //    //    }
                        //}
                    }
                    else
                    {
                        //MessageBox.Show(objML.GetMessageBoxText("SDMS_DBConnection"), objML.GetMessageBoxText("SDMS_DB"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        this.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }
        private bool CheckSDMSConnection()
        {
            try
            {
                string _cnString = ("data source="
                            + (txtServerName.Text.Trim() + (";initial catalog="
                            + (txtDataBase.Text.Trim() + (";user id="
                            + (txtUserName.Text.Trim() + (";pwd=" + txtPassword.Text.Trim())))))));
                // Network Library=DBMSSOCN;
                SqlConnection myConnection = new SqlConnection(_cnString);
                myConnection.Open();
                myConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        private bool Validation()
        {
            try
            {
                lblError.Text = string.Empty;
                if (string.IsNullOrEmpty(txtServerName.Text) || string.IsNullOrWhiteSpace(txtServerName.Text))
                {
                    lblError.Visible = true;
                    // lblError.Text = objML.GetControlText(lblError.Name + (int)Multilanguage.MultiLanguageModule.AlternativeControlText.Text2);
                    txtServerName.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(txtDataBase.Text) || string.IsNullOrWhiteSpace(txtDataBase.Text))
                {
                    lblError.Visible = true;
                    //lblError.Text = objML.GetControlText(lblError.Name + (int)Multilanguage.MultiLanguageModule.AlternativeControlText.Text3);
                    txtDataBase.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrWhiteSpace(txtUserName.Text))
                {
                    lblError.Visible = true;
                    //lblError.Text = objML.GetControlText(lblError.Name + (int)Multilanguage.MultiLanguageModule.AlternativeControlText.Text4);
                    txtUserName.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    lblError.Visible = true;
                    // lblError.Text = objML.GetControlText(lblError.Name + (int)Multilanguage.MultiLanguageModule.AlternativeControlText.Text5);
                    txtPassword.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void ReadXmlFile_SDMSConc(bool bolWirte = false)
        {
            try
            {

                //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //if (config != null)
                //{
                //    string strConnection = config.ConnectionStrings.ConnectionStrings[1].ConnectionString;
                //    string[] strConnect = strConnection.Split(';');

                //    //ServerName
                //    if (strConnect[0].Length > 0)
                //    {
                //        string[] strServerName = strConnect[0].Split('=');
                //        if (strServerName[1].Length > 0)
                //        {
                //            strSDMSServerName = strServerName[1];
                //        }
                //        else
                //        {
                //            strSDMSServerName = "";
                //        }
                //    }
                //    else
                //    {
                //        strSDMSServerName = "";
                //    }

                //    //DataBaseName
                //    if (strConnect[1].Length > 0)
                //    {
                //        string[] strDataBaseName = strConnect[1].Split('=');
                //        if (strDataBaseName[1].Length > 0)
                //        {
                //            strSDMSDataBaseName = strDataBaseName[1];
                //        }
                //        else
                //        {
                //            strSDMSDataBaseName = "";
                //        }
                //    }
                //    else
                //    {
                //        strSDMSDataBaseName = "";
                //    }

                //    // User Name
                //    if (strConnect[3].Length > 0)
                //    {
                //        string[] strUserName = strConnect[3].Split('=');
                //        if (strUserName[1].Length > 0)
                //        {
                //            strSDMSUserName = strUserName[1];
                //        }
                //        else
                //        {
                //            strSDMSUserName = "";
                //        }
                //    }
                //    else
                //    {
                //        strSDMSUserName = "";
                //    }

                //    // PassWord
                //    if (strConnect[4].Length > 0)
                //    {
                //        string[] strPassword = strConnect[4].Split('=');
                //        if (strPassword[1].Length > 0)
                //        {
                //            strSDMSPassword = strPassword[1];
                //        }
                //        else
                //        {
                //            strSDMSPassword = "";
                //        }
                //    }
                //    else
                //    {
                //        strSDMSPassword = "";
                //    }
                if (ConFigFileExists() == true)
                {

                    xmlDoc.Load(Application.StartupPath + "\\config.xml");
                    nodes = xmlDoc.GetElementsByTagName("LIMSDataBaseConnection");
                    foreach (XmlNode node in nodes)
                    {
                        bnodes = node.ChildNodes;
                        foreach (XmlNode bnode in bnodes)
                        {
                            if (bnode.Name == "LIMSServerName")
                            {
                                strSDMSServerName = bnode.InnerText.Trim();
                                // strSDMSServerName = ApplicationGlobal.SimpleCrypt(bnode.InnerText);
                            }
                            if (bnode.Name == "LIMSDataBaseName")
                            {
                                strSDMSDataBaseName = bnode.InnerText.Trim();
                                // strSDMSDataBaseName = ApplicationGlobal.SimpleCrypt(bnode.InnerText);
                            }
                            if (bnode.Name == "LIMSUserName")
                            {
                                strSDMSUserName = bnode.InnerText.Trim();
                                //strSDMSUserName = ApplicationGlobal.SimpleCrypt(bnode.InnerText);
                            }
                            if (bnode.Name == "LIMSPassword")
                            {
                                strSDMSPassword = bnode.InnerText.Trim();
                                // strSDMSPassword = ApplicationGlobal.SimpleCrypt(bnode.InnerText);
                            }
                        }
                    }
                    if (bolWirte = true)
                    {
                        //WriteConfigXML_SDMS(Application.StartupPath + "\\SDMS\\config.xml", strSDMSServerName, strSDMSDataBaseName, strSDMSUserName, strSDMSPassword);
                        //WriteConfigXML_SDMS(Application.StartupPath + "\\DynamicDesigner\\config.xml", strSDMSServerName, strSDMSDataBaseName, strSDMSUserName, strSDMSPassword);
                        WriteConfigXML_SDMS(Application.StartupPath + "\\config.xml", strSDMSServerName, strSDMSDataBaseName, strSDMSUserName, strSDMSPassword);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool WriteConfigXML_SDMS(string Path, string strServerName, string strDatabasename, string strUserName, string strPassword)
        {
            try
            {
                if (ConFigFileExists() == true)
                {
                    XmlDocument xml = new XmlDocument();
                    //xml.Load(Application.UserAppDataPath + "\\config.xml");
                    // xml.Load(Application.StartupPath + "\\SDMS\\config.xml");
                    xml.Load(Path);
                    XmlNode commentsElement_server = xml.SelectSingleNode(String.Format("SDMS/SDMSDataBaseConnection/SDMSServerName"));
                    commentsElement_server.InnerText = strServerName;

                    XmlNode commentsElement_DB = xml.SelectSingleNode(String.Format("SDMS/SDMSDataBaseConnection/SDMSDataBaseName"));
                    commentsElement_DB.InnerText = strDatabasename;

                    XmlNode commentsElement_UserName = xml.SelectSingleNode(String.Format("SDMS/SDMSDataBaseConnection/SDMSUserName"));
                    commentsElement_UserName.InnerText = strUserName;

                    XmlNode commentsElement_pwd = xml.SelectSingleNode(String.Format("SDMS/SDMSDataBaseConnection/SDMSPassword"));
                    commentsElement_pwd.InnerText = strPassword;

                    XmlNode commentsElement_limsserver = xml.SelectSingleNode(String.Format("SDMS/LIMSDataBaseConnection/LIMSServerName"));
                    commentsElement_limsserver.InnerText = strServerName;

                    XmlNode commentsElement_limsDB = xml.SelectSingleNode(String.Format("SDMS/LIMSDataBaseConnection/LIMSDataBaseName"));
                    commentsElement_limsDB.InnerText = strDatabasename;

                    XmlNode commentsElement_limsUserName = xml.SelectSingleNode(String.Format("SDMS/LIMSDataBaseConnection/LIMSUserName"));
                    commentsElement_limsUserName.InnerText = strUserName;

                    XmlNode commentsElement_limspwd = xml.SelectSingleNode(String.Format("SDMS/LIMSDataBaseConnection/LIMSPassword"));
                    commentsElement_limspwd.InnerText = strPassword;

                    //xml.Save(Application.UserAppDataPath + "\\config.xml");
                    xml.Save(Path);
                    //xml.Save(Application.StartupPath + "\\SDMS\\config.xml");

                    return true;
                }
                else
                {
                    //XmlTextWriter xmlNewFile = new XmlTextWriter(Application.UserAppDataPath + "\\config.xml", null);
                    XmlTextWriter xmlNewFile = new XmlTextWriter(Path, null);
                    var _Wrt = xmlNewFile;
                    _Wrt.Formatting = Formatting.Indented;
                    _Wrt.Indentation = 4;
                    //_with1.IndentChar = string.Empty;
                    _Wrt.WriteStartDocument(false);
                    _Wrt.WriteComment("Connection Property");
                    _Wrt.WriteStartElement("SDMS");

                    _Wrt.WriteStartElement("SDMSDataBaseConnection");
                    _Wrt.WriteElementString("SDMSServerName", SimpleCrypt(strServerName));
                    _Wrt.WriteElementString("SDMSDataBaseName", SimpleCrypt(strDatabasename));
                    _Wrt.WriteElementString("SDMSUserName", SimpleCrypt(strUserName));
                    _Wrt.WriteElementString("SDMSPassword", SimpleCrypt(strPassword));
                    _Wrt.WriteEndElement();

                    _Wrt.WriteStartElement("LIMSDataBaseConnection");
                    _Wrt.WriteElementString("LIMSServerName", string.Empty);
                    _Wrt.WriteElementString("LIMSDataBaseName", string.Empty);
                    _Wrt.WriteElementString("LIMSUserName", string.Empty);
                    _Wrt.WriteElementString("LIMSPassword", string.Empty);
                    _Wrt.WriteEndElement();

                    _Wrt.WriteStartElement("LanguageSetting");
                    _Wrt.WriteElementString("Language", SimpleCrypt("EN"));

                    _Wrt.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool ConFigFileExists()
        {
            try
            {
                //if (!System.IO.File.Exists(Application.UserAppDataPath + "\\config.xml"))
                if (!System.IO.File.Exists(Application.StartupPath + "\\config.xml"))
                    return false;
                //xmlDoc.Load((Application.UserAppDataPath + "\\config.xml"));
                xmlDoc.Load((Application.StartupPath + "\\config.xml"));
                return true;
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static string SimpleCrypt(string Text)
        {
            try
            {
                // Encrypts/decrypts the passed string using 
                // a simple ASCII value-swapping algorithm
                string result = Text;
                //string result = "";
                //char newChar;
                //int currCharAsc;
                //for (int i = 0; i < Text.Length; i++)
                //{
                //    newChar = (char)0;
                //    currCharAsc = Convert.ToInt16(Text[i]);
                //    if (currCharAsc < 128)
                //    {
                //        newChar = Convert.ToChar(currCharAsc + 128);
                //    }
                //    else if (currCharAsc > 128)
                //    { 
                //        newChar = Convert.ToChar(currCharAsc - 128);
                //    }
                //    result += newChar;
                //}
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return string.Empty;
            }
        }
        public void ReadAndWriteConfig()
        {
            ReadXmlFile_SDMSConc(true);
        }

        public bool SetConnectionString()
        {
            try
            {
                if (ConFigFileExists() == true)
                {
                    //MessageBox.Show("true");

                    xmlDoc.Load(Application.StartupPath + "\\config.xml");
                    nodes = xmlDoc.GetElementsByTagName("LIMSDataBaseConnection");
                    foreach (XmlNode node in nodes)
                    {
                        bnodes = node.ChildNodes;
                        foreach (XmlNode bnode in bnodes)
                        {
                            if (bnode.Name == "LIMSServerName")
                            {
                                strSDMSServerName = bnode.InnerText.Trim();
                                // strSDMSServerName = ApplicationGlobal.SimpleCrypt(bnode.InnerText);
                            }
                            if (bnode.Name == "LIMSDataBaseName")
                            {
                                strSDMSDataBaseName = bnode.InnerText.Trim();
                                // strSDMSDataBaseName = ApplicationGlobal.SimpleCrypt(bnode.InnerText);
                            }
                            if (bnode.Name == "LIMSUserName")
                            {
                                strSDMSUserName = bnode.InnerText.Trim();
                                //strSDMSUserName = ApplicationGlobal.SimpleCrypt(bnode.InnerText);
                            }
                            if (bnode.Name == "LIMSPassword")
                            {
                                strSDMSPassword = bnode.InnerText.Trim();
                                // strSDMSPassword = ApplicationGlobal.SimpleCrypt(bnode.InnerText);
                            }
                        }
                    }

                    strConnection = ("data source="
                            + (strSDMSServerName + (";initial catalog="
                            + (strSDMSDataBaseName + (";user id="
                            + (strSDMSUserName + (";pwd=" + strSDMSPassword)))))));
                    SqlConnection myConnection = new SqlConnection(strConnection);
                    myConnection.Open();
                    myConnection.Close();
                    return true;


                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

    }
}
