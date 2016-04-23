using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _30sep.Models;
using _30sep.Global;

namespace _30sep.Controllers
{
    public class HomeController : Controller
    {
        private _31sep db = new _31sep();
        public ActionResult Index()
        {
            var books = db.Book.Include(b => b.AspNetUsers).Include(b => b.Genre).Include(b => b.Author);
             random R=new random();
            var ra= R.RandomBook(books.ToList());
            var imgpath = ra.Imagepath;
            ViewData["BookImage"] = imgpath;
            ViewData["RandomBookId"] = ra.BookId;
            return View(books.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}