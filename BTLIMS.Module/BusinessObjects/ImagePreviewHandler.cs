using System;
using System.Web;

namespace LDM.Module.BusinessObjects
{
    public class ImagePreviewHandler : IHttpHandler
    {
        public byte[] Bytes { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string FilePath { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            try
            {

                context.Response.Clear();
                context.Response.Buffer = true;
                context.Response.Charset = "";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.ContentType = ContentType;
                context.Response.BinaryWrite(Bytes);
                context.Response.Flush();
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
