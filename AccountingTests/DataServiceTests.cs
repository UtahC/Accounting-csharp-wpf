using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Tests
{
    [TestClass()]
    public class DataServiceTests
    {
        static DataService dataService;

        [ClassInitialize()]
        public static void classInit(TestContext context)
        {
            dataService = new DataService();
        }
        [TestMethod()]
        public void insertTest()
        {
            Item item = new Item("2016/5/22", "cusname", "itemname", 5, 10, 15, "note");
            Assert.AreEqual(true, dataService.insert(item));
        }

        [TestMethod()]
        public void deleteTest()
        {
            Assert.AreEqual(true, dataService.delete(2));
        }

        [TestMethod()]
        public void updateTest()
        {
            Item item = new Item("2016/5/22", "cusname", "itemname", 5, 10, 15, "note");
            Assert.AreEqual(true, dataService.update(1, item));
        }
    }
}