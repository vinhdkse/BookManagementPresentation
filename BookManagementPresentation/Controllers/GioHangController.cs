using BookManagementPresentation.Model;
using BookManagementPresentation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookManagementPresentation.Controllers
{
    public class GioHangController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public List<ItemGioHang> LayGioHang()
        {
            List<ItemGioHang> listGioHang = Session["GioHang"] as List<ItemGioHang>;
            //nếu chưa có giỏ hàng >> tạo mới
            if (listGioHang == null)
            {
                listGioHang = new List<ItemGioHang>();
                Session["GioHang"] = listGioHang;

            }
            //có rồi thì lấy nó ra
            return listGioHang;
        }
        //thêm giỏ hàng

        public ActionResult ThemGioHang(int _BookID, string urlPath)
        {
            //kiểm tra sách có tồn tại trong CSDL không
            Book book = db.Books.Single(s => s.ID == _BookID);
            //nếu sách k tồn tại >> đưa về trang 404
            if (book == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //nếu tồn tại >> lấy giỏ hàng ra
            List<ItemGioHang> listGioHang = LayGioHang();
            //1: sách đã có trong giỏ hàng
            ItemGioHang isExistedBook = listGioHang.SingleOrDefault(s => s.BookID == _BookID);
            if (isExistedBook != null)
            {
                //kiểm tra số lượng tồn <= số lượng trong giỏ hàng
                if (book.Quantity <= isExistedBook.Quantity)
                {
                    return View("ThongBao");
                }
                isExistedBook.Quantity++;
                isExistedBook.TotalPrice = isExistedBook.Quantity * isExistedBook.Price;
                return Redirect(urlPath);
            }
            //2: sách chưa có trong giỏ >> thêm sách mới với số lượng =1

            ItemGioHang itemGH = new ItemGioHang(_BookID);
            if (book.Quantity <= itemGH.Quantity)
            {
                return View("ThongBao");
            }
            listGioHang.Add(itemGH);
            return Redirect(urlPath);

        }

        public int TinhTongSoLuong()
        {
            //lấy giỏ hàng trên session
            List<ItemGioHang> gioHang = Session["GioHang"] as List<ItemGioHang>;
            if (gioHang == null)
            {
                return 0;
            }

            return gioHang.Sum(itemGH => itemGH.Quantity);
        }

        public decimal TinhTongTien()
        {
            //lấy giỏ hàng trên session
            List<ItemGioHang> gioHang = Session["GioHang"] as List<ItemGioHang>;
            if (gioHang == null)
            {
                return 0;
            }

            return gioHang.Sum(itemGH => itemGH.TotalPrice);
        }

        //dùng ajax dễ render
        public ActionResult GioHangPartial()
        {
            //mang tổng số lượng và tổng tiền qua trang partial
            if (TinhTongSoLuong() == 0)//số lượng =0 >> tông tiền bằng 0 >> kiểm tra 1 cái là đủ
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            ViewBag.ExistGH = Session["GioHang"];
            return PartialView();
        }

        // GET: GioHang
        public ActionResult XemGioHang(string EditGH)
        {
            //lấy giỏ hàng
            if (EditGH != null)
            {
                ViewBag.EditGH = EditGH;
            }
            return View();

        }
        public ActionResult GioHangCon()
        {
            List<ItemGioHang> GioHang = LayGioHang();
            //lấy tổng tiền
            ViewBag.TongTien = TinhTongTien();

            return PartialView(GioHang);
        }
        public ActionResult EditGioHang(int _BookID)
        {
            //kiểm tra giỏ hàng đã tồn tại trên sesion chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //kiểm tra sách có tồn tại trong CSDL hay k
            Book sach = db.Books.SingleOrDefault(s => s.ID == _BookID);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //lấy giỏ hàng từ session
            List<ItemGioHang> listGH = LayGioHang();
            //kiểm tra sản phẩm đã có trong giỏ hàng chưa
            ItemGioHang ExistedBook = listGH.SingleOrDefault(s => s.BookID == _BookID);
            if (ExistedBook == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //lấy giỏ hàng để thiết kế giao diện
            ViewBag.GioHang = listGH;
            //lấy tổng tiền
            ViewBag.TongTien = TinhTongTien();
            return View(ExistedBook);
        }

        //xử lý button Cập Nhật
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            //kiểm tra số lượng tồn
            string EditGH = "";
            Book bookCheck = db.Books.Single(s => s.ID == itemGH.BookID);
            if (bookCheck.Quantity < itemGH.Quantity)
            {
                //ViewBag.Error = "Sản phẩm này đã mua quá số lượng có thể cung cấp!!Vui lòng mua số lượng ít hơn";
                itemGH.Quantity = bookCheck.Quantity;
                EditGH = bookCheck.Quantity+"";
            }

            //cập nhật số lượng trong sesion giỏ hàng
            //B1:Lấy List<GioHang> tu session["GioHang"]
            List<ItemGioHang> listGH = LayGioHang();
            //b2: Lấy sản phẩm cần cập nhật từ trong list giỏ hàng ra
            ItemGioHang itemGHUpdate = listGH.Find(s => s.BookID == itemGH.BookID);
            //b3:cập nhật lại số lượng và thành tiền
            itemGHUpdate.Quantity = itemGH.Quantity;
            itemGHUpdate.TotalPrice = itemGHUpdate.Quantity * itemGHUpdate.Price;

            return RedirectToAction("XemGioHang", new { EditGH = EditGH });
        }

        //xóa giỏ hàng
        public ActionResult XoaItemGioHang(int _BookID)
        {
            //kiểm tra giỏ hàng đã tồn tại trên sesion chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //kiểm tra sách có tồn tại trong CSDL hay k
            Book sach = db.Books.SingleOrDefault(s => s.ID == _BookID);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //lấy giỏ hàng từ session
            List<ItemGioHang> listGH = LayGioHang();
            //kiểm tra sản phẩm đã có trong giỏ hàng chưa
            ItemGioHang ExistedBook = listGH.SingleOrDefault(s => s.BookID == _BookID);
            if (ExistedBook == null)
            {
                return RedirectToAction("Index", "Home");
            }
            listGH.Remove(ExistedBook);
            return RedirectToAction("XemGioHang");
        }
        [Authorize]
        public ActionResult XacNhanDatHang()
        {
            ViewBag.TongTien = TinhTongTien();
            ViewBag.Tinh = db.Provinces.ToList();
            ViewBag.ThanhToan = db.Payments.Where(s=>s.isDelete==false).ToList();
            List<ItemGioHang> GioHang = LayGioHang();
            ViewBag.GioHang = GioHang;
            ViewBag.PhiVanChuyen = 0;
            ViewBag.TongTien = TinhTongTien();
            ViewBag.TongCong = TinhTongTien() + 0;
            //lấy tổng tiền

            return View();
        }
        public decimal PhiVanChuyen(int province, int city, int dis)
        {
            var sale = Session["Vip"];
            if (sale != null)
            {
                return 0;
            }
            decimal tong = TinhTongTien();
            if (tong >= 2000000)
            {
                return 0;
            }
            if (province == 2)
            {
                return 100000;
            }
            if (province == 3)
            {
                return 150000;
            }
            if (city == 6)
            {
                if (dis == 52 || dis == 53 || dis == 55 || dis == 56 || dis == 72)
                {
                    return 60000;
                }
                else if (dis == 49 || dis == 62 || dis == 69)
                {
                    return 50000;
                }
                else
                {
                    return 30000;
                }
            }
            else if (city == 1 || city == 2 || city == 5)
            {
                return 70000;
            }
            else
            {
                return 80000;
            }
        }
        [Authorize]
        [HttpPost]
        public JsonResult LoadThanhPho(string id)

        {

            int? k = int.Parse(id);
            var arr = db.Cities.Where(s => s.ProvinceId == k).ToList();
            IList<City> list = new List<City>();
            foreach (var item in arr)
            {
                list.Add(new City { Id = item.Id, Name = item.Name });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [HttpPost]
        public JsonResult LoadHuyen(string id)
        {

            int? k = int.Parse(id);
            District d = db.Districts.Find(k);
            var arr = db.Districts.Where(s => s.CityId == k).ToList();
            IList<District> list = new List<District>();
            foreach (var item in arr)
            {
                list.Add(new District { Id = item.Id, Name = item.Name });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ChangeQuantity(string id, string quantity, string model)
        {
            int? k = int.Parse(id);
            Book bookCheck = db.Books.Single(s => s.ID == k);
            if (model.Equals("add"))
            {
                if (bookCheck.Quantity == int.Parse(quantity))
                {
                    return Json(quantity, JsonRequestBehavior.AllowGet);
                }

                //cập nhật số lượng trong sesion giỏ hàng
                //B1:Lấy List<GioHang> tu session["GioHang"]
                List<ItemGioHang> list = LayGioHang();
                //b2: Lấy sản phẩm cần cập nhật từ trong list giỏ hàng ra
                ItemGioHang item = list.Find(s => s.BookID == k);
                //b3:cập nhật lại số lượng và thành tiền
                item.Quantity = int.Parse(quantity) + 1;
                item.TotalPrice = item.Quantity * item.Price;
                string result = "<div class='table - responsive'>";
                result += "<table class='table table-bordered table-vcenter'><thead> <tr><th colspan='2'>Sách</th><th class='text-center'>Số Lượng"
+ "</th><th class='text-right'>Đơn Giá</th><th class='text-right'>Thành Tiền</th><th></th></tr></thead>";
                result += "<tbody>";
                for (int i = 0; i < list.Count; i++)
                {
                    var itemE = list.ElementAt(i);

                    result += "<tr>" +
                          "<td style='width: 200px;'>" +
                    "<img src='" + "/" + itemE.Image + "' " + "style='width:180px;'></td><td>" +
                     "<strong>" + itemE.BookName + "</strong><br><br>" +
                     "<strong class='text-success'>In stock</strong> - 24h Delivery" +
                      "</td><td class='text-center'>" +
                       "<a href='javascript:void(0)' class='btn btn-xs btn-danger' " +
                    "data-toggle='tooltip' title='Remove' id='" + (i + 1) + "' onclick='sub(this)'>" +
                    "<i class='fa fa-minus'></i></a>" +
                              "<strong> " + itemE.Quantity + " </strong>" +
                              "<input type='hidden' value='" + itemE.Quantity + "' id='quan " + (i + 1) + "'" + "name='" + itemE.BookID + "'/>" +
                               "<a href='javascript:void(0)' class='btn btn-xs btn-success' data-toggle='tooltip' title='Add' id='" + (i + 1) + "' onclick='add(this)'><i class='fa fa-plus' ></i></a> </td>"
                               + "<td class='text-right'>" + itemE.Price.ToString("#,##") + " VNĐ</td>" +
                             " <td class='text-right'><strong>" + itemE.TotalPrice.ToString("#,##") + " VNĐ</strong></td>" +
                               " <td><a href='/GioHang/EditGioHang?_BookID=" + itemE.BookID + "' class='btn btn-xs btn-success' data-toggle='tooltip' title='Sửa'>Sửa</a>" +
                                     " <a href='/GioHang/XoaItemGioHang?_BookID=" + itemE.BookID + "' class='btn btn-xs btn-danger' data-toggle='tooltip' title = 'Xóa' >Xóa</a ></td></tr>";
                }
                result += "<tr class='active'><td colspan='4' class='text-right text-uppercase h4'><strong>Tổng tiền</strong></td>" +
                                               " <td class='text-right text-success h4'><strong>" + list.Sum(itemGH => itemGH.TotalPrice).ToString("#,##") + " VNĐ</strong></td> <td></td></tr>";
                result += "</tbody>";
                result += "</table>";
                result += "</div>";
                result += "<div class='row'><div class='col-xs-7 col-md-3'><a href='/Home/Test' class='btn btn-block btn-primary'>Tiếp tục mua hàng</a></div>" +
"<form action='/GioHang/XacNhanDatHang' method='post'><div class='col-xs-5 col-md-3 col-md-offset-6'>" +
"<input type='submit' value='Đặt hàng' class='btn btn-block btn-danger' id='btnDatHang' /></div></form></div>";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            List<ItemGioHang> listGH = LayGioHang();

            if (int.Parse(quantity) == 1)
            {
                ItemGioHang ExistedBook = listGH.SingleOrDefault(s => s.BookID == k);
                listGH.Remove(ExistedBook);
                string result1 = "";
                if (listGH.Count == 0)
                {
                    result1 += "<div><h1 style='color:red'>Chưa có sản phẩm trong giỏ hàng ^^</h1></div>"
 + "<div class='col-xs-7 col-md-3'><a href='/Home/Test' class='btn btn-block btn-primary'>Tiếp tục mua hàng</a></div>";
                }
                else
                {
                    result1 += "<div class='table - responsive'>";
                    result1 += "<table class='table table-bordered table-vcenter'><thead> <tr><th colspan='2'>Sách</th><th class='text-center'>Số Lượng"
    + "</th><th class='text-right'>Đơn Giá</th><th class='text-right'>Thành Tiền</th><th></th></tr></thead>";
                    result1 += "<tbody>";
                    for (int i = 0; i < listGH.Count; i++)
                    {
                        var itemE = listGH.ElementAt(i);
                        result1 += "<tr>" +
                              "<td style='width: 200px;'>" +
                        "<img src='" + "/" + itemE.Image + "' " + "style='width:180px;'></td><td>" +
                         "<strong>" + itemE.BookName + "</strong><br><br>" +
                         "<strong class='text-success'>In stock</strong> - 24h Delivery" +
                          "</td><td class='text-center'>" +
                           "<a href='javascript:void(0)' class='btn btn-xs btn-danger' " +
                        "data-toggle='tooltip' title='Remove' id='" + (i + 1) + "' onclick='sub(this)'>" +
                        "<i class='fa fa-minus'></i></a>" +
                                  "<strong> " + itemE.Quantity + " </strong>" +
                                  "<input type='hidden' value='" + itemE.Quantity + "' id='quan " + (i + 1) + "'" + "name='" + itemE.BookID + "'/>" +
                                   "<a href='javascript:void(0)' class='btn btn-xs btn-success' data-toggle='tooltip' title='Add' id='" + (i + 1) + "' onclick='add(this)'><i class='fa fa-plus' ></i></a> </td>"
                                   + "<td class='text-right'>" + itemE.Price.ToString("#,##") + " VNĐ</td>" +
                                 " <td class='text-right'><strong>" + itemE.TotalPrice.ToString("#,##") + " VNĐ</strong></td>" +
                                   " <td><a href='/GioHang/EditGioHang?_BookID=" + itemE.BookID + "' class='btn btn-xs btn-success' data-toggle='tooltip' title='Sửa'>Sửa</a>" +
                                         " <a href='/GioHang/XoaItemGioHang?_BookID=" + itemE.BookID + "' class='btn btn-xs btn-danger' data-toggle='tooltip' title = 'Xóa' >Xóa</a ></td></tr>";
                    }
                    result1 += "<tr class='active'><td colspan='4' class='text-right text-uppercase h4'><strong>Tổng tiền</strong></td>" +
                                                   " <td class='text-right text-success h4'><strong>" + listGH.Sum(itemGH => itemGH.TotalPrice).ToString("#,##") + " VNĐ</strong></td> <td></td></tr>";
                    result1 += "</tbody>";
                    result1 += "</table>";
                    result1 += "</div>";
                    result1 += "<div class='row'><div class='col-xs-7 col-md-3'><a href='/Home/Test' class='btn btn-block btn-primary'>Tiếp tục mua hàng</a></div>" +
"<form action='/GioHang/XacNhanDatHang' method='post'><div class='col-xs-5 col-md-3 col-md-offset-6'>" +
"<input type='submit' value='Đặt hàng' class='btn btn-block btn-danger' id='btnDatHang' /></div></form></div>";
                }

                return Json(result1, JsonRequestBehavior.AllowGet);
            }

            //cập nhật số lượng trong sesion giỏ hàng
            //B1:Lấy List<GioHang> tu session["GioHang"]
            //b2: Lấy sản phẩm cần cập nhật từ trong list giỏ hàng ra
            ItemGioHang itemGHUpdate = listGH.Find(s => s.BookID == k);
            //b3:cập nhật lại số lượng và thành tiền
            itemGHUpdate.Quantity = int.Parse(quantity) - 1;
            itemGHUpdate.TotalPrice = itemGHUpdate.Quantity * itemGHUpdate.Price;

            string result2 = "<div class='table - responsive'>";
            result2 += "<table class='table table-bordered table-vcenter'><thead> <tr><th colspan='2'>Sách</th><th class='text-center'>Số Lượng"
+ "</th><th class='text-right'>Đơn Giá</th><th class='text-right'>Thành Tiền</th><th></th></tr></thead>";
            result2 += "<tbody>";
            for (int i = 0; i < listGH.Count; i++)
            {
                var itemE = listGH.ElementAt(i);
                result2 += "<tr>" +
                      "<td style='width: 200px;'>" +
                "<img src='" + "/" + itemE.Image + "' " + "style='width:180px;'></td><td>" +
                 "<strong>" + itemE.BookName + "</strong><br><br>" +
                 "<strong class='text-success'>In stock</strong> - 24h Delivery" +
                  "</td><td class='text-center'>" +
                   "<a href='javascript:void(0)' class='btn btn-xs btn-danger' " +
                "data-toggle='tooltip' title='Remove' id='" + (i + 1) + "' onclick='sub(this)'>" +
                "<i class='fa fa-minus'></i></a>" +
                          "<strong> " + itemE.Quantity + " </strong>" +
                          "<input type='hidden' value='" + itemE.Quantity + "' id='quan " + (i + 1) + "'" + "name='" + itemE.BookID + "'/>" +
                           "<a href='javascript:void(0)' class='btn btn-xs btn-success' data-toggle='tooltip' title='Add' id='" + (i + 1) + "' onclick='add(this)'><i class='fa fa-plus' ></i></a> </td>"
                           + "<td class='text-right'>" + itemE.Price.ToString("#,##") + " VNĐ</td>" +
                         " <td class='text-right'><strong>" + itemE.TotalPrice.ToString("#,##") + " VNĐ</strong></td>" +
                           " <td><a href='/GioHang/EditGioHang?_BookID=" + itemE.BookID + "' class='btn btn-xs btn-success' data-toggle='tooltip' title='Sửa'>Sửa</a>" +
                                     " <a href='/GioHang/XoaItemGioHang?_BookID=" + itemE.BookID + "' class='btn btn-xs btn-danger' data-toggle='tooltip' title = 'Xóa' >Xóa</a ></td></tr>";
            }
            result2 += "<tr class='active'><td colspan='4' class='text-right text-uppercase h4'><strong>Tổng tiền</strong></td>" +
                                           " <td class='text-right text-success h4'><strong>" + listGH.Sum(itemGH => itemGH.TotalPrice).ToString("#,##") + " VNĐ</strong></td> <td></td></tr>";
            result2 += "</tbody>";
            result2 += "</table>";
            result2 += "</div>";
            result2 += "<div class='row'><div class='col-xs-7 col-md-3'><a href='/Home/Test' class='btn btn-block btn-primary'>Tiếp tục mua hàng</a></div>" +
"<form action='/GioHang/XacNhanDatHang' method='post'><div class='col-xs-5 col-md-3 col-md-offset-6'>" +
"<input type='submit' value='Đặt hàng' class='btn btn-block btn-danger' id='btnDatHang' /></div></form></div>";
            return Json(result2, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [HttpPost]
        public JsonResult GetBill(string id)
        {
            int? k = int.Parse(id);
            District d = db.Districts.Find(k);
            decimal total = TinhTongTien();
            decimal phi = PhiVanChuyen(d.City.ProvinceId, d.CityId, d.Id);
            decimal tong = total + phi;
            List<ItemGioHang> list = LayGioHang();
            string result = "";
            foreach (var item in list)
            {
                result += "<tr><td style='width: 200px;'><img src='/" + item.Image + "' alt='' style='width: 180px;'></td><td>"
                             + "<strong> " + item.BookName + " </strong><br>"
                             + "<strong class='text-success'>In stock</strong> - 24h Delivery</td>"
                             + "<td class='text-center'><span class='label label-success'><strong>" + item.Quantity + "</strong></span>"
                             + "</td><td class='text-right'>" + item.Price.ToString("#,##") + " VNĐ</td><td class='text-right'><strong>" + item.TotalPrice.ToString("#,##") + " VNĐ</strong></td></tr>";
            }

            result += "<tr><td colspan = '4' class='text-right h4'><strong>Tổng tiền sản phẩm</strong></td><td class='text-right h4'><strong>" + total.ToString("#,##") + " VNĐ</strong></td></tr>";
            if (phi == 0)
            {
                result += "<tr><td colspan = '4' class='text-right h4'><strong>Phí vận chuyển</strong></td><td class='text-right h4'><strong>" + 0 + " VNĐ</strong></td></tr>";
            }
            else
            {
                result += "<tr><td colspan = '4' class='text-right h4'><strong>Phí vận chuyển</strong></td><td class='text-right h4'><strong>" + phi.ToString("#,##") + " VNĐ</strong></td></tr>";
            }
            string sale = "0%";
            MyUser my = db.MyUsers.SingleOrDefault(s => s.Email == User.Identity.Name);
            var re = db.DonDatHangs.Where(s => s.CusUsername == my.Id).Sum(s => s.Total);
            if (re != null)
            {
                double totalsale = double.Parse(re.Value.ToString());
                if (totalsale >= 20000000 && totalsale <= 39999999)
                {
                    sale = "5%";
                    tong = (tong*95)/100;
                }
                if(totalsale >= 40000000 && totalsale <= 59999999)
                {
                    sale = "10%";
                    tong = (tong * 90) / 100;
                }
                if (totalsale >= 60000000)
                {
                    sale = "15%";
                    tong = (tong * 85) / 100;
                }
            }
            result += "<tr><td colspan = '4' class='text-right h4'><strong>Khuyến mãi</strong></td><td class='text-right h4'><strong>" + sale + " VNĐ</strong></td></tr>";
            result += "<tr class='active'><td colspan = '4' class='text-right text-uppercase h4'><strong>Tổng đơn hàng</strong></td><td class='text-right text-success h4'><strong name='Tong'>" + tong.ToString("#,##") + " VNĐ</strong></td></tr>";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult History(string username)
        {
            MyUser my = db.MyUsers.SingleOrDefault(s => s.Email == username);
            var re = db.DonDatHangs.Where(s => s.CusUsername == my.Id).Sum(s => s.Total);
            double total = 0;
            if (re != null)
            {
                total = double.Parse(re.Value.ToString());
                if (total < 20000000)
                {
                    ViewBag.Vip = "Khách hàng vãng lai";
                }
                if (total >= 20000000 && total <= 39999999)
                {
                    ViewBag.Vip = "Khách hàng tương tác";
                }
                if (total >= 40000000 && total <= 59999999)
                {
                    ViewBag.Vip = "Khách hàng thân quen";
                }
                if (total >= 60000000)
                {
                    ViewBag.Vip = "Anh em trong gia đình";
                }
                ViewBag.Total = total;
            }
            var list = (IList<DonDatHang>)db.DonDatHangs.Where(s => s.MyUser.Email == username).OrderByDescending(s => s.OrderDate).ToList();
            return View(list);
        }
        [Authorize]
        public ActionResult HistoryPartial(string username)
        {
            var list = (IList<DonDatHang>)db.DonDatHangs.Where(s => s.MyUser.Email == username).OrderByDescending(s => s.DDH_ID).ToList();
            return PartialView(list);
        }
        [Authorize]
        public ActionResult LichSu(int id)
        {
            DonDatHang dt = db.DonDatHangs.Find(id);
            ViewBag.All = dt;
            var list = (IList<Cart>)db.Carts.Where(s => s.DDH_ID == id).ToList();
            var rek = db.DonDatHangs.Where(s => s.CusUsername == dt.CusUsername && s.isCanceled == false).Sum(s => s.Total);
            if (rek != null)
            {
                double total = double.Parse(rek.Value.ToString());
                if (total >= 20000000)
                {
                    Session["Vip"] = "Yes";
                }
                else
                {
                    Session["Vip"] = null;
                }
            }
            else
            {
                Session["Vip"] = null;
            }
            return View(list);
        }
        [Authorize]
        [HttpPost]
        public ActionResult HuyDatHang(int id, bool check)
        {
            DonDatHang dt = db.DonDatHangs.Find(id);
            dt.isCanceled = check;
            db.Entry(dt).State = EntityState.Modified;
            List<Cart> listC = db.Carts.Where(s => s.DDH_ID == id).ToList();
            foreach (var item in listC)
            {
                Book book = db.Books.Find(item.BookID);
                if (check == false)
                {
                    book.Quantity = book.Quantity - item.Quantity;
                }
                else
                {
                    book.Quantity = book.Quantity + item.Quantity;
                }
                db.Entry(book).State = EntityState.Modified;
            }

            db.SaveChanges();
            ViewBag.All = dt;
            return RedirectToAction("LichSu", new { id = id });
        }
        //chức năng đặt hàng
        [Authorize]
        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            //kiểm tra giỏ hàng đã tồn tại trên sesion chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }


            //thêm đơn đặt hàng
            DonDatHang ddh = new DonDatHang();
            ddh.OrderDate = DateTime.Now;
            ddh.isDelivered = false;
            ddh.isPaid = false;
            ddh.isCanceled = false;
            ddh.isDeleted = false;
            MyUser my = db.MyUsers.SingleOrDefault(s => s.Email == User.Identity.Name);
            ddh.CusUsername = my.Id;
            ddh.PaymentId = int.Parse(f["Payment"]);
            if (ddh.PaymentId == 2 || ddh.PaymentId == 5 || ddh.PaymentId == 6)
            {
                ddh.isPaid = true;
            }
            District d = db.Districts.Find(int.Parse(f["District"]));
            ddh.Address = f["Address"] + ", " + d.Name + ", " + d.City.Name + ", " + d.City.Province.Name;
            ddh.Phone = f["Phone"];
            
            decimal tong = TinhTongTien() + PhiVanChuyen(d.City.ProvinceId, d.CityId, d.Id);
            var re = db.DonDatHangs.Where(s => s.CusUsername == my.Id&&s.isCanceled==false).Sum(s => s.Total);
            if (re != null)
            {
                double totalsale = double.Parse(re.Value.ToString());
                if (totalsale >= 20000000 && totalsale <= 39999999)
                {
                    tong = (tong * 95) / 100;
                }
                if (totalsale >= 40000000 && totalsale <= 59999999)
                {
                    tong = (tong * 90) / 100;
                }
                if (totalsale >= 60000000)
                {
                    tong = (tong * 85) / 100;
                }
            }
            ddh.Total = double.Parse(tong+"");
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();
            //chi tiết đơn đặt hàng(cart)
            List<ItemGioHang> listGH = LayGioHang();
            foreach (var item in listGH)
            {
                Cart ctdh = new Cart();
                ctdh.DDH_ID = ddh.DDH_ID;
                ctdh.BookID = item.BookID;
                ctdh.BookName = item.BookName;
                ctdh.Quantity = item.Quantity;
                ctdh.Price = double.Parse(item.Price.ToString());
                db.Carts.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;


            var rek = db.DonDatHangs.Where(s => s.CusUsername == my.Id && s.isCanceled == false).Sum(s => s.Total);


            if (rek != null)
            {
                double total = double.Parse(rek.Value.ToString());
                if (total >= 20000000)
                {
                    Session["Vip"] = "Yes";
                }
                else
                {
                    Session["Vip"] = null;
                }
            }
            return RedirectToAction("LichSu",new { id= ddh.DDH_ID});
        }


        //them gio hang bang ajax

        public ActionResult ThemGioHangAjax(int _BookID, string urlPath)
        {
            //string AddToCartFail = "HetHang";
            //kiểm tra sách có tồn tại trong CSDL không
            Book book = db.Books.Find(_BookID);
            //nếu sách k tồn tại >> đưa về trang 404
            if (book == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //nếu tồn tại >> lấy giỏ hàng ra
            List<ItemGioHang> listGioHang = LayGioHang();
            //1: sách đã có trong giỏ hàng
            ItemGioHang isExistedBook = listGioHang.SingleOrDefault(s => s.BookID == _BookID);
            if (isExistedBook != null)
            {
                if (book.Quantity <= isExistedBook.Quantity)
                {
                    isExistedBook.Quantity = book.Quantity;
                }
                else
                {
                    isExistedBook.Quantity++;
                }
                isExistedBook.TotalPrice = isExistedBook.Quantity * isExistedBook.Price;
                if (urlPath.Contains("XemGioHang"))
                {
                    return RedirectToAction("GioHangCon");
                }
                return RedirectToAction("GioHangPartial");
            }
            //2: sách chưa có trong giỏ >> thêm sách mới với số lượng =1

            ItemGioHang itemGH = new ItemGioHang(_BookID);
            if (book.Quantity <= itemGH.Quantity)
            {
                itemGH.Quantity = book.Quantity;
            }
            listGioHang.Add(itemGH);
            //render 1 phan
            //ViewBag.TongSoLuong = TinhTongSoLuong();
            //ViewBag.TongTien = TinhTongTien();
            if (urlPath.Contains("XemGioHang"))
            {
                return RedirectToAction("GioHangCon");
            }
            return RedirectToAction("GioHangPartial");

        }
        public ActionResult BestSeller()
        {
            //lấy sách bán chạy
            string sql = "select * " +
                        "from Books " +
                        "where Books.ID in (select BookID " +
                                              "from(Select top(3) c.BookID, SUM(c.Quantity) as sumQuantity " +
                                                      "from Carts as c  , dbo.DonDatHangs as ddh, dbo.Books as b " +
                                                      "where c.DDH_ID = ddh.DDH_ID and b.ID=c.BookID and b.isDelete=0 and b.Quantity>0 and ddh.OrderDate <= GETDATE() and ddh.OrderDate >= (GETDATE() - 7) and ddh.isPaid = 'true' " +
                                                      "group by BookID " +
                                                      "order by(sumQuantity) DESC " +
                                                      ")a  " +
                                               ") ";



            List<Book> BookIDList = db.Database.SqlQuery<Book>(sql).ToList();

            return PartialView(BookIDList);

        }
    }
}