using System;

namespace LDM.Module.BusinessObjects
{
    public class GetTextAndRectangle
    {
        public iTextSharp.text.Rectangle Rect;
        public string Text;
        public int pageno;

        public GetTextAndRectangle(iTextSharp.text.Rectangle rect, String text, int pno)
        {
            this.Rect = rect;
            this.Text = text;
            this.pageno = pno;
        }
    }
}
