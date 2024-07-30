using DevExpress.XtraReports.UI;
using Modules.BusinessObjects.InfoClass;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;

namespace Modules.BusinessObjects.Report
{
    public partial class COC_Report : DevExpress.XtraReports.UI.XtraReport
    {
        public COC_Report()
        {
            InitializeComponent();
        }

        public void DataBindingsToReport()
        {
            lblCOCID.DataBindings.Add("Text", DataSource, "COCID");
            lblProjectID.DataBindings.Add("Text", DataSource, "ProjectId");
            lblProjectName.DataBindings.Add("Text", DataSource, "ProjectName");
            lblClientName.DataBindings.Add("Text", DataSource, "ClientName");
            lblAddress1.DataBindings.Add("Text", DataSource, "Address1");
            lblAddress2.DataBindings.Add("Text", DataSource, "Address2");
            lblContact.DataBindings.Add("Text", DataSource, "Contact");
            lblPhone.DataBindings.Add("Text", DataSource, "Phone");
            lblFax.DataBindings.Add("Text", DataSource, "Fax");
            lblEmail.DataBindings.Add("Text", DataSource, "Email");
            lblPO.DataBindings.Add("Text", DataSource, "PO#");
            lblQuoteNo.DataBindings.Add("Text", DataSource, "QuoteNo");
            lblDueDate.DataBindings.Add("Text", DataSource, "DueDate");
            lblTAT.DataBindings.Add("Text", DataSource, "TurnAroundTime");
            lblProjectLocation.DataBindings.Add("Text", DataSource, "ProjectLocation");
            lblReportRequirement.DataBindings.Add("Text", DataSource, "ReportRequirement");
            lblSamplerCompany.DataBindings.Add("Text", DataSource, "SamplerCompany");
            lblCollector.DataBindings.Add("Text", DataSource, "Collector");
            lblPreservativeCode.DataBindings.Add("Text", DataSource, "PresevitiveCode");
            tblTestName.DataBindings.Add("Text", DataSource, "TestName");
            tblCOCSampleID.DataBindings.Add("Text", DataSource, "COCSampleID");
            tblClientSampleID.DataBindings.Add("Text", DataSource, "ClientSampleID");
            tblSamplingDate.DataBindings.Add("Text", DataSource, "SamplingDate");
            tblSamplingTime.DataBindings.Add("Text", DataSource, "SamplingTime");
            tblContainers.DataBindings.Add("Text", DataSource, "Containers");
            tblMatrix.DataBindings.Add("Text", DataSource, "Matrix");
            tblSampleTestName.DataBindings.Add("Text", DataSource, "SampleTestName");
            tblComment.DataBindings.Add("Text", DataSource, "Comment");
            tblCreatedBy.DataBindings.Add("Text", DataSource, "CreatedBy");
            tblCreatedDate.DataBindings.Add("Text", DataSource, "CreatedDate");
            tblValidatedBy.DataBindings.Add("Text", DataSource, "ValidateBy");
            tblValidatedDate.DataBindings.Add("Text", DataSource, "ValidatedDate");
            tblLabComment.DataBindings.Add("Text", DataSource, "LabComment");


            //xrTableCell4.DataBindings.Add("Text",DataSource, "SampleID");
        }

        private void label2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ProjectDetailsInfo projectDetails = new ProjectDetailsInfo();
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ProjectName"].ToString()))
            {
                projectDetails.ProjectName = ConfigurationManager.AppSettings["ProjectName"].ToString();
            }
            if (sender != null && !string.IsNullOrEmpty(projectDetails.ProjectName))
            {
                if (projectDetails.ProjectName.ToUpper().Trim() == "OIL")
                {
                    label2.Text = "A & B Petroleum";
                    this.pictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(global::Modules.Properties.Resources.Oil_COC_Logo, true);
                }
                else if (projectDetails.ProjectName.ToUpper().Trim() == "SFL")
                {
                    label2.Text = "";
                    this.pictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(global::Modules.Properties.Resources.SFL_Logo, true);
                }
                else if (projectDetails.ProjectName.ToUpper().Trim() == "BATTA")
                {
                    label2.Text = "BATTA Laboratories-Corporate Office";
                    this.pictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(global::Modules.Properties.Resources.Batta_coc_Logo, true);
                }
                else if (projectDetails.ProjectName.ToUpper().Trim() == "CONSCI")
                {
                    label2.Text = "";
                    this.pictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(global::Modules.Properties.Resources.CONSCI_COC_LOGO, true);
                }
                else if (projectDetails.ProjectName.ToUpper().Trim() == "BTSOFT")
                {
                    label2.Text = "";
                    this.pictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(global::Modules.Properties.Resources.BTS_PrimaryLogo, true);
                    pictureBox1.WidthF = 140;
                    pictureBox1.HeightF = 80;
                    pictureBox1.LeftF = 60;
                }
                else
                {
                    label2.Text = string.Empty;
                }
            }
            else
            {
                label2.Text = string.Empty;
            }
        }
        private void tblTestName_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                //foreach(XRTableRow row in table5)
                if (GetCurrentColumnValue("TestName") != null)
                {
                    var val = GetCurrentColumnValue("TestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("TestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 11)
                            {
                                for (int i = 0; i < strAllTestsList.Length; i++)
                                {
                                    if (!System.String.IsNullOrEmpty(strAllTestsList[11]))
                                    {
                                        cell.Text = strAllTestsList[11];
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tblSampleTestName_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                XRTable tblTests = this.table5;
                XRTableRow rowTests = this.table5.Rows[0];

                //foreach(XRTableRow row in table6)
                if (GetCurrentColumnValue("SampleTestName") != null)
                {
                    var val = GetCurrentColumnValue("SampleTestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("SampleTestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 0)
                            {
                                foreach (string currentTest in strAllTestsList)
                                {
                                    if (!System.String.IsNullOrEmpty(currentTest) && currentTest == rowTests.Cells[11].Text)
                                    {
                                        cell.Text = "x";
                                        return;
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tableCell26_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                //foreach(XRTableRow row in table5)
                if (GetCurrentColumnValue("TestName") != null)
                {
                    var val = GetCurrentColumnValue("TestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("TestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 0)
                            {
                                for (int i = 0; i < strAllTestsList.Length; i++)
                                {
                                    if (!System.String.IsNullOrEmpty(strAllTestsList[0]))
                                    {
                                        cell.Text = strAllTestsList[0];
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tableCell27_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                //foreach(XRTableRow row in table5)
                if (GetCurrentColumnValue("TestName") != null)
                {
                    var val = GetCurrentColumnValue("TestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("TestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 1)
                            {
                                for (int i = 0; i < strAllTestsList.Length; i++)
                                {
                                    if (!System.String.IsNullOrEmpty(strAllTestsList[1]))
                                    {
                                        cell.Text = strAllTestsList[1];
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tableCell28_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                //foreach(XRTableRow row in table5)
                if (GetCurrentColumnValue("TestName") != null)
                {
                    var val = GetCurrentColumnValue("TestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("TestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 2)
                            {
                                for (int i = 0; i < strAllTestsList.Length; i++)
                                {
                                    if (!System.String.IsNullOrEmpty(strAllTestsList[2]))
                                    {
                                        cell.Text = strAllTestsList[2];
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tableCell29_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                //foreach(XRTableRow row in table5)
                if (GetCurrentColumnValue("TestName") != null)
                {
                    var val = GetCurrentColumnValue("TestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("TestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 3)
                            {
                                for (int i = 0; i < strAllTestsList.Length; i++)
                                {
                                    if (!System.String.IsNullOrEmpty(strAllTestsList[3]))
                                    {
                                        cell.Text = strAllTestsList[3];
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tableCell30_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                //foreach(XRTableRow row in table5)
                if (GetCurrentColumnValue("TestName") != null)
                {
                    var val = GetCurrentColumnValue("TestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("TestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 4)
                            {
                                for (int i = 0; i < strAllTestsList.Length; i++)
                                {
                                    if (!System.String.IsNullOrEmpty(strAllTestsList[4]))
                                    {
                                        cell.Text = strAllTestsList[4];
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tableCell31_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                //foreach(XRTableRow row in table5)
                if (GetCurrentColumnValue("TestName") != null)
                {
                    var val = GetCurrentColumnValue("TestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("TestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 5)
                            {
                                for (int i = 0; i < strAllTestsList.Length; i++)
                                {
                                    if (!System.String.IsNullOrEmpty(strAllTestsList[5]))
                                    {
                                        cell.Text = strAllTestsList[5];
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tableCell32_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                //foreach(XRTableRow row in table5)
                if (GetCurrentColumnValue("TestName") != null)
                {
                    var val = GetCurrentColumnValue("TestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("TestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 6)
                            {
                                for (int i = 0; i < strAllTestsList.Length; i++)
                                {
                                    if (!System.String.IsNullOrEmpty(strAllTestsList[6]))
                                    {
                                        cell.Text = strAllTestsList[6];
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tableCell33_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                //foreach(XRTableRow row in table5)
                if (GetCurrentColumnValue("TestName") != null)
                {
                    var val = GetCurrentColumnValue("TestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("TestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 7)
                            {
                                for (int i = 0; i < strAllTestsList.Length; i++)
                                {
                                    if (!System.String.IsNullOrEmpty(strAllTestsList[7]))
                                    {
                                        cell.Text = strAllTestsList[7];
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }
        }

        private void tableCell34_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                //foreach(XRTableRow row in table5)
                if (GetCurrentColumnValue("TestName") != null)
                {
                    var val = GetCurrentColumnValue("TestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("TestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 8)
                            {
                                for (int i = 0; i < strAllTestsList.Length; i++)
                                {
                                    if (!System.String.IsNullOrEmpty(strAllTestsList[8]))
                                    {
                                        cell.Text = strAllTestsList[8];
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tableCell35_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                //foreach(XRTableRow row in table5)
                if (GetCurrentColumnValue("TestName") != null)
                {
                    var val = GetCurrentColumnValue("TestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("TestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 9)
                            {
                                for (int i = 0; i < strAllTestsList.Length; i++)
                                {
                                    if (!System.String.IsNullOrEmpty(strAllTestsList[9]))
                                    {
                                        cell.Text = strAllTestsList[9];
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }
        private void tableCell47_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            //var sampler = GetCurrentColumnValue("Sampler");
            //string strfullcollector = string.Empty;
            //if (sampler != null && sampler.GetType() != typeof(System.DBNull))
            //{
            //    foreach (string row in (IEnumerable<string>)sampler)
            //    {
            //        string strSampler = (System.String)row;

            //        if (!string.IsNullOrEmpty(strSampler))
            //        {
            //            strfullcollector += strSampler + ", ";`
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(strfullcollector))
            //    {
            //        strfullcollector = strfullcollector.Substring(0, strfullcollector.Length - 2);
            //    }
            //}

        }

        private void tableCell36_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                //foreach(XRTableRow row in table5)
                if (GetCurrentColumnValue("TestName") != null)
                {
                    var val = GetCurrentColumnValue("TestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("TestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 10)
                            {
                                for (int i = 0; i < strAllTestsList.Length; i++)
                                {
                                    if (!System.String.IsNullOrEmpty(strAllTestsList[10]))
                                    {
                                        cell.Text = strAllTestsList[10];
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }



        private void tableCell48_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                XRTable tblTests = this.table5;
                XRTableRow rowTests = this.table5.Rows[0];

                //foreach(XRTableRow row in table6)
                if (GetCurrentColumnValue("SampleTestName") != null)
                {
                    var val = GetCurrentColumnValue("SampleTestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("SampleTestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 0)
                            {
                                foreach (string currentTest in strAllTestsList)
                                {
                                    if (!System.String.IsNullOrEmpty(currentTest) && currentTest == rowTests.Cells[0].Text)
                                    {
                                        cell.Text = "x";
                                        return;
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }
        }

        private void tableCell49_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                XRTable tblTests = this.table5;
                XRTableRow rowTests = this.table5.Rows[0];

                //foreach(XRTableRow row in table6)
                if (GetCurrentColumnValue("SampleTestName") != null)
                {
                    var val = GetCurrentColumnValue("SampleTestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("SampleTestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 0)
                            {
                                foreach (string currentTest in strAllTestsList)
                                {
                                    if (!System.String.IsNullOrEmpty(currentTest) && currentTest == rowTests.Cells[1].Text)
                                    {
                                        cell.Text = "x";
                                        return;
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tableCell50_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                XRTable tblTests = this.table5;
                XRTableRow rowTests = this.table5.Rows[0];

                //foreach(XRTableRow row in table6)
                if (GetCurrentColumnValue("SampleTestName") != null)
                {
                    var val = GetCurrentColumnValue("SampleTestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("SampleTestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 0)
                            {
                                foreach (string currentTest in strAllTestsList)
                                {
                                    if (!System.String.IsNullOrEmpty(currentTest) && currentTest == rowTests.Cells[2].Text)
                                    {
                                        cell.Text = "x";
                                        return;
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tableCell51_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                XRTable tblTests = this.table5;
                XRTableRow rowTests = this.table5.Rows[0];

                //foreach(XRTableRow row in table6)
                if (GetCurrentColumnValue("SampleTestName") != null)
                {
                    var val = GetCurrentColumnValue("SampleTestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("SampleTestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 0)
                            {
                                foreach (string currentTest in strAllTestsList)
                                {
                                    if (!System.String.IsNullOrEmpty(currentTest) && currentTest == rowTests.Cells[3].Text)
                                    {
                                        cell.Text = "x";
                                        return;
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tableCell52_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                XRTable tblTests = this.table5;
                XRTableRow rowTests = this.table5.Rows[0];

                //foreach(XRTableRow row in table6)
                if (GetCurrentColumnValue("SampleTestName") != null)
                {
                    var val = GetCurrentColumnValue("SampleTestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("SampleTestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 0)
                            {
                                foreach (string currentTest in strAllTestsList)
                                {
                                    if (!System.String.IsNullOrEmpty(currentTest) && currentTest == rowTests.Cells[4].Text)
                                    {
                                        cell.Text = "x";
                                        return;
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tableCell53_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                XRTable tblTests = this.table5;
                XRTableRow rowTests = this.table5.Rows[0];

                //foreach(XRTableRow row in table6)
                if (GetCurrentColumnValue("SampleTestName") != null)
                {
                    var val = GetCurrentColumnValue("SampleTestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("SampleTestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 0)
                            {
                                foreach (string currentTest in strAllTestsList)
                                {
                                    if (!System.String.IsNullOrEmpty(currentTest) && currentTest == rowTests.Cells[5].Text)
                                    {
                                        cell.Text = "x";
                                        return;
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tableCell54_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                XRTable tblTests = this.table5;
                XRTableRow rowTests = this.table5.Rows[0];

                //foreach(XRTableRow row in table6)
                if (GetCurrentColumnValue("SampleTestName") != null)
                {
                    var val = GetCurrentColumnValue("SampleTestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("SampleTestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 0)
                            {
                                foreach (string currentTest in strAllTestsList)
                                {
                                    if (!System.String.IsNullOrEmpty(currentTest) && currentTest == rowTests.Cells[6].Text)
                                    {
                                        cell.Text = "x";
                                        return;
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tableCell55_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                XRTable tblTests = this.table5;
                XRTableRow rowTests = this.table5.Rows[0];

                //foreach(XRTableRow row in table6)
                if (GetCurrentColumnValue("SampleTestName") != null)
                {
                    var val = GetCurrentColumnValue("SampleTestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("SampleTestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 0)
                            {
                                foreach (string currentTest in strAllTestsList)
                                {
                                    if (!System.String.IsNullOrEmpty(currentTest) && currentTest == rowTests.Cells[7].Text)
                                    {
                                        cell.Text = "x";
                                        return;
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tableCell56_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                XRTable tblTests = this.table5;
                XRTableRow rowTests = this.table5.Rows[0];

                //foreach(XRTableRow row in table6)
                if (GetCurrentColumnValue("SampleTestName") != null)
                {
                    var val = GetCurrentColumnValue("SampleTestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("SampleTestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 0)
                            {
                                foreach (string currentTest in strAllTestsList)
                                {
                                    if (!System.String.IsNullOrEmpty(currentTest) && currentTest == rowTests.Cells[8].Text)
                                    {
                                        cell.Text = "x";
                                        return;
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tableCell57_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                XRTable tblTests = this.table5;
                XRTableRow rowTests = this.table5.Rows[0];

                //foreach(XRTableRow row in table6)
                if (GetCurrentColumnValue("SampleTestName") != null)
                {
                    var val = GetCurrentColumnValue("SampleTestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("SampleTestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 0)
                            {
                                foreach (string currentTest in strAllTestsList)
                                {
                                    if (!System.String.IsNullOrEmpty(currentTest) && currentTest == rowTests.Cells[9].Text)
                                    {
                                        cell.Text = "x";
                                        return;
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void tableCell58_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sender != null)
            {
                XRTableCell cell = sender as XRTableCell;
                XRTable tblTests = this.table5;
                XRTableRow rowTests = this.table5.Rows[0];

                //foreach(XRTableRow row in table6)
                if (GetCurrentColumnValue("SampleTestName") != null)
                {
                    var val = GetCurrentColumnValue("SampleTestName");
                    if (val.GetType() != typeof(System.DBNull))
                    {
                        System.String strAllTestName = GetCurrentColumnValue("SampleTestName").ToString();//row.Cells[11].Value.ToString();
                        if (!System.String.IsNullOrEmpty(strAllTestName))
                        {
                            string[] strAllTestsList = strAllTestName.Split('^');
                            if (strAllTestsList != null && strAllTestsList.Length > 0)
                            {
                                foreach (string currentTest in strAllTestsList)
                                {
                                    if (!System.String.IsNullOrEmpty(currentTest) && currentTest == rowTests.Cells[10].Text)
                                    {
                                        cell.Text = "x";
                                        return;
                                    }
                                    else
                                    {
                                        cell.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                cell.Text = "";
                            }
                        }
                        else
                        {
                            cell.Text = "";
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
                else
                {
                    cell.Text = "";
                }
            }

        }

        private void COC_Report_FillEmptySpace(object sender, BandEventArgs e)
        {
            XRTable sourceTable = this.table7;
            XRTableRow xrTableRowSource = this.table7.Rows[0];
            XRTable table = XRTable.CreateTable(new RectangleF(new PointF(sourceTable.LocationF.X, 0.0F),
                     new SizeF(sourceTable.SizeF.Width, e.Band.HeightF)), 20, xrTableRowSource.Cells.Count);
            table.BeginInit();
            foreach (XRTableRow row in table)
            {
                for (int i = 0; i < xrTableRowSource.Cells.Count; i++)
                {
                    row.Cells[i].WidthF = xrTableRowSource.Cells[i].WidthF;
                    row.Cells[i].Borders = xrTableRowSource.Cells[i].GetEffectiveBorders();
                }
            }
            table.EndInit();
            e.Band.Controls.Add(table);

        }

        private void lblAddress2_EvaluateBinding(object sender, BindingEventArgs e)
        {
            var valcity = GetCurrentColumnValue("CustomerCity");
            var valstate = GetCurrentColumnValue("CustomerState");
            var valzip = GetCurrentColumnValue("CustomerZipCode");
            string strcity = string.Empty;
            string strstate = string.Empty;
            string strzip = string.Empty;
            if (valcity != null && valcity.GetType() != typeof(System.DBNull))
            {
                strcity = (System.String)GetCurrentColumnValue("CustomerCity");
            }
            if (valstate != null && valstate.GetType() != typeof(System.DBNull))
            {
                strstate = (System.String)GetCurrentColumnValue("CustomerState");
            }
            if (valzip != null && valzip.GetType() != typeof(System.DBNull))
            {
                strzip = (System.String)GetCurrentColumnValue("CustomerZipCode");
            }

            string strfullcitystatezip = string.Empty;
            if (!string.IsNullOrEmpty(strcity))
            {
                strfullcitystatezip = strcity;
            }
            if (!string.IsNullOrEmpty(strstate))
            {
                if (string.IsNullOrEmpty(strfullcitystatezip))
                {
                    strfullcitystatezip = strstate;
                }
                else
                {
                    strfullcitystatezip = strfullcitystatezip + ", " + strstate;
                }

            }
            if (!string.IsNullOrEmpty(strzip))
            {
                if (string.IsNullOrEmpty(strfullcitystatezip))
                {
                    strfullcitystatezip = strzip;
                }
                else
                {
                    strfullcitystatezip = strfullcitystatezip + ", " + strzip;
                }
            }
            if (!string.IsNullOrEmpty(strfullcitystatezip))
            {
                e.Value = strfullcitystatezip;
            }
            else
            {
                e.Value = "";
            }
        }
        private void lblAddress1_EvaluateBinding(object sender, BindingEventArgs e)
        {
            var valAddress = GetCurrentColumnValue("Addressline1");
            var valFullAddress = GetCurrentColumnValue("Addressline2");
            string strAddress1 = string.Empty;
            string strAddress2 = string.Empty;
            if (valAddress != null && valAddress.GetType() != typeof(System.DBNull))
            {
                strAddress1 = (System.String)GetCurrentColumnValue("Addressline1");
            }
            if (valFullAddress != null && valFullAddress.GetType() != typeof(System.DBNull))
            {
                strAddress2 = (System.String)GetCurrentColumnValue("Addressline2");
            }
            string strfulladdress = string.Empty;
            if (!string.IsNullOrEmpty(strAddress1))
            {
                strfulladdress = strAddress1;
            }
            if (!string.IsNullOrEmpty(strAddress2))
            {
                if (string.IsNullOrEmpty(strfulladdress))
                {
                    strfulladdress = strAddress2;
                }
                else
                {
                    strfulladdress = strfulladdress + ", " + strAddress2;
                }

            }
            if (!string.IsNullOrEmpty(strfulladdress))
            {
                e.Value = strfulladdress;
            }
            else
            {
                e.Value = "";
            }
        }

        private void tblTestName_EvaluateBinding(object sender, BindingEventArgs e)
        {
            foreach (XRTableRow row in table5)
            {
                if (row.Cells[0].Value == row.Cells[11].Value || row.Cells[1].Value == row.Cells[11].Value ||
                row.Cells[2].Value == row.Cells[11].Value || row.Cells[3].Value == row.Cells[11].Value ||
                row.Cells[4].Value == row.Cells[11].Value || row.Cells[5].Value == row.Cells[11].Value ||
                row.Cells[6].Value == row.Cells[11].Value || row.Cells[7].Value == row.Cells[11].Value ||
                row.Cells[8].Value == row.Cells[11].Value || row.Cells[9].Value == row.Cells[11].Value ||
                row.Cells[10].Value == row.Cells[11].Value)
                {
                    e.Value = "";
                }
            }

        }

        private void tableCell36_EvaluateBinding(object sender, BindingEventArgs e)
        {
            foreach (XRTableRow row in table5)
            {
                if (row.Cells[0].Value == row.Cells[10].Value || row.Cells[1].Value == row.Cells[10].Value ||
                row.Cells[2].Value == row.Cells[10].Value || row.Cells[3].Value == row.Cells[10].Value ||
                row.Cells[4].Value == row.Cells[10].Value || row.Cells[5].Value == row.Cells[10].Value ||
                row.Cells[6].Value == row.Cells[10].Value || row.Cells[7].Value == row.Cells[10].Value ||
                row.Cells[8].Value == row.Cells[10].Value || row.Cells[9].Value == row.Cells[10].Value)
                {
                    e.Value = "";
                }
            }

        }

        private void tableCell35_EvaluateBinding(object sender, BindingEventArgs e)
        {
            foreach (XRTableRow row in table5)
            {
                if (row.Cells[0].Value == row.Cells[9].Value || row.Cells[1].Value == row.Cells[9].Value ||
                row.Cells[2].Value == row.Cells[9].Value || row.Cells[3].Value == row.Cells[9].Value ||
                row.Cells[4].Value == row.Cells[9].Value || row.Cells[5].Value == row.Cells[9].Value ||
                row.Cells[6].Value == row.Cells[9].Value || row.Cells[7].Value == row.Cells[9].Value ||
                row.Cells[8].Value == row.Cells[9].Value)
                {
                    e.Value = "";
                }
            }

        }

        private void tableCell34_EvaluateBinding(object sender, BindingEventArgs e)
        {
            foreach (XRTableRow row in table5)
            {
                if (row.Cells[0].Value == row.Cells[8].Value || row.Cells[1].Value == row.Cells[8].Value ||
                row.Cells[2].Value == row.Cells[8].Value || row.Cells[3].Value == row.Cells[8].Value ||
                row.Cells[4].Value == row.Cells[8].Value || row.Cells[5].Value == row.Cells[8].Value ||
                row.Cells[6].Value == row.Cells[8].Value || row.Cells[7].Value == row.Cells[8].Value)
                {
                    e.Value = "";
                }
            }

        }

        private void tableCell33_EvaluateBinding(object sender, BindingEventArgs e)
        {
            foreach (XRTableRow row in table5)
            {
                if (row.Cells[0].Value == row.Cells[7].Value || row.Cells[1].Value == row.Cells[7].Value ||
                row.Cells[2].Value == row.Cells[7].Value || row.Cells[3].Value == row.Cells[7].Value ||
                row.Cells[4].Value == row.Cells[7].Value || row.Cells[5].Value == row.Cells[7].Value ||
                row.Cells[6].Value == row.Cells[7].Value)
                {
                    e.Value = "";
                }
            }

        }

        private void tableCell32_EvaluateBinding(object sender, BindingEventArgs e)
        {
            foreach (XRTableRow row in table5)
            {
                if (row.Cells[0].Value == row.Cells[6].Value || row.Cells[1].Value == row.Cells[6].Value ||
                row.Cells[2].Value == row.Cells[6].Value || row.Cells[3].Value == row.Cells[6].Value ||
                row.Cells[4].Value == row.Cells[6].Value || row.Cells[5].Value == row.Cells[6].Value)
                {
                    e.Value = "";
                }
            }

        }

        private void tableCell31_EvaluateBinding(object sender, BindingEventArgs e)
        {
            foreach (XRTableRow row in table5)
            {
                if (row.Cells[0].Value == row.Cells[5].Value || row.Cells[1].Value == row.Cells[5].Value ||
                row.Cells[2].Value == row.Cells[5].Value || row.Cells[3].Value == row.Cells[5].Value ||
                row.Cells[4].Value == row.Cells[5].Value)
                {
                    e.Value = "";
                }
            }

        }

        private void tableCell30_EvaluateBinding(object sender, BindingEventArgs e)
        {
            foreach (XRTableRow row in table5)
            {
                if (row.Cells[0].Value == row.Cells[4].Value || row.Cells[1].Value == row.Cells[4].Value ||
                row.Cells[2].Value == row.Cells[4].Value || row.Cells[3].Value == row.Cells[4].Value)
                {
                    e.Value = "";
                }
            }

        }

        private void tableCell29_EvaluateBinding(object sender, BindingEventArgs e)
        {
            foreach (XRTableRow row in table5)
            {
                if (row.Cells[0].Value == row.Cells[3].Value || row.Cells[1].Value == row.Cells[3].Value ||
                row.Cells[2].Value == row.Cells[3].Value)
                {
                    e.Value = "";
                }
            }

        }

        private void tableCell28_EvaluateBinding(object sender, BindingEventArgs e)
        {
            foreach (XRTableRow row in table5)
            {
                if (row.Cells[0].Value == row.Cells[2].Value || row.Cells[1].Value == row.Cells[2].Value)
                {
                    e.Value = "";
                }
            }

        }

        private void tableCell27_EvaluateBinding(object sender, BindingEventArgs e)
        {
            foreach (XRTableRow row in table5)
            {
                if (row.Cells[0].Value == row.Cells[1].Value)
                {
                    e.Value = "";
                }
            }

        }

        private void tableCell26_EvaluateBinding(object sender, BindingEventArgs e)
        {
            foreach (XRTableRow row in table5)
            {
                if (row.Cells[11].Value != null)
                {
                    System.String strAllTestName = row.Cells[11].Text.ToString();
                    if (!System.String.IsNullOrEmpty(strAllTestName))
                    {
                        string[] strAllTestsList = strAllTestName.Split('^');
                        for (int i = 0; i < strAllTestsList.Length; i++)
                        {
                            e.Value = strAllTestsList[0];
                        }
                    }
                }
            }

        }

        private void lblCollector_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //DataTable dt = (DataTable)DataSource;
            //string strfullcollector = string.Empty;
            //foreach (DataRow dr in dt.Rows )
            //{
            //    if (strfullcollector == null)
            //    {
            //        strfullcollector = dr["Sampler"].ToString ();
            //    }
            //    else
            //    {
            //        strfullcollector = strfullcollector + ',' + dr["Sampler"].ToString ();
            //    }
            //}
            //lblCollector.Text = strfullcollector;


            DataTable dt = (DataTable)DataSource;
            HashSet<string> uniqueCollectors = new HashSet<string>();

            foreach (DataRow dr in dt.Rows)
            {
                string collector = dr["Sampler"].ToString();
                if (!uniqueCollectors.Contains(collector))
                {
                    uniqueCollectors.Add(collector);
                }
            }

            string strfullcollector = string.Join(", ", uniqueCollectors);
            lblCollector.Text = strfullcollector;


        }
    }
}
