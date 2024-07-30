using iTextSharp.text.pdf.parser;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.BusinessObjects
{
    public class MyLocationTextExtractionStrategy : LocationTextExtractionStrategy
    {
        //Hold each coordinate
        public List<GetTextAndRectangle> myPoints = new List<GetTextAndRectangle>();

        //The string that we're searching for
        public string TextToSearchFor { get; set; }

        public int pageno { get; set; }

        //How to compare strings
        public System.Globalization.CompareOptions CompareOptions { get; set; }

        //Automatically called for each chunk of text in the PDF
        public override void RenderText(TextRenderInfo renderInfo)
        {
            base.RenderText(renderInfo);
            //System.Globalization.CompareOptions.
            //See if the current chunk contains the text
            var startPosition = System.Globalization.CultureInfo.CurrentCulture.CompareInfo.IndexOf(renderInfo.GetText(), this.TextToSearchFor, this.CompareOptions);

            //If not found bail
            if (startPosition < 0)
            {
                return;
            }

            //Grab the individual characters
            var chars = renderInfo.GetCharacterRenderInfos().Skip(startPosition).Take(this.TextToSearchFor.Length).ToList();

            //Grab the first and last character
            var firstChar = chars.First();
            var lastChar = chars.Last();

            //Get the bounding box for the chunk of text
            var bottomLeft = firstChar.GetDescentLine().GetStartPoint();
            var topRight = lastChar.GetAscentLine().GetEndPoint();

            //Create a rectangle from it
            var rect = new iTextSharp.text.Rectangle(
                                                    bottomLeft[Vector.I1],
                                                    bottomLeft[Vector.I2],
                                                    topRight[Vector.I1],
                                                    topRight[Vector.I2]
                                                    );

            //Add this to our main collection
            this.myPoints.Add(new GetTextAndRectangle(rect, this.TextToSearchFor, this.pageno));
        }
    }
}
