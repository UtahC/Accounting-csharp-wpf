using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accounting.Tests
{
    [TestClass()]
    public class PdfPrinterTests
    {
        static PdfPrinter printer;
        [ClassInitialize]
        public static void initial(TestContext text)
        {
            printer = new PdfPrinter();
        }
        [TestMethod()]
        public void drawTest()
        {
            printer.setPageCount(1);
            printer.drawTextLine(1, "hello world\n", Size.Small); 
            printer.drawTextLine(1, "and newLine 這裡.\n", Size.Small);
            printer.drawTextLine(1, "and here.", Size.Large);
            printer.print("test.pdf");
            Assert.AreEqual(true, true);
        }
    }
}