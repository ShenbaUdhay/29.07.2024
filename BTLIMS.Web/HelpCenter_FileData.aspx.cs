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

namespace LDM.Web
{
    public partial class HelpCenter_FileData : System.Web.UI.Page
    {
        VersionControlInfo objVersion = new VersionControlInfo();
        HelpCenterInfo objhelpinfo = new HelpCenterInfo();
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
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_FileDate_GridView.HeaderStyle.BackColor = Color.FromArgb(186, 214, 253);
                            lbl_FormName0.ForeColor = HelpCenter_FileDate_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Gray;
                            HelpCenter_FileDate_GridView.RowStyle.BackColor = HelpCenter_FileDate_GridView.BorderColor = Color.FromArgb(224, 237, 255);
                        }
                        else if (objhelpinfo.strcrtTheme == "BlackGlass")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_FileDate_GridView.HeaderStyle.BackColor = Color.FromArgb(42, 42, 42);
                            lbl_FormName0.ForeColor = HelpCenter_FileDate_GridView.HeaderStyle.ForeColor = Color.White;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Gray;
                            HelpCenter_FileDate_GridView.RowStyle.BackColor = HelpCenter_FileDate_GridView.BorderColor = Color.FromArgb(147, 167, 186);
                        }
                        else if (objhelpinfo.strcrtTheme == "DevEx")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_FileDate_GridView.HeaderStyle.BackColor = Color.FromArgb(238, 238, 243);
                            lbl_FormName0.ForeColor = HelpCenter_FileDate_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Black;
                            HelpCenter_FileDate_GridView.RowStyle.BackColor = HelpCenter_FileDate_GridView.BorderColor = Color.FromArgb(244, 245, 248);
                        }
                        else if (objhelpinfo.strcrtTheme == "Glass")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_FileDate_GridView.HeaderStyle.BackColor = Color.FromArgb(194, 229, 239);
                            HelpCenter_FileDate_GridView.RowStyle.BackColor = HelpCenter_FileDate_GridView.BorderColor = Color.FromArgb(189, 226, 239);
                            lbl_FormName0.ForeColor = HelpCenter_FileDate_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Black;
                        }
                        else if (objhelpinfo.strcrtTheme == "Office2003Blue")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_FileDate_GridView.HeaderStyle.BackColor = Color.FromArgb(164, 198, 248);
                            HelpCenter_FileDate_GridView.RowStyle.BackColor = HelpCenter_FileDate_GridView.BorderColor = Color.FromArgb(215, 230, 248);
                            lbl_FormName0.ForeColor = HelpCenter_FileDate_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Black;
                        }
                        else if (objhelpinfo.strcrtTheme == "Office2003Olive")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_FileDate_GridView.HeaderStyle.BackColor = Color.FromArgb(214, 220, 187);
                            HelpCenter_FileDate_GridView.RowStyle.BackColor = HelpCenter_FileDate_GridView.BorderColor = Color.FromArgb(255, 255, 213);
                            lbl_FormName0.ForeColor = HelpCenter_FileDate_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Black;
                        }
                        else if (objhelpinfo.strcrtTheme == "Office2003Silver")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_FileDate_GridView.HeaderStyle.BackColor = Color.FromArgb(208, 208, 222);
                            HelpCenter_FileDate_GridView.RowStyle.BackColor = HelpCenter_FileDate_GridView.BorderColor = Color.FromArgb(230, 230, 238);
                            lbl_FormName0.ForeColor = HelpCenter_FileDate_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Black;
                        }
                        else if (objhelpinfo.strcrtTheme == "Office2010Black")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_FileDate_GridView.HeaderStyle.BackColor = Color.FromArgb(39, 39, 39);
                            HelpCenter_FileDate_GridView.RowStyle.BackColor = HelpCenter_FileDate_GridView.BorderColor = Color.FromArgb(163, 162, 163);
                            lbl_FormName0.ForeColor = HelpCenter_FileDate_GridView.HeaderStyle.ForeColor = Color.White;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Gray;
                        }
                        else if (objhelpinfo.strcrtTheme == "Office2010Blue")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_FileDate_GridView.HeaderStyle.BackColor = Color.FromArgb(211, 228, 246);
                            HelpCenter_FileDate_GridView.RowStyle.BackColor = HelpCenter_FileDate_GridView.BorderColor = Color.FromArgb(180, 202, 227);
                            lbl_FormName0.ForeColor = HelpCenter_FileDate_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Black;
                        }
                        else if (objhelpinfo.strcrtTheme == "Office2010Silver")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_FileDate_GridView.HeaderStyle.BackColor = Color.FromArgb(210, 213, 218);
                            HelpCenter_FileDate_GridView.RowStyle.BackColor = HelpCenter_FileDate_GridView.BorderColor = Color.FromArgb(255, 255, 213);
                            lbl_FormName0.ForeColor = HelpCenter_FileDate_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Black;
                        }
                        else if (objhelpinfo.strcrtTheme == "PlasticBlue")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_FileDate_GridView.HeaderStyle.BackColor = Color.FromArgb(60, 79, 145);
                            HelpCenter_FileDate_GridView.RowStyle.BackColor = HelpCenter_FileDate_GridView.BorderColor = Color.FromArgb(145, 162, 202);
                            lbl_FormName0.ForeColor = HelpCenter_FileDate_GridView.HeaderStyle.ForeColor = Color.White;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Gray;
                        }
                        else if (objhelpinfo.strcrtTheme == "RedWine")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_FileDate_GridView.HeaderStyle.BackColor = Color.FromArgb(120, 0, 43);
                            HelpCenter_FileDate_GridView.RowStyle.BackColor = HelpCenter_FileDate_GridView.BorderColor = Color.FromArgb(208, 77, 126);
                            lbl_FormName0.ForeColor = HelpCenter_FileDate_GridView.HeaderStyle.ForeColor = Color.White;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Gray;
                        }
                        else if (objhelpinfo.strcrtTheme == "SoftOrange")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_FileDate_GridView.HeaderStyle.BackColor = Color.FromArgb(233, 233, 233);
                            HelpCenter_FileDate_GridView.RowStyle.BackColor = HelpCenter_FileDate_GridView.BorderColor = Color.FromArgb(255, 159, 67);
                            lbl_FormName0.ForeColor = HelpCenter_FileDate_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Black;
                        }
                        else if (objhelpinfo.strcrtTheme == "Youthful")
                        {
                            Heading_Panel.BackColor = lbl_FormName0.BackColor = HelpCenter_FileDate_GridView.HeaderStyle.BackColor = Color.FromArgb(138, 161, 80);
                            HelpCenter_FileDate_GridView.RowStyle.BackColor = HelpCenter_FileDate_GridView.BorderColor = Color.FromArgb(229, 238, 207);
                            lbl_FormName0.ForeColor = HelpCenter_FileDate_GridView.HeaderStyle.ForeColor = Color.Black;
                            lbl_owns.ForeColor = lbl_Version.ForeColor = Color.Black;
                        }
                    }

                    string strscreenwidth = System.Web.HttpContext.Current.Request.Cookies.Get("screenwidth").Value;
                    int prscreenwidth = Convert.ToInt32(strscreenwidth) * 2 / 100;
                    Heading_Panel.Width = Convert.ToInt32(strscreenwidth); //- prscreenwidth;
                    HelpCenter_FileDate_GridView.Width = Convert.ToInt32(strscreenwidth); // - prscreenwidth;
                    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    string selectSQL = "SP_HelpCenter_FileData";
                    HelpCenterInfo helpCenterInfo = new HelpCenterInfo();
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand(selectSQL, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Topic", SqlDbType.VarChar).Value = helpCenterInfo.StrTopic;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable DT = new DataTable();
                    adapter.Fill(DT);
                    HelpCenter_FileDate_GridView.DataSource = DT;
                    HelpCenter_FileDate_GridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        protected void CustomersGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = HelpCenter_FileDate_GridView.Rows[index];
                string strFilename = selectedRow.Cells[0].Text;
                HelpCenterInfo helpCenterInfo = new HelpCenterInfo();
                helpCenterInfo.StrDownloadTopic = strFilename;
                HelpCenterController helpCenterController = new HelpCenterController();
                helpCenterController.downloadfile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}