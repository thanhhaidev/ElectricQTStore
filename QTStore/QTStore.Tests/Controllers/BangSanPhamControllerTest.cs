using System;
using System.Linq;
using QTStore.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using QTStore.Models;
using System.Collections.Generic;

namespace QTStore.Tests.Controllers
{
    [TestClass]
    public class BangSanPhamControllerTest
    {
        [TestMethod]
        public void IndexTest()
        {
            var controller = new BangSanPhamController();
            var result = controller.Index() as ViewResult;

            var db = new CS4PEEntities();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(List<BangSanPham>));
            Assert.AreEqual(db.BangSanPhams.Count(), ((List<BangSanPham>)result.Model).Count());

        }
    }
}
