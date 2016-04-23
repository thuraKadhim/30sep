using _30sep.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace _30sep.Controllers
{
    public class BooksController : Controller
    {
        private _31sep db = new _31sep();

        // GET: Books
        public ActionResult Index()
        {
            var book = db.Book.Include(b => b.AspNetUsers).Include(b => b.Author).Include(b => b.Genre);
            return View(book.ToList());
        }
        [Authorize]
        public ActionResult MyBook()
        {
            var id = User.Identity.GetUserId();
            var books = db.Book.Where(b => b.AspNetUsers.Id == id).ToList();

            return View(books);
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ReviewId = new SelectList(db.Review, "Id", "Name");
            if (id == null)
            {
                id = 4;
            }

            Book book = db.Book.Find(id);
            ViewData["Average"] = Global.AverageRating.averageRating(book);
           
            if (book == null)
            {
                return HttpNotFound();
            }
           
            ViewData["Reviews"]=db.Review.Where(x => x.BookId==id ).ToList();
             
           



            
            return View(book);
        }

        // GET: Books/Create
        [Authorize]
        public ActionResult Create()
        {

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.AuthorID = new SelectList(db.Author, "AuthorId", "FirstName");
            ViewBag.GenreId = new SelectList(db.Genre, "GenreId", "GenreName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "BookId,Title,Description,ISBN,AuthorID,GenreId,UserId,Imagepath")] Book book, HttpPostedFileBase file)
        {
            ModelState.Remove("BookId");
            ModelState.Remove("UserId");
            book.UserId=User.Identity.GetUserId();
           

                if (ModelState.IsValid)
                {
                    db.Book.Add(book);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            
         

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", book.UserId);
            ViewBag.AuthorID = new SelectList(db.Author, "AuthorId", "FirstName", book.AuthorID);
            ViewBag.GenreId = new SelectList(db.Genre, "GenreId", "GenreName", book.GenreId);
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", book.UserId);
            ViewBag.AuthorID = new SelectList(db.Author, "AuthorId", "FirstName", book.AuthorID);
            ViewBag.GenreId = new SelectList(db.Genre, "GenreId", "GenreName", book.GenreId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "BookId,Title,Description,ISBN,AuthorID,GenreId,UserId,Imagepath")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", book.UserId);
            ViewBag.AuthorID = new SelectList(db.Author, "AuthorId", "FirstName", book.AuthorID);
            ViewBag.GenreId = new SelectList(db.Genre, "GenreId", "GenreName", book.GenreId);
            return View(book);
        }

        // GET: Books/Delete/5
   [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Book.Find(id);
            db.Book.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateReview(FormCollection reviewform)
        {
            if (reviewform["Title"].IsNullOrWhiteSpace() && reviewform["Review"].IsNullOrWhiteSpace() &&
                reviewform["star"].IsNullOrWhiteSpace())
            {
                return RedirectToAction("Details", routeValues: new { id = reviewform["Id"] });
            }
            if (reviewform["Review"].IsNullOrWhiteSpace() &&
                reviewform["star"].IsNullOrWhiteSpace())
            {
                return RedirectToAction("Details", routeValues: new { id = reviewform["Id"] });
            }
            var bookid = int.Parse(reviewform["BookId"]);
            var userid = User.Identity.GetUserId();
            var rating = !string.IsNullOrEmpty(reviewform["star"]) ? int.Parse(reviewform["star"]) : 0;

            var title = reviewform["Title"];
            var description = reviewform["Review"];
            var created = DateTime.Now;

            var review = new Review
            {
                BookId = bookid,
                UserId = userid,
                Title = title,
                Description = description,
                Rating = rating
            };

            db.Review.Add(review);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = review.BookId });
        }

       
    
    }
}
