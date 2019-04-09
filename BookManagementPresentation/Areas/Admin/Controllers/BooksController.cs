using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookManagementPresentation.Model;
using BookManagementPresentation.Models;

namespace BookManagementPresentation.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Books
        public ActionResult Index()
        {
            var books = db.Books.Include(b => b.Category).Include(c => c.Author).Where(s=>s.isDelete==false);
            return View(books.ToList());
        }

        // GET: Admin/Books/Details/5

        // GET: Admin/Books/Create
        public ActionResult Create()
        {
            ViewData["authorList"] = db.Authors.ToList();
            ViewData["categoryList"] = db.Categorys.ToList();
            return View();
        }

        // POST: Admin/Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost,ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Name,AuthorId,Publisher,PublishDate,Price,Quantity,Description,CategoryId,isDelete")] Book book, HttpPostedFileBase Image)
        {

            var allowedExtensions = new[] {
                ".Jpg", ".png", ".jpg", "jpeg",".JPG",".PNG",".JPEG"
                                              };
            var fileName = Path.GetFileName(Image.FileName);
            var ext = Path.GetExtension(Image.FileName);
            if (allowedExtensions.Contains(ext))
            {
                var path = Path.Combine(Server.MapPath("~/img"), fileName);
                book.Image = "img/" + fileName;
                book.isDelete = false;
                db.Books.Add(book);
                db.SaveChanges();
                Image.SaveAs(path);
            }
            return RedirectToAction("Index");
        }

        // GET: Admin/Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book != null)
            {
                Category category = db.Categorys.Find(book.CategoryId);
                Author author = db.Authors.Find(book.AuthorId);
                ViewData["authorList"] = db.Authors.ToList();
                ViewData["author"] = author;
                ViewData["category"] = category;
                ViewData["book"] = book;
                ViewData["categoryList"] = db.Categorys.ToList();
                return View();
            }
            return HttpNotFound();
        }

        // POST: Admin/Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,Name,AuthorId,Publisher,PublishDate,Price,Quantity,Description,CategoryId,isDelete")] Book book, HttpPostedFileBase Img, FormCollection f)
        {
            string check = f["checkChange"].ToString();
            if (check.Equals("NO"))
            {
                book.Image = f["noChange"].ToString();
            }
            else
            {
                var allowedExtensions = new[] {
                ".Jpg", ".png", ".jpg", "jpeg",".JPG",".PNG",".JPEG"
                                              };
                var fileName = Path.GetFileName(Img.FileName);
                var ext = Path.GetExtension(Img.FileName);
                if (allowedExtensions.Contains(ext))
                {
                    book.isDelete = false;
                    var path = Path.Combine(Server.MapPath("~/img"), fileName);
                    book.Image = "img/" + fileName;
                    Img.SaveAs(path);

                }
            }
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Admin/Books/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            book.isDelete = true;
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            //db.Books.Remove(book);
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
    }
}
