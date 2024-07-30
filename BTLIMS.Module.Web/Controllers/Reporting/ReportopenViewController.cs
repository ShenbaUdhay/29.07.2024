using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Pdf;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
//using Rebex.Net;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web;

namespace LDM.Module.Web.Controllers.Reporting
{
    public partial class ReportopenViewController : ViewController, IXafCallbackHandler
    {
        public string strFTPServerName = string.Empty;
        public string strFTPPath = string.Empty;
        public string strFTPUserName = string.Empty;
        public string strFTPPassword = string.Empty;
        public int FTPPort = 0;
        public bool strFTPStatus;
        string strExportedPath = string.Empty;
        DynamicReportDesignerConnection objRInfo = new DynamicReportDesignerConnection();
        MessageTimer timer = new MessageTimer();
        curlanguage objLanguage = new curlanguage();
        public ReportopenViewController()
        {
            InitializeComponent();
            TargetViewId = "Reporting_ListView_Copy_ReportView";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                ASPxGridView gridView = gridListEditor.Grid;
                gridView.ClientInstanceName = View.Id;
                ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                selparameter.CallbackManager.RegisterHandler("ReportView", this);
                gridView.HtmlDataCellPrepared += GridView_HtmlDataCellPrepared;
                //gridView.CustomCallback += GridView_CustomCallback;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void GridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                if (e.Parameters != null && e.Parameters.Length > 0)
                {
                    ShowReport(e.Parameters);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void GridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                ASPxGridView grid = sender as ASPxGridView;
                if (e.DataColumn.FieldName != "ReportID") return;
                //e.Cell.Attributes.Add("onclick", string.Format("{0}.PerformCallback(this.innerText);", grid.ClientInstanceName));
                //e.Cell.Attributes.Add("onclick", "RaiseXafCallback(globalCallbackControl, 'ReportView', this.innerText, '', false);");
                e.Cell.Attributes.Add("ondblclick", "RaiseXafCallback(globalCallbackControl, 'ReportView', this.innerText, '', false);");
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        private void ShowReport(string ReportID)
        {
            try
            {
                //objRInfo.WebConfigFTPConn = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FTPConnectionStrings"))["FTPConnectionString"];
                //ReadXmlFile_FTPConc();
                //Rebex.Net.Ftp _FTP = GetFTPConnection();
                //string strTempPath = Path.GetTempPath();
                //String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                //strExportedPath = "//ReportPreview//FinalReports//" + ReportID + ".pdf";
                string WatermarkText;
                if (objLanguage.strcurlanguage == "En")
                {
                    WatermarkText = "Approved";
                }
                else
                {
                    WatermarkText = ConfigurationManager.AppSettings["ReportWaterMarkText"];
                }
                if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportSave")) == false)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportSave"));
                }

                //if (_FTP.FileExists(strExportedPath))
                FileLinkSepDBController objfilelink = Frame.GetController<FileLinkSepDBController>();
                if (objfilelink != null && !string.IsNullOrEmpty(ReportID))
                {
                    //string strDesginationPath = HttpContext.Current.Server.MapPath(@"~\ReportSave\" + timeStamp + ".pdf");

                    //string strpath = strExportedPath;

                    //strExportedPath = "ftp://" + strFTPUserName + ":" + strFTPPassword + "@" + strFTPServerName + ":" + FTPPort + "//ReportPreview//FinalReports//" + ReportID + ".pdf";
                    //Download(strExportedPath, strDesginationPath);
                    DataTable dt = objfilelink.GetFileLink(ReportID);
                    if (dt != null && dt.Rows.Count > 0 && dt.Rows[0] != null && dt.Rows[0]["FileContent"].GetType() == typeof(byte[]))
                    {
                        byte[] objbyte = (byte[])dt.Rows[0]["FileContent"];
                        MemoryStream ms = new MemoryStream(objbyte);
                        MemoryStream tempms = new MemoryStream();
                        using (PdfDocumentProcessor documentProcessor = new PdfDocumentProcessor())
                        {
                            string fontName = "Microsoft Yahei";
                            int fontSize = 25;
                            PdfStringFormat stringFormat = PdfStringFormat.GenericTypographic;
                            stringFormat.Alignment = PdfStringAlignment.Center;
                            stringFormat.LineAlignment = PdfStringAlignment.Center;
                            //documentProcessor.LoadDocument(strDesginationPath);
                            documentProcessor.LoadDocument(ms);
                            using (SolidBrush brush = new SolidBrush(Color.FromArgb(33, Color.Black)))
                            {
                                using (Font font = new Font(fontName, fontSize))
                                {
                                    foreach (var page in documentProcessor.Document.Pages)
                                    {
                                        var watermarkSize = page.CropBox.Width * 0.75;
                                        using (PdfGraphics graphics = documentProcessor.CreateGraphics())
                                        {
                                            SizeF stringSize = graphics.MeasureString(WatermarkText, font);
                                            Single scale = Convert.ToSingle(watermarkSize / stringSize.Width);
                                            graphics.TranslateTransform(Convert.ToSingle(page.CropBox.Width * 0.5), Convert.ToSingle(page.CropBox.Height * 0.5));
                                            graphics.RotateTransform(-45);
                                            graphics.TranslateTransform(Convert.ToSingle(-stringSize.Width * scale * 0.5), Convert.ToSingle(-stringSize.Height * scale * 0.5));
                                            using (Font actualFont = new Font(fontName, fontSize * scale))
                                            {
                                                RectangleF rect = new RectangleF(0, 0, stringSize.Width * scale, stringSize.Height * scale);
                                                graphics.DrawString(WatermarkText, actualFont, brush, rect, stringFormat);
                                            }
                                            graphics.AddToPageForeground(page, 72, 72);
                                        }
                                    }
                                }
                            }

                            //documentProcessor.SaveDocument(strDesginationPath);
                            documentProcessor.SaveDocument(tempms);
                        }

                        NonPersistentObjectSpace Popupos = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(PDFPreview));
                        PDFPreview objToShow = (PDFPreview)Popupos.CreateObject(typeof(PDFPreview));
                        objToShow.ReportID = ReportID;
                        objToShow.PDFData = tempms.ToArray();
                        objToShow.ViewID = View.Id;
                        DetailView CreatedDetailView = Application.CreateDetailView(Popupos, objToShow);
                        CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                        ShowViewParameters showViewParameters = new ShowViewParameters(CreatedDetailView);
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        //showViewParameters.CreatedView.Caption = "PDFViewer";
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.AcceptAction.Active.SetItemValue("disable", false);
                        dc.CancelAction.Active.SetItemValue("disable", false);
                        dc.CloseOnCurrentObjectProcessing = false;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));

                        //string[] path = strDesginationPath.Split('\\');
                        //int arrcount = path.Length;
                        //int sc = arrcount - 2;
                        //string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                        ////strDesginationPath = string.Format("javascript:void(window.open('" + OriginalPath + "','_blank'));");
                        //strDesginationPath = string.Format(OriginalPath);
                        ////WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", strExportedPath));
                        //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", strDesginationPath)); 
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ReportNotFound"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
                else
                {
                    //strDesginationPath = string.Empty; 
                    Application.ShowViewStrategy.ShowMessage("Report not found", InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
                //return strDesginationPath;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Download(string ftpFile, string localFile)
        {

            //string ftp = "ftp://yourserver.com/";
            //string ftpFolder = "Uploads/";
            try
            {
                //Create FTP Request.
                System.Net.FtpWebRequest request = (System.Net.FtpWebRequest)WebRequest.Create(ftpFile);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                //Enter FTP Server credentials.
                request.Credentials = new NetworkCredential(strFTPUserName, strFTPPassword);
                request.UsePassive = true;
                request.UseBinary = true;
                request.EnableSsl = false;

                //Fetch the Response and read it into a MemoryStream object.
                System.Net.FtpWebResponse response = (System.Net.FtpWebResponse)request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (Stream fileStream = new FileStream(localFile, FileMode.CreateNew))
                    {
                        responseStream.CopyTo(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void ProcessAction(string parameter)
        {
            try
            {
                ShowReport(parameter);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //public void ReadXmlFile_FTPConc()
        //{
        //    try
        //    {
        //        string[] FTPconnectionstring = objRInfo.WebConfigFTPConn.Split(';');
        //        strFTPServerName = FTPconnectionstring[0].Split('=').GetValue(1).ToString();
        //        strFTPUserName = FTPconnectionstring[1].Split('=').GetValue(1).ToString();
        //        strFTPPassword = FTPconnectionstring[2].Split('=').GetValue(1).ToString();
        //        strFTPPath = FTPconnectionstring[3].Split('=').GetValue(1).ToString();
        //        FTPPort = Convert.ToInt32(FTPconnectionstring[4].ToString().Split('=').GetValue(1).ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        //public Rebex.Net.Ftp GetFTPConnection()
        //{
        //    try
        //    {
        //        Rebex.Net.Ftp FTP = new Rebex.Net.Ftp();
        //        FTP.TransferType = FtpTransferType.Binary;
        //        if ((!(strFTPServerName == null)
        //                    && ((strFTPServerName.Length > 0)
        //                    && (!(strFTPUserName == null)
        //                    && (strFTPUserName.Length > 0)))))
        //        {
        //            try
        //            {
        //                FTP.Timeout = 3000;
        //                FTP.Connect(strFTPServerName, FTPPort);
        //                FTP.Login(strFTPUserName, strFTPPassword);
        //                strFTPStatus = true;
        //            }
        //            catch (Exception ex)
        //            {
        //                strFTPStatus = false;
        //                return new Rebex.Net.Ftp();
        //            }

        //        }
        //        else
        //        {
        //            strFTPStatus = false;
        //            return new Rebex.Net.Ftp();
        //        }

        //        return FTP;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
}
