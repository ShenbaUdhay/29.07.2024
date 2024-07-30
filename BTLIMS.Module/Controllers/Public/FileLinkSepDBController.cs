using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LDM.Module.Controllers.Public
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class FileLinkSepDBController : ViewController
    {
        public FileLinkSepDBController()
        {
            InitializeComponent();
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
   
        public void FileLink(string ReportId, byte[] ms)
        {
            string WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (!string.IsNullOrEmpty(WebConfigConn))
            {
                System.Data.SqlClient.SqlConnection objconection = new System.Data.SqlClient.SqlConnection(WebConfigConn);

                
                using (SqlCommand sqlCommand = new SqlCommand("Report_Insert_SP", objconection))
                {
                    objconection.Open();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@ReportID", ReportId);
                    param[1] = new SqlParameter("@FileContent", ms);
                    //param[2] = new SqlParameter("ReportName", ms);
                    sqlCommand.Parameters.AddRange(param);
                    sqlCommand.ExecuteNonQuery();
                    objconection.Close();
                }


                //IObjectSpace objspace = Application.CreateObjectSpace();
                //Modules.BusinessObjects.SampleManagement.Reporting report = objspace.CreateObject<Modules.BusinessObjects.SampleManagement.Reporting>();
                //if (report != null)
                //{

                //    ReportId = report.ReportID;
                //    SqlCommand cmd = new SqlCommand("insert into Report(ReportID)values('" + ReportId + "')", objconection);
                //    cmd.ExecuteNonQuery();
                //    //objconection.Close();

                //MemoryStream tempms = new MemoryStream();
                // XtraReport xtraReport = new XtraReport(ms);

                //xtraReport = XtraReport.FromStream(tempms, true);
                //tempms.ToArray();

                //foreach (var parameter in xtraReport.Parameters) parameter.Visible = true;
                //xtraReport.CreateDocument();
                //var memoryStream = new MemoryStream();
                //xtraReport.SaveLayoutToXml(memoryStream);
                //memoryStream.Position = 0;
                //memoryStream.ToArray();

                //}


            }

        }
        public void FileLinkUpdate(string ReportId, byte[] ms)
        {
            string WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (!string.IsNullOrEmpty(WebConfigConn))
            {
                System.Data.SqlClient.SqlConnection objconection = new System.Data.SqlClient.SqlConnection(WebConfigConn);


                using (SqlCommand sqlCommand = new SqlCommand("Report_update_SP", objconection))
                {
                    objconection.Open();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@ReportID", ReportId);
                    param[1] = new SqlParameter("@FileContent", ms);
                    //param[2] = new SqlParameter("ReportName", ms);
                    sqlCommand.Parameters.AddRange(param);
                    sqlCommand.ExecuteNonQuery();
                    objconection.Close();
                }


                //IObjectSpace objspace = Application.CreateObjectSpace();
                //Modules.BusinessObjects.SampleManagement.Reporting report = objspace.CreateObject<Modules.BusinessObjects.SampleManagement.Reporting>();
                //if (report != null)
                //{

                //    ReportId = report.ReportID;
                //    SqlCommand cmd = new SqlCommand("insert into Report(ReportID)values('" + ReportId + "')", objconection);
                //    cmd.ExecuteNonQuery();
                //    //objconection.Close();

                //MemoryStream tempms = new MemoryStream();
                // XtraReport xtraReport = new XtraReport(ms);

                //xtraReport = XtraReport.FromStream(tempms, true);
                //tempms.ToArray();

                //foreach (var parameter in xtraReport.Parameters) parameter.Visible = true;
                //xtraReport.CreateDocument();
                //var memoryStream = new MemoryStream();
                //xtraReport.SaveLayoutToXml(memoryStream);
                //memoryStream.Position = 0;
                //memoryStream.ToArray();

                //}


            }

        }
        public DataTable GetFileLink(string ReportId )
        {
            string WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            //DataTable datatab1e = new DataTable();
            DataTable dt = new DataTable();

            if (!string.IsNullOrEmpty(WebConfigConn))
            {
                System.Data.SqlClient.SqlConnection objconection = new System.Data.SqlClient.SqlConnection(WebConfigConn);


                //using (SqlCommand sqlCommand = new SqlCommand("GETREPORT_SP", objconection))
                //{
                //    objconection.Open();
                //    sqlCommand.CommandType = CommandType.StoredProcedure;
                //    SqlParameter[] param = new SqlParameter[1];
                //    param[0] = new SqlParameter("@ReportID", ReportId);
                //    //param[1] = new SqlParameter("@ReportName", ReportName);
                //    //param[1] = new SqlParameter("@FileContent", ms);
                //    sqlCommand.Parameters.AddRange(param);
                //    //sqlCommand.ExecuteNonQuery();
                //    SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                //    dataAdapter.Fill(datatab1e);
                //    objconection.Close();

                //}
                using (SqlCommand sqlCommand = new SqlCommand("GETREPORT_SP", objconection))
                {
                    objconection.Open();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@ReportID", ReportId);
                    sqlCommand.Parameters.AddRange(param);
                    using (SqlDataAdapter sda = new SqlDataAdapter(sqlCommand))
                    {
                        sda.Fill(dt);
                    }
                    //using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    //{
                    //    // Check if the result set has rows
                    //    if (reader.HasRows)
                    //    {
                    //        // Iterate through the rows
                    //        while (reader.Read())
                    //        {
                    //            // Get the varbinary value from the specified column
                    //            byte[] varbinaryData = (byte[])reader["FileContent"];

                    //        }
                    //    }
                    //}
                    objconection.Close();
                }

            }
            return dt;

        }

        public void RawDataLink(string CocReportId, byte[] CocFile)
        {

            string WebConfigConn = ConfigurationManager.ConnectionStrings["ReportingString"].ToString();
            if (!string.IsNullOrEmpty(WebConfigConn))
            {
                System.Data.SqlClient.SqlConnection objconection = new System.Data.SqlClient.SqlConnection(WebConfigConn);


                using (SqlCommand sqlCommand = new SqlCommand("RawData_Insert_SP", objconection))
                {
                    objconection.Open();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@COCReportID", CocReportId);
                    param[1] = new SqlParameter("@COCFileContent", CocFile);
                    //param[2] = new SqlParameter("ReportName", ms);
                    sqlCommand.Parameters.AddRange(param);
                    sqlCommand.ExecuteNonQuery();
                    objconection.Close();
                }



            }
        }


        public DataTable GetRawDataLink(string CocReportId)
        {

            
            string WebConfigConn = ConfigurationManager.ConnectionStrings["ReportingString"].ToString();
            DataTable dt = new DataTable();

            if (!string.IsNullOrEmpty(WebConfigConn))
            {
                System.Data.SqlClient.SqlConnection objconection = new System.Data.SqlClient.SqlConnection(WebConfigConn);


                using (SqlCommand sqlCommand = new SqlCommand("GetRawData_SP", objconection))
                {
                    objconection.Open();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@CocReportID", CocReportId);
                    sqlCommand.Parameters.AddRange(param);
                    using (SqlDataAdapter sda = new SqlDataAdapter(sqlCommand))
                    {
                        sda.Fill(dt);
                    }
                    
                    objconection.Close();
                }

            }
            return dt;
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
