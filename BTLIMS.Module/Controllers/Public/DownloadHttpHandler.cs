using System;
using System.Web;

namespace LDM.Module.Controllers.Public
{
    public class DownloadHttpHandler : IHttpHandler
    {
        ExceptionTrackingViewController exceptionTrackingViewController = new ExceptionTrackingViewController();
        public byte[] Bytes { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string FilePath { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //context.Response.Headers.Clear();
                context.Response.ClearHeaders();
                context.Response.Clear();
                context.Response.ClearContent();
                context.Response.Buffer = true;
                context.Response.BufferOutput = true;
                context.Response.Charset = "";
                //context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
                context.Response.AddHeader("Last-Modified", DateTime.Now.ToLongDateString());
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.ContentType = ContentType;
                //context.Response.BinaryWrite(Bytes);
                //HttpContext.Current.Response.TransmitFile("~/ReportDownloads/" + FileName);
                HttpContext.Current.Response.TransmitFile(FilePath);
                context.Response.Flush();
                //context.Response.End();
                context.Response.SuppressContent = true;
                context.ApplicationInstance.CompleteRequest();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
