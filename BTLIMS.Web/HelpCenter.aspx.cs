using LDM.Module.Controllers.Public;
using LDM.Module.Web.Controllers;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Unit = System.Web.UI.WebControls.Unit;

namespace LDM.Web
{
    public partial class HelpCenter : System.Web.UI.Page
    {
        VersionControlInfo objVersion = new VersionControlInfo();
        HelpCenterInfo objhelpinfo = new HelpCenterInfo();

        protected System.Web.UI.HtmlControls.HtmlGenericControl bodyID;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    lbl_Version.Text = "Version " + objVersion.VersionNumber;
                    lbl_owns.Text = "Powered by BTSOFT";
                    string[] whitethemelist = { "BlackGlass", "Office2010Black", "PlasticBlue", "RedWine", "Youthful" };
                    if (!string.IsNullOrEmpty(objhelpinfo.strcrtTheme))
                    {
                        if (whitethemelist.Contains(objhelpinfo.strcrtTheme))
                        {
                            ProjectLogo.ImageUrl = "~/Images/NewAlpacaLogoWhite.png";
                            lbl_owns.ForeColor = Color.Gray;
                            lbl_Version.ForeColor = Color.Gray;
                        }
                        else
                        {
                            ProjectLogo.ImageUrl = "~/Images/NewAlpacaLogoBlack.png";
                            lbl_owns.ForeColor = Color.Black;
                            lbl_Version.ForeColor = Color.Black;
                        }

                        if (objhelpinfo.strcrtTheme == "Aqua")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_GridView.HeaderStyle.BackColor = Color.FromArgb(186, 214, 253);
                            lbl_FormName0.ForeColor = HelpCenter_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Gray;
                            Category_DropDownList.BackColor = txt_search.BackColor = HelpCenter_GridView.RowStyle.BackColor = HelpCenter_GridView.BorderColor = Color.FromArgb(224, 237, 255);
                        }
                        else if (objhelpinfo.strcrtTheme == "BlackGlass")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_GridView.HeaderStyle.BackColor = Color.FromArgb(42, 42, 42);
                            lbl_FormName0.ForeColor = HelpCenter_GridView.HeaderStyle.ForeColor = Color.White;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Gray;
                            Category_DropDownList.BackColor = txt_search.BackColor = HelpCenter_GridView.RowStyle.BackColor = HelpCenter_GridView.BorderColor = Color.FromArgb(147, 167, 186);
                        }
                        else if (objhelpinfo.strcrtTheme == "DevEx")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_GridView.HeaderStyle.BackColor = Color.FromArgb(238, 238, 243);
                            lbl_FormName0.ForeColor = HelpCenter_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Black;
                            Category_DropDownList.BackColor = txt_search.BackColor = HelpCenter_GridView.RowStyle.BackColor = HelpCenter_GridView.BorderColor = Color.FromArgb(244, 245, 248);
                        }
                        else if (objhelpinfo.strcrtTheme == "Glass")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_GridView.HeaderStyle.BackColor = Color.FromArgb(194, 229, 239);
                            Category_DropDownList.BackColor = txt_search.BackColor = HelpCenter_GridView.RowStyle.BackColor = HelpCenter_GridView.BorderColor = Color.FromArgb(189, 226, 239);
                            lbl_FormName0.ForeColor = HelpCenter_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Black;
                        }
                        else if (objhelpinfo.strcrtTheme == "Office2003Blue")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_GridView.HeaderStyle.BackColor = Color.FromArgb(164, 198, 248);
                            Category_DropDownList.BackColor = txt_search.BackColor = HelpCenter_GridView.RowStyle.BackColor = HelpCenter_GridView.BorderColor = Color.FromArgb(215, 230, 248);
                            lbl_FormName0.ForeColor = HelpCenter_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Black;
                        }
                        else if (objhelpinfo.strcrtTheme == "Office2003Olive")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_GridView.HeaderStyle.BackColor = Color.FromArgb(214, 220, 187);
                            Category_DropDownList.BackColor = txt_search.BackColor = HelpCenter_GridView.RowStyle.BackColor = HelpCenter_GridView.BorderColor = Color.FromArgb(255, 255, 213);
                            lbl_FormName0.ForeColor = HelpCenter_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Black;
                        }
                        else if (objhelpinfo.strcrtTheme == "Office2003Silver")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_GridView.HeaderStyle.BackColor = Color.FromArgb(208, 208, 222);
                            Category_DropDownList.BackColor = txt_search.BackColor = HelpCenter_GridView.RowStyle.BackColor = HelpCenter_GridView.BorderColor = Color.FromArgb(230, 230, 238);
                            lbl_FormName0.ForeColor = HelpCenter_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Black;
                        }
                        else if (objhelpinfo.strcrtTheme == "Office2010Black")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_GridView.HeaderStyle.BackColor = Color.FromArgb(39, 39, 39);
                            Category_DropDownList.BackColor = txt_search.BackColor = HelpCenter_GridView.RowStyle.BackColor = HelpCenter_GridView.BorderColor = Color.FromArgb(163, 162, 163);
                            lbl_FormName0.ForeColor = HelpCenter_GridView.HeaderStyle.ForeColor = Color.White;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Gray;
                        }
                        else if (objhelpinfo.strcrtTheme == "Office2010Blue")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_GridView.HeaderStyle.BackColor = Color.FromArgb(211, 228, 246);
                            Category_DropDownList.BackColor = txt_search.BackColor = HelpCenter_GridView.RowStyle.BackColor = HelpCenter_GridView.BorderColor = Color.FromArgb(180, 202, 227);
                            lbl_FormName0.ForeColor = HelpCenter_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Black;
                        }
                        else if (objhelpinfo.strcrtTheme == "Office2010Silver")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_GridView.HeaderStyle.BackColor = Color.FromArgb(210, 213, 218);
                            Category_DropDownList.BackColor = txt_search.BackColor = HelpCenter_GridView.RowStyle.BackColor = HelpCenter_GridView.BorderColor = Color.FromArgb(255, 255, 213);
                            lbl_FormName0.ForeColor = HelpCenter_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Black;
                        }
                        else if (objhelpinfo.strcrtTheme == "PlasticBlue")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_GridView.HeaderStyle.BackColor = Color.FromArgb(60, 79, 145);
                            Category_DropDownList.BackColor = txt_search.BackColor = HelpCenter_GridView.RowStyle.BackColor = HelpCenter_GridView.BorderColor = Color.FromArgb(145, 162, 202);
                            lbl_FormName0.ForeColor = HelpCenter_GridView.HeaderStyle.ForeColor = Color.White;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Gray;
                        }
                        else if (objhelpinfo.strcrtTheme == "RedWine")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_GridView.HeaderStyle.BackColor = Color.FromArgb(120, 0, 43);
                            Category_DropDownList.BackColor = txt_search.BackColor = HelpCenter_GridView.RowStyle.BackColor = HelpCenter_GridView.BorderColor = Color.FromArgb(208, 77, 126);
                            lbl_FormName0.ForeColor = HelpCenter_GridView.HeaderStyle.ForeColor = Color.White;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Gray;
                        }
                        else if (objhelpinfo.strcrtTheme == "SoftOrange")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_GridView.HeaderStyle.BackColor = Color.FromArgb(233, 233, 233);
                            Category_DropDownList.BackColor = txt_search.BackColor = HelpCenter_GridView.RowStyle.BackColor = HelpCenter_GridView.BorderColor = Color.FromArgb(255, 159, 67);
                            lbl_FormName0.ForeColor = HelpCenter_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Black;
                        }
                        else if (objhelpinfo.strcrtTheme == "Youthful")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_GridView.HeaderStyle.BackColor = Color.FromArgb(138, 161, 80);
                            Category_DropDownList.BackColor = txt_search.BackColor = HelpCenter_GridView.RowStyle.BackColor = HelpCenter_GridView.BorderColor = Color.FromArgb(229, 238, 207);
                            lbl_FormName0.ForeColor = HelpCenter_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Black;
                        }
                    }

                    string strscreenwidth = System.Web.HttpContext.Current.Request.Cookies.Get("screenwidth").Value;
                    int prscreenwidth = Convert.ToInt32(strscreenwidth) * 2 / 100;
                    Heading_Panel.Width = Convert.ToInt32(strscreenwidth);// - prscreenwidth;
                    HelpCenter_GridView.Width = Convert.ToInt32(strscreenwidth) - prscreenwidth;
                    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    string selectSQL = "SP_HelpCenter";
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand(selectSQL, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Category", SqlDbType.VarChar).Value = Category_DropDownList.SelectedItem.Value.ToString();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable DT = new DataTable();
                    adapter.Fill(DT);
                    int i = 0;
                    DT.Columns.Add("FileData");
                    foreach (DataRow dr in DT.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["File"].ToString()))
                        {
                            dr["FileData"] = "Download";
                        }
                        else
                        {
                            dr["FileData"] = string.Empty;
                        }
                        System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox();
                        if (rtBox != null)
                        {
                            if (!string.IsNullOrEmpty(dr["Article"].ToString()))
                            {
                                rtBox.Rtf = dr["Article"].ToString();
                                dr["Article"] = rtBox.Text;
                            }
                            else
                            {
                                dr["Article"] = string.Empty;
                            }
                        }
                        else
                        {
                            dr["Article"] = string.Empty;
                        }
                    }
                    foreach (DataControlField column in HelpCenter_GridView.Columns)
                    {
                        if (column.HeaderText == "Question" && column.GetType() == typeof(ButtonField))
                        {
                            column.Visible = false;
                        }
                        else if (column.HeaderText == "Question" && column.GetType() != typeof(ButtonField))
                        {
                            column.Visible = true;
                        }
                        if (column.HeaderText == "Question")
                        {
                            column.ItemStyle.Width = Unit.Percentage(30);
                        }
                        else if (column.HeaderText == "Article")
                        {
                            column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Percentage(50);
                        }
                        else
                        {
                            column.ItemStyle.Width = Unit.Percentage(20);
                        }
                    }
                    HelpCenter_GridView.DataSource = DT;
                    HelpCenter_GridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                ExceptionTrackingViewController ExceptionController = new ExceptionTrackingViewController();
                ExceptionController.InsertAspxException(ex.Message.ToString(), ex.StackTrace.ToString(), this.GetType().Name.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name.ToString());
            }
        }
        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Category_DropDownList.SelectedItem.Value.ToString() == "FAQ")
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    string selectSQL = "SP_HelpCenter";
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand(selectSQL, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Category", SqlDbType.VarChar).Value = Category_DropDownList.SelectedItem.Value.ToString();
                    cmd.Parameters.AddWithValue("@txtsearch", SqlDbType.VarChar).Value = txt_search.Text.ToString();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable DT = new DataTable();
                    adapter.Fill(DT);
                    HelpCenter_GridView.AutoGenerateColumns = false;
                    DT.Columns.Add("FileData");
                    foreach (DataRow dr in DT.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["File"].ToString()))
                        {
                            dr["FileData"] = "Download";
                        }
                        else
                        {
                            dr["FileData"] = string.Empty;
                        }
                        System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox();
                        if (rtBox != null)
                        {
                            if (!string.IsNullOrEmpty(dr["Article"].ToString()))
                            {
                                rtBox.Rtf = dr["Article"].ToString();
                                dr["Article"] = rtBox.Text;
                            }
                            else
                            {
                                dr["Article"] = string.Empty;
                            }
                        }
                        else
                        {
                            dr["Article"] = string.Empty;
                        }
                    }
                    string strscreenwidth = System.Web.HttpContext.Current.Request.Cookies.Get("screenwidth").Value;
                    int prscreenwidth = Convert.ToInt32(strscreenwidth) * 2 / 100;
                    HelpCenter_GridView.Width = Convert.ToInt32(strscreenwidth) - prscreenwidth;
                    foreach (DataControlField column in HelpCenter_GridView.Columns)
                    {
                        if (column.HeaderText == "Question" && column.GetType() == typeof(ButtonField))
                        {
                            column.Visible = false;
                        }
                        else if (column.HeaderText == "Question" && column.GetType() != typeof(ButtonField))
                        {
                            column.Visible = true;
                        }
                        if (column.HeaderText == "Question")
                        {
                            column.ItemStyle.Width = Unit.Percentage(30);
                        }
                        else if (column.HeaderText == "Article")
                        {
                            column.ItemStyle.Width = Unit.Percentage(50);
                        }
                        else
                        {
                            column.ItemStyle.Width = Unit.Percentage(20);
                        }
                    }
                    HelpCenter_GridView.DataSource = DT;
                    HelpCenter_GridView.DataBind();
                }
                else if (Category_DropDownList.SelectedItem.Value.ToString() == "Manual")
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    string selectSQL = "SP_HelpCenter_Manual";
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand(selectSQL, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Category", SqlDbType.VarChar).Value = Category_DropDownList.SelectedItem.Value.ToString();
                    cmd.Parameters.AddWithValue("@txtsearch", SqlDbType.VarChar).Value = txt_search.Text.ToString();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable DT = new DataTable();
                    adapter.Fill(DT);
                    DT.Columns.Add("FileData");
                    foreach (DataRow dr in DT.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["File"].ToString()))
                        {
                            dr["FileData"] = "Download";
                        }
                        else
                        {
                            dr["FileData"] = string.Empty;
                        }
                    }
                    string strscreenwidth = System.Web.HttpContext.Current.Request.Cookies.Get("screenwidth").Value;
                    int prscreenwidth = Convert.ToInt32(strscreenwidth) * 2 / 100;
                    HelpCenter_GridView.Width = Convert.ToInt32(strscreenwidth) - prscreenwidth;
                    foreach (DataControlField column in HelpCenter_GridView.Columns)
                    {
                        if (column.HeaderText == "Question" && column.GetType() != typeof(ButtonField))
                        {
                            column.Visible = false;
                        }
                        else if (column.HeaderText == "Question" && column.GetType() == typeof(ButtonField))
                        {
                            column.Visible = true;
                        }
                        if (column.HeaderText == "Question")
                        {
                            column.ItemStyle.Width = Unit.Percentage(30);
                        }
                        else if (column.HeaderText == "Article")
                        {
                            column.ItemStyle.Width = Unit.Percentage(50);
                        }
                        else
                        {
                            column.ItemStyle.Width = Unit.Percentage(20);
                        }
                    }
                    HelpCenter_GridView.DataSource = DT;
                    HelpCenter_GridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ExceptionTrackingViewController ExceptionController = new ExceptionTrackingViewController();
                ExceptionController.Frame.GetController<ExceptionTrackingViewController>().InsertAspxException(ex.Message.ToString(), ex.StackTrace.ToString(), this.GetType().Name.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name.ToString());
                //ExceptionController.InsertAspxException(ex.Message.ToString(), ex.StackTrace.ToString(), this.GetType().Name.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name.ToString());
            }
        }

        protected void CustomersGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (Category_DropDownList.SelectedValue == "FAQ")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow selectedRow = HelpCenter_GridView.Rows[index];
                    string strTopic = selectedRow.Cells[0].Text;
                    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    string selectSQL = "SP_HelpCenter_FAQ";
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand(selectSQL, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Topic", SqlDbType.VarChar).Value = strTopic;
                    HelpCenterInfo helpCenterInfo = new HelpCenterInfo();
                    helpCenterInfo.StrTopic = strTopic;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable DT = new DataTable();
                    adapter.Fill(DT);
                    bool IsMoreFileData = false;
                    if (DT != null && DT.Rows.Count > 1)
                    {
                        IsMoreFileData = true;
                        Response.Redirect("HelpCenter_FileData.Aspx", false);
                    }
                    else if (DT != null && DT.Rows.Count == 1)
                    {
                        HelpCenterController helpCenterController = new HelpCenterController();
                        helpCenterController.downloadfile();
                    }
                }
                else
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    string selectSQL = "SP_HelpCenter_Manual";
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand(selectSQL, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Category", SqlDbType.VarChar).Value = Category_DropDownList.SelectedItem.Value.ToString();
                    cmd.Parameters.AddWithValue("@txtsearch", SqlDbType.VarChar).Value = txt_search.Text.ToString();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable DT = new DataTable();
                    adapter.Fill(DT);
                    HelpCenterInfo helpCenterInfo = new HelpCenterInfo();
                    int index = Convert.ToInt32(e.CommandArgument);
                    int i = 0;
                    string strHPTopic = string.Empty;
                    foreach (DataRow dr in DT.Rows)
                    {
                        if (i == index)
                        {
                            if (!string.IsNullOrEmpty(dr["Question"].ToString()))
                            {
                                strHPTopic = dr["Question"].ToString();
                            }
                        }
                        else
                        {
                            i++;
                            continue;
                        }
                        break;
                    }

                    helpCenterInfo.StrTopic = strHPTopic;
                    Response.Redirect("HelpCenter_Manual.Aspx", false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //MessageBox.Show(ex.Message);
                //ExceptionTrackingViewController ExceptionController = new ExceptionTrackingViewController();
                //ExceptionController.Frame.GetController<ExceptionTrackingViewController>().InsertAspxException(ex.Message.ToString(), ex.StackTrace.ToString(), this.GetType().Name.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name.ToString());
            }
        }

        protected void btnsearch_onclick(object sender, EventArgs e)
        {
            try
            {
                if (Category_DropDownList.SelectedItem.Value.ToString() == "FAQ")
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    string selectSQL = "SP_HelpCenter";
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand(selectSQL, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Category", SqlDbType.VarChar).Value = Category_DropDownList.SelectedItem.Value.ToString();
                    cmd.Parameters.AddWithValue("@txtsearch", SqlDbType.VarChar).Value = txt_search.Text.ToString();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable DT = new DataTable();
                    adapter.Fill(DT);
                    HelpCenter_GridView.AutoGenerateColumns = false;
                    DT.Columns.Add("FileData");
                    foreach (DataRow dr in DT.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["File"].ToString()))
                        {
                            dr["FileData"] = "Download";
                        }
                        else
                        {
                            dr["FileData"] = string.Empty;
                        }
                        System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox();
                        if (rtBox != null)
                        {
                            if (!string.IsNullOrEmpty(dr["Article"].ToString()))
                            {
                                rtBox.Rtf = dr["Article"].ToString();
                                dr["Article"] = rtBox.Text;
                            }
                            else
                            {
                                dr["Article"] = string.Empty;
                            }
                        }
                        else
                        {
                            dr["Article"] = string.Empty;
                        }
                    }
                    string strscreenwidth = System.Web.HttpContext.Current.Request.Cookies.Get("screenwidth").Value;
                    int prscreenwidth = Convert.ToInt32(strscreenwidth) * 2 / 100;
                    HelpCenter_GridView.Width = Convert.ToInt32(strscreenwidth) - prscreenwidth;
                    foreach (DataControlField column in HelpCenter_GridView.Columns)
                    {
                        if (column.HeaderText == "Question" && column.GetType() == typeof(ButtonField))
                        {
                            column.Visible = false;
                        }
                        else if (column.HeaderText == "Question" && column.GetType() != typeof(ButtonField))
                        {
                            column.Visible = true;
                        }
                        if (column.HeaderText == "Question")
                        {
                            column.ItemStyle.Width = Unit.Percentage(30);
                        }
                        else if (column.HeaderText == "Article")
                        {
                            column.ItemStyle.Width = Unit.Percentage(50);
                        }
                        else
                        {
                            column.ItemStyle.Width = Unit.Percentage(20);
                        }
                    }
                    HelpCenter_GridView.DataSource = DT;
                    HelpCenter_GridView.DataBind();
                }
                else if (Category_DropDownList.SelectedItem.Value.ToString() == "Manual")
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    string selectSQL = "SP_HelpCenter_Manual";
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand(selectSQL, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Category", SqlDbType.VarChar).Value = Category_DropDownList.SelectedItem.Value.ToString();
                    cmd.Parameters.AddWithValue("@txtsearch", SqlDbType.VarChar).Value = txt_search.Text.ToString();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable DT = new DataTable();
                    adapter.Fill(DT);
                    DT.Columns.Add("FileData");
                    foreach (DataRow dr in DT.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["File"].ToString()))
                        {
                            dr["FileData"] = "Download";
                        }
                        else
                        {
                            dr["FileData"] = string.Empty;
                        }
                    }
                    string strscreenwidth = System.Web.HttpContext.Current.Request.Cookies.Get("screenwidth").Value;
                    int prscreenwidth = Convert.ToInt32(strscreenwidth) * 2 / 100;
                    HelpCenter_GridView.Width = Convert.ToInt32(strscreenwidth) - prscreenwidth;
                    foreach (DataControlField column in HelpCenter_GridView.Columns)
                    {
                        if (column.HeaderText == "Question" && column.GetType() != typeof(ButtonField))
                        {
                            column.Visible = false;
                        }
                        else if (column.HeaderText == "Question" && column.GetType() == typeof(ButtonField))
                        {
                            column.Visible = true;
                        }
                        if (column.HeaderText == "Question")
                        {
                            column.ItemStyle.Width = Unit.Percentage(30);
                        }
                        else if (column.HeaderText == "Article")
                        {
                            column.ItemStyle.Width = Unit.Percentage(50);
                        }
                        else
                        {
                            column.ItemStyle.Width = Unit.Percentage(20);
                        }
                    }
                    HelpCenter_GridView.DataSource = DT;
                    HelpCenter_GridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ExceptionTrackingViewController ExceptionController = new ExceptionTrackingViewController();
                ExceptionController.Frame.GetController<ExceptionTrackingViewController>().InsertAspxException(ex.Message.ToString(), ex.StackTrace.ToString(), this.GetType().Name.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name.ToString());
            }
        }
    }
}