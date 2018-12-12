using System;
using System.Linq;
using QTStore.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using QTStore.Models;
using System.Collections.Generic;
using Moq;
using System.Web;
using System.Web.Routing;
using System.Transactions;

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

        [TestMethod]
        public void CreateGetTest()
        {
            var controller = new BangSanPhamController();
            var result = controller.Create() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData["Loai_id"], typeof(SelectList));
        }

        [TestMethod]
        public void DetailTest()
        {
            string id = "1";
            var controller = new BangSanPhamController();

            var context = new Mock<HttpContextBase>();
            context.Setup(c => c.Server.MapPath("~/App_Data/" + id)).Returns("~/App_Data/" + id);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            var result = controller.Details(id) as FilePathResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("images", result.ContentType);
            Assert.AreEqual("~/App_Data/" + id, result.FileName);
        }

        [TestMethod]
        public void CreatePostTest()
        {
            var controller = new BangSanPhamController();
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var files = new Mock<HttpFileCollectionBase>();
            var file = new Mock<HttpPostedFileBase>();
            context.Setup(c => c.Request).Returns(request.Object);
            request.Setup(r => r.Files).Returns(files.Object);
            files.Setup(f => f["HinhAnh"]).Returns(file.Object);
            file.Setup(f => f.ContentLength).Returns(1);
            context.Setup(c => c.Server.MapPath("~/App_Data")).Returns("~/App_Data");
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            var db = new CS4PEEntities();
            var model = new BangSanPham();
            model.Loai_id = db.LoaiSanPhams.First().id;
            model.TenSP = "TenSP";
            model.MaSP = "MaSP";
            model.GiaGoc = 123;
            model.GiaBan = 456;
            model.GiaGop = 789;
            model.SoLuongTon = 10;

            using (var scope = new TransactionScope())
            {
                var result0 = controller.Create(model) as RedirectToRouteResult;
                Assert.IsNotNull(result0);
                file.Verify(f => f.SaveAs(It.Is<string>(s => s.StartsWith("~/App_Data/"))));
                Assert.AreEqual("Index", result0.RouteValues["action"]);

                file.Setup(f => f.ContentLength).Returns(0);
                var result1 = controller.Create(model) as ViewResult;
                Assert.IsNotNull(result1);
                Assert.IsInstanceOfType(result1.ViewData["Loai_id"], typeof(SelectList));
            }
        }
    }
}