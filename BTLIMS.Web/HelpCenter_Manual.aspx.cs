using Modules.BusinessObjects.InfoClass;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

namespace LDM.Web
{
    public partial class HelpCenter_Manual : System.Web.UI.Page
    {
        string contentbyte = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            HelpCenterInfo helpCenterInfo = new HelpCenterInfo();
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string selectSQL = "SP_HelpCenter_ManualArticle";
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(selectSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Topic", SqlDbType.VarChar).Value = helpCenterInfo.StrTopic;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable DT = new DataTable();
            adapter.Fill(DT);
            if (DT != null && DT.Rows.Count > 1)
            {

            }
            else if (DT != null && DT.Rows.Count == 1)
            {
                string HPTopic = string.Empty;
                string contentbyte = string.Empty;
                byte[] bytes = new byte[100];
                foreach (DataRow dr in DT.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["Topic"].ToString()))
                    {
                        HPTopic = dr["Topic"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dr["Article"].ToString()))
                    {
                        contentbyte = dr["Article"].ToString();

                        //string strcontent = Encoding.ASCII.GetBytes(contentbyte);
                        //bytes = (byte[])dr["Article"];
                        //bytes = (byte[])strcontent;
                        //blob = new Byte[(dr["Content"].GetBytes(0, 0, null, 0, int.MaxValue))];
                    }
                }
                string strscreenwidth = System.Web.HttpContext.Current.Request.Cookies.Get("screenwidth").Value;
                int prscreenwidth = Convert.ToInt32(strscreenwidth) * 2 / 100;
                ASPxRichEdit1.Width = Convert.ToInt32(strscreenwidth) - prscreenwidth;
                string strheight = System.Web.HttpContext.Current.Request.Cookies.Get("height").Value;
                int prscreenheight = Convert.ToInt32(strheight) * 2 / 100;
                ASPxRichEdit1.Height = Convert.ToInt32(strheight) - prscreenwidth;
                ASPxRichEdit1.DataSource = contentbyte;
                ASPxRichEdit1.DataBind();
                string strLocalFile = HttpContext.Current.Server.MapPath((@"~\HPManual\" + HPTopic + ".rtf"));
                if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\HPManual\")) == false)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\HPManual\"));
                }
                if (File.Exists(strLocalFile))
                {
                    File.Delete(strLocalFile);
                }
                File.WriteAllText(strLocalFile, contentbyte);
                ASPxRichEdit1.Open(strLocalFile, DevExpress.XtraRichEdit.DocumentFormat.Rtf);

                // Instantiate Viewer


            }

        }
    }
}