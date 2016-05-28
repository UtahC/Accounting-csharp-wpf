using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Accounting
{
    public enum Size
    {
        Small = 12,
        Medium = 16,
        Large = 20
    }

    public class PdfPrinter
    {
        private PdfDocument document;
        private List<PdfPage> pages = new List<PdfPage>();
        private List<XGraphics> gfxes = new List<XGraphics>();
        private int[] nowY;

        public PdfPrinter()
        {
            // Create a new PDF document
            document = new PdfDocument();
        }

        /// <summary>
        /// This method will remove all the context in every page.
        /// </summary>
        /// <param name="count"></param>
        public void setPageCount(int count)
        {
            nowY = new int[count];
            pages.Clear();
            for (int i = 0; i < count; i++)
            {
                PdfPage page = document.AddPage();
                pages.Add(page);
                XGraphics gfx = XGraphics.FromPdfPage(page);
                gfxes.Add(gfx);
                nowY[i] = 10;
            }
        }

        public void drawTextLine(int pageNumber, string text, Size size)
        {
            drawTextLine(pageNumber, text, size, false);
        }

        public void drawTextLine(int pageNumber, string text, Size size, bool isTitle)
        {
            PdfPage page = pages[pageNumber - 1];
            // Get an XGraphics object for drawing
            XGraphics gfx = gfxes[pageNumber - 1];
            //XTextFormatter tf = new XTextFormatter(gfx);
            // Create a font
            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
            XFont font = new XFont("標楷體", (int)size, XFontStyle.Bold, options);
            // Draw the text
            XStringFormat format = isTitle ? XStringFormats.TopCenter : XStringFormats.TopLeft;
            gfx.DrawString(text, font, XBrushes.Black,
            new XRect(10, nowY[pageNumber - 1], page.Width - 20, (int)size), format);
            nowY[pageNumber - 1] = nowY[pageNumber - 1] + (int)size + 2;
        }

        public void print(string fileName)
        {
            // Save the document...
            document.Save(fileName);
            // ...and start a viewer.
            Process.Start(fileName);
        }
    }
}
