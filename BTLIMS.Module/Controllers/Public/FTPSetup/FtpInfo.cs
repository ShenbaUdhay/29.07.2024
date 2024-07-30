using Modules.BusinessObjects.InfoClass;
using Rebex.Net;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace LDM.Module.Controllers.Public.FTPSetup
{
    public class FtpInfo
    {

        public string strFTPServerName = string.Empty;
        public string strFTPPath = string.Empty;
        public string strFTPUserName = string.Empty;
        public string strFTPPassword = string.Empty;
        public int FTPPort = 0;
        public bool strFTPStatus;
        string strExportedPath = string.Empty;
        DynamicReportDesignerConnection objRInfo = new DynamicReportDesignerConnection();
        Rebex.Net.Ftp _FTP;
        public FtpInfo()
        {
            //ReadXmlFile_FTPConc();
            // _FTP = GetFTPConnection();
        }
        public string GetDocument(string ReportID)
        {
            try
            {
                ReadXmlFile_FTPConc();
                Rebex.Net.Ftp _FTP = GetFTPConnection();
                if (_FTP.State == FtpState.Ready)
                {
                    strExportedPath = "LDMReports//" + ReportID + ".pdf";
                    string strLocalPath = string.Empty;
                    if (_FTP.FileExists(strExportedPath))
                    {
                        if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\EmailReport")) == false)
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\EmailReport"));
                        }
                        strLocalPath = HttpContext.Current.Server.MapPath(@"~\EmailReport\" + ReportID + ".pdf");
                        _FTP.GetFile(strExportedPath, strLocalPath);//((strExportedPath, ms);                        
                    }
                    return strLocalPath;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public void ReadXmlFile_FTPConc()
        {
            try
            {
                objRInfo.WebConfigFTPConn = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FTPConnectionStrings"))["FTPConnectionString"];

                if (objRInfo.WebConfigFTPConn != string.Empty)
                {
                    string[] FTPconnectionstring = objRInfo.WebConfigFTPConn.Split(';');
                    strFTPServerName = FTPconnectionstring[0].Split('=').GetValue(1).ToString();
                    strFTPUserName = FTPconnectionstring[1].Split('=').GetValue(1).ToString();
                    strFTPPassword = FTPconnectionstring[2].Split('=').GetValue(1).ToString();
                    strFTPPath = FTPconnectionstring[3].Split('=').GetValue(1).ToString();
                    FTPPort = Convert.ToInt32(FTPconnectionstring[4].ToString().Split('=').GetValue(1).ToString());
                }
            }
            catch (Exception ex)
            {

            }
        }
        public Rebex.Net.Ftp GetFTPConnection()
        {
            try
            {
                Rebex.Net.Ftp FTP = new Rebex.Net.Ftp();
                FTP.TransferType = FtpTransferType.Binary;
                if ((!(strFTPServerName == null)
                            && ((strFTPServerName.Length > 0)
                            && (!(strFTPUserName == null)
                            && (strFTPUserName.Length > 0)))))
                {
                    try
                    {
                        FTP.Timeout = 3000;
                        FTP.Connect(strFTPServerName, FTPPort);
                        FTP.Login(strFTPUserName, strFTPPassword);
                        strFTPStatus = true;
                    }
                    catch (Exception ex)
                    {
                        strFTPStatus = false;
                        return new Rebex.Net.Ftp();
                    }
                }
                else
                {
                    strFTPStatus = false;
                    return new Rebex.Net.Ftp();
                }
                return FTP;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
