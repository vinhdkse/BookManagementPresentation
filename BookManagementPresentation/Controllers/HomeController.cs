using BookManagementPresentation.Model;
using BookManagementPresentation.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BookManagementPresentation.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            //lấy sách bán chạy
            string sql = "select * " +
                        "from Books " +
                        "where Books.ID in (select BookID " +
                                              "from(Select top(5) c.BookID, SUM(c.Quantity) as sumQuantity " +
                                                      "from Carts as c  , dbo.DonDatHangs as ddh, dbo.Books as b " +
                                                      "where c.DDH_ID = ddh.DDH_ID and b.ID=c.BookID and b.isDelete=0 and b.Quantity>0 and ddh.OrderDate <= GETDATE() and ddh.OrderDate >= (GETDATE() - 7) and ddh.isPaid = 'true' " +
                                                      "group by BookID " +
                                                      "order by(sumQuantity) DESC " +
                                                      ")a  " +
                                               ") ";



            List<Book> BookIDList = db.Database.SqlQuery<Book>(sql).ToList();

            return View(BookIDList);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult SendMail(Message model)
        {
            model.DateSend = DateTime.Now;
            db.Messages.Add(model);
            db.SaveChanges();
            ViewBag.Success = "Thư của bạn đã được gửi thành công";
            return View("Contact");
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Test(int? Page,string selectedValue)
        {
            if(Page == null)
            {
                return View();

            }
            return RedirectToAction("_TestPartialView",new { Page=Page,selectedValue=selectedValue});
        }
        public ActionResult _TestPartialView(int? Page, string selectedValue)
        {
            ViewBag.sort = selectedValue;
            var ListSach = db.Books.Where(s=>s.Quantity>0 && s.isDelete == false && s.Quantity > 0).ToList().OrderBy(s => s.PublishDate);
            if(selectedValue == "2")
            {
                ListSach = db.Books.Where(s => s.isDelete == false && s.Quantity > 0).ToList().OrderBy(s => s.Name);
            }
            if (selectedValue == "3")
            {
                ListSach = db.Books.Where(s => s.isDelete == false && s.Quantity > 0).ToList().OrderByDescending(s => s.Name);
            }
            if (selectedValue == "4")
            {
                ListSach = db.Books.Where(s => s.isDelete == false && s.Quantity > 0).ToList().OrderBy(s => s.Price);
            }
            if (selectedValue == "5")
            {
                ListSach = db.Books.Where(s => s.isDelete == false && s.Quantity > 0).ToList().OrderByDescending(s => s.Price);
            }

            //phân trang bằng thư viện pagedlist
            int pageSize = 8;
            int pageNumber = (Page ?? 1);//phép gán: nếu Page k chưa giá trị thì mặc định bằng 1
            return PartialView(ListSach.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult LoadDanhMucSach(int? CageID)
        {
            var SachList = db.Books.Where(s=>s.isDelete==false);
            return PartialView(SachList);
        }
        public ActionResult LoadSachTheoDanhMuc(int? CageID, int? Page, string selectedValue)//dâu hỏi vì int 32 hoặc int 64; page: trang hiên tại
        {
            if(Page == null)
            {
                return View();
            }
            if (CageID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//lỗi 400-k tìm thấy
            }
            return RedirectToAction("DanhMucPartial", new { CageID = CageID, Page = Page, selectedValue = selectedValue });

        }
        public ActionResult DanhMucPartial(int? CageID, int? Page, string selectedValue)
        {
            ViewBag.sort = selectedValue;
            var ListSach = db.Books.Where(s => s.CategoryId == CageID && s.isDelete == false &&s.Quantity>0).ToList().OrderBy(s => s.PublishDate);
            if (selectedValue == "2")
            {
                ListSach = db.Books.Where(s => s.CategoryId == CageID  && s.isDelete == false && s.Quantity > 0).ToList().OrderBy(s => s.Name);
            }
            if (selectedValue == "3")
            {
                ListSach = db.Books.Where(s => s.CategoryId == CageID  && s.isDelete == false && s.Quantity > 0).ToList().OrderByDescending(s => s.Name);
            }
            if (selectedValue == "4")
            {
                ListSach = db.Books.Where(s => s.CategoryId == CageID  && s.isDelete == false && s.Quantity > 0).ToList().OrderBy(s => s.Price);
            }
            if (selectedValue == "5")
            {
                ListSach = db.Books.Where(s => s.CategoryId == CageID  && s.isDelete == false && s.Quantity > 0).ToList().OrderByDescending(s => s.Price);
            }
            //phân trang bằng thư viện pagedlist
            int pageSize = 8;
            int pageNumber = (Page ?? 1);//phép gán: nếu Page k chưa giá trị thì mặc định bằng 1
            ViewBag.CageID = CageID;
            return PartialView(ListSach.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult LoadSachTheoDanhMucVaTacGia(int? CageID, int? AuthorID, int? Page, string selectedValue)//dâu hỏi vì int 32 hoặc int 64
        {
            if (Page == null)
            {
                return View();
            }
            if (CageID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//lỗi 400-k tìm thấy
            }
            return RedirectToAction("TacGiaPartial", new { CageID = CageID, AuthorID= AuthorID, Page = Page, selectedValue = selectedValue });

        }
        public ActionResult TacGiaPartial(int? CageID, int? AuthorID, int? Page, string selectedValue)
        {
            ViewBag.sort = selectedValue;
            var ListSach = db.Books.Where(s => s.CategoryId == CageID && s.AuthorId == AuthorID && s.isDelete == false&&s.Quantity>0).ToList().OrderBy(s => s.PublishDate);
            if (selectedValue == "2")
            {
                ListSach = db.Books.Where(s => s.CategoryId == CageID && s.AuthorId == AuthorID && s.isDelete == false && s.Quantity > 0).ToList().OrderBy(s => s.Name);
            }
            if (selectedValue == "3")
            {
                ListSach = db.Books.Where(s => s.CategoryId == CageID && s.AuthorId == AuthorID && s.isDelete == false && s.Quantity > 0).ToList().OrderByDescending(s => s.Name);
            }
            if (selectedValue == "4")
            {
                ListSach = db.Books.Where(s => s.CategoryId == CageID && s.AuthorId == AuthorID && s.isDelete == false && s.Quantity > 0).ToList().OrderBy(s => s.Price);
            }
            if (selectedValue == "5")
            {
                ListSach = db.Books.Where(s => s.CategoryId == CageID && s.AuthorId == AuthorID && s.isDelete == false && s.Quantity > 0).ToList().OrderByDescending(s => s.Price);
            }
            Author ThongTinTacGia = db.Authors.SingleOrDefault(tg => tg.ID == AuthorID);
            ViewBag.ThongTinTacGia = ThongTinTacGia;
            ViewBag.CageID = CageID;
            int pageSize = 8;
            int pageNumber = (Page ?? 1);

            return PartialView(ListSach.ToPagedList(pageNumber, pageSize));
        }
        //[HttpPost]
        //public ActionResult CommentAction(FormCollection f)
        //{
        //    Comment m = new Comment();
        //    m.BookId = int.Parse(f["BookId"]);
        //    m.UserId = f["UserId"];
        //    m.ContentDate = DateTime.Now;
        //    m.Content = f["Content"];
        //    if (m.Content.Equals(""))
        //    {
        //        return RedirectToAction("Comment", new { m.BookId });
        //    }
        //    MyUser my = db.MyUsers.Find(m.UserId);
        //    m.Name = my.Name;
        //    db.Comments.Add(m);
        //    db.SaveChanges();
        //    return RedirectToAction("Comment",new { m.BookId});
        //}
        public ActionResult Comment(int? BookID)
        {
            var list = db.Comments.Where(s => s.BookId == BookID).ToList();
            ViewData["ID"] = BookID;
            ViewData["ListComment"] = list;
            return PartialView();
        }
        public ActionResult XemChiTietSach(int? BookID, int? AuthorID, string AuthorName)
        {
            if (BookID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//lỗi 400-k tìm thấy
            }
            var Sach = db.Books.SingleOrDefault(s => s.ID == BookID && s.isDelete == false && s.Quantity > 0);
            if (Sach == null)
            {
                return HttpNotFound();
            }
            if (AuthorName != null)
            {
                ViewBag.AuthorName = AuthorName;
            }
            else
            {
                Author TacGia = db.Authors.SingleOrDefault(tg => tg.ID == Sach.AuthorId);
                ViewBag.AuthorName = TacGia.Name;
            }
            var ListComment = db.Comments.Where(s => s.BookId == BookID).ToList();
            ViewData["ListComment"] = ListComment;
            ////mang tên tác giả qau trang chi tiết
            //var TenTacGia = db.Authors.SingleOrDefault(tg=>tg.AuthorID==AuthorID);
            //ViewBag.TenTacGia = TenTacGia;
            return View(Sach);
        }
        public JsonResult GetSearchList(string search)
        {


            var allsearch = (from B in db.Books
                             where B.Name.Contains(search)
                             select new { B.Name });

            return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult FindBookByLikeName(int? Page, string selectedValue, string bookName)
        {
            if(Page==null)
            {
                ViewBag.TuKhoa = bookName;
                ViewBag.Sort = selectedValue;
                return View();
            }
            return RedirectToAction("FullNamePartial", new { Page = Page, selectedValue = selectedValue,bookName=bookName });
        }
        public ActionResult FullNamePartial(int? Page, string selectedValue, string bookName)
        {
            var ListSearchBook = db.Books.Where(b => b.Name.Contains(bookName) && b.isDelete == false && b.Quantity > 0).ToList().OrderBy(s => s.PublishDate);
            ViewBag.sort = selectedValue;
            if (selectedValue == "2")
            {
                ListSearchBook = db.Books.Where(s => s.Name.Contains(bookName) &&s.Quantity > 0 && s.isDelete == false).ToList().OrderBy(s => s.Name);
            }
            if (selectedValue == "3")
            {
                ListSearchBook = db.Books.Where(s => s.Name.Contains(bookName) && s.isDelete == false && s.Quantity > 0).ToList().OrderByDescending(s => s.Name);
            }
            if (selectedValue == "4")
            {
                ListSearchBook = db.Books.Where(s => s.Name.Contains(bookName) && s.isDelete == false && s.Quantity > 0).ToList().OrderBy(s => s.Price);
            }
            if (selectedValue == "5")
            {
                ListSearchBook = db.Books.Where(s => s.Name.Contains(bookName) && s.isDelete == false && s.Quantity > 0).ToList().OrderByDescending(s => s.Price);
            }
            ViewBag.TuKhoa = bookName;
            //phân trang bằng thư viện pagedlist
            int pageSize = 8;
            int pageNumber = (Page ?? 1);//phép gán: nếu Page k chưa giá trị thì mặc định bằng 1
            return PartialView(ListSearchBook.ToPagedList(pageNumber, pageSize));
        }
    }
}