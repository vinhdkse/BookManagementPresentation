using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BookManagementPresentation.Model;
using BookManagementPresentation.Models;

namespace BookManagementPresentation.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuanLyDonHangController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/QuanLiDonHang
        public ActionResult Index()
        {
            //danh sách các đơn hàng chưa giao+chưa thanh toán
            var listChuaGiao_ChuaThanhToan = db.DonDatHangs.Where(dh => dh.isDelivered == false && dh.isPaid == false).OrderBy(dh => dh.OrderDate);
            return View(listChuaGiao_ChuaThanhToan);
        }


        public ActionResult DaThanhToan_ChuaGiaoHang()
        {
            //danh sách các đơn hàng chưa giao+đã thanh toán
            var listChuaGiao_DaThanhToan = db.DonDatHangs.Where(dh => dh.isDelivered == false && dh.isPaid == true).OrderBy(dh => dh.OrderDate);
            return View(listChuaGiao_DaThanhToan);

        }
        public ActionResult DaThanhToan_DaGiaoHang()
        {
            //danh sách các đơn hàng chưa giao+đã thanh toán
            var listDaGiao_DaThanhToan = db.DonDatHangs.Where(dh => dh.isDelivered == true && dh.isPaid == true).OrderBy(dh => dh.OrderDate);
            return View(listDaGiao_DaThanhToan);
        }
        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            //kiểm tra id hợp lệ k
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var DonDatHang = db.DonDatHangs.SingleOrDefault(ddh => ddh.DDH_ID == id);
            //kiem tra don dat hang co ton tai k
            if (DonDatHang == null)
            {
                return HttpNotFound();
            }
            //lay chi tiet don hang
            var chiTietDonHangList = db.Carts.Where(ct => ct.DDH_ID == id);
            ViewBag.ListChiTietDonHang = chiTietDonHangList;
            return View(DonDatHang);
        }

        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang ddh)
        {
            //Truy vấn lấy ra dữ liệu của đơn hàng đó 
            DonDatHang ddhUpdate = db.DonDatHangs.Single(dh => dh.DDH_ID == ddh.DDH_ID);
            ddhUpdate.isPaid = ddh.isPaid;
            ddhUpdate.isDelivered = ddh.isDelivered;
            ddhUpdate.DeliverDate = ddh.DeliverDate;
            db.SaveChanges();

            //Lấy danh sách chi tiết đơn hàng để hiển thị cho người dùng thấy
            var chiTietDonHangList = db.Carts.Where(ct => ct.DDH_ID == ddh.DDH_ID);
            ViewBag.ListChiTietDonHang = chiTietDonHangList;
            //Gửi khách hàng 1 mail để xác nhận việc thanh toán/ giao hàng 
            string paid = "";
            if (bool.Parse(ddhUpdate.isPaid.ToString()))
            {
                paid = "Đã thanh toán";
            }
            else
            {
                paid = "Chưa thanh toán";
            }
            string deliver = "";
            if (bool.Parse(ddhUpdate.isDelivered.ToString()))
            {
                deliver = "Đã giao";
            }
            else
            {
                deliver = "Chưa giao";
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("<table id='table' border='1'><thead><td colspan='5'><table style='width: 100 %; '><tr>");
            builder.Append("<td>Mã đơn hàng: <b style='color: red; '>" + ddhUpdate.DDH_ID + "</b></td>");
            builder.Append("<td>Mã khách hàng: <b style='color: red; '>" + ddhUpdate.MyUser.Id + "</b></td>");
            builder.Append("<td>Tên khách hàng: <b style='color: red; '>" + ddhUpdate.MyUser.Name + "</b></td>");
            builder.Append("<td>Ngày đặt: <b style='color: red; '>" + ddhUpdate.OrderDate.ToString() + "</b></td>");
            builder.Append("<td>Đã thanh toán: <b style='color: red; '>" + paid + "</b></td>");
            builder.Append("<td>Đã giao: <b style='color: red; '>" + deliver + "</b></td>");
            builder.Append("</tr></table></td></tr></thead><thead><tr>");
            builder.Append("<th class='text-center'>Sản Phẩm</th>");
            builder.Append("<th class='text-center'>Số Lượng Đặt</th>");
            builder.Append("<th class='text-center'>Đơn Giá</th>");
            builder.Append("</tr></thead><tbody>");



            int TongSoLuong = 0;
            
            foreach (var item in chiTietDonHangList)
            {

                TongSoLuong += item.Quantity;

                builder.Append("<tr><td class='text-center'>" + item.BookName + "</td>");
                builder.Append("<td class='text-center'>" + item.Quantity + "</td>");
                builder.Append("<td class='text-center'>" + item.Price + "</td></tr>");

            }
            builder.Append("<tr><td align='right' colspan='5'>Tổng số lượng: " + TongSoLuong + " cuốn sách</td></tr>");
            builder.Append("<tr><td align='right' colspan='5'>Tổng tiền: " + double.Parse(ddhUpdate.Total.ToString()).ToString("#,##") + " VNĐ</td></tr>");
            builder.Append("</tbody></table>");
            //string content = System.IO.File.ReadAllText(Server.MapPath(@"~\Areas\Admin\Views\QuanLyDonHang\EmailContent.html"));
            //content = content.Replace("{{MaDonHang}}", ddhUpdate.DDH_ID.ToString());
            //content = content.Replace("{{MaKhachHang}}", ddhUpdate.MyUser.Id);
            //content = content.Replace("{{TenKhachHang}}", ddhUpdate.MyUser.Name);
            //content = content.Replace("{{NgayDat}}", ddhUpdate.OrderDate.ToString());
            //content = content.Replace("{{DaThanhToan}}", ddhUpdate.isPaid.ToString());
            //content = content.Replace("{{DaGiao}}", ddhUpdate.isDelivered.ToString());

            //int TongSoLuong = 0;
            //double TongTien = 0;
            //foreach (var item in chiTietDonHangList)
            //{

            //        TongSoLuong += item.Quantity;

            //    TongTien += (item.Quantity * item.Price);
            //    content = content.Replace("{{TenSach}}", item.BookName);
            //    content = content.Replace("{{HinhAnh}}", "~/"+item.Book.Image);
            //    content = content.Replace("{{SoLuong}}", item.Quantity.ToString());
            //    content = content.Replace("{{DonGia}}", item.Price.ToString("#,##"));

            //}
            //content = content.Replace("{{TongSoLuong}}", TongSoLuong.ToString());
            //content = content.Replace("{{TongTien}}", TongTien.ToString("#,##"));




            MyUser my = db.MyUsers.SingleOrDefault(s=>s.Id == ddh.CusUsername);
            GuiEmail("Xác nhận đơn hàng của nhà sách VAT", my.Email, ConfigurationManager.AppSettings["Email"].ToString(), ConfigurationManager.AppSettings["Password"].ToString(), builder.ToString());
            return View(ddhUpdate);
        }

        public void GuiEmail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
        {
            // goi email
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail); // Địa chỉ nhận
            mail.From = new MailAddress(ToEmail); // Địa chỉ gửi
            mail.Subject = Title;  // tiêu đề gửi
            mail.Body = Content;                 // Nội dung
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; // host gửi của Gmail
            smtp.Port = 587;               //port của Gmail
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            (FromEmail, PassWord);//Tài khoản password người gửi
            smtp.EnableSsl = true;   //kích hoạt giao tiếp an toàn SSL
            smtp.Send(mail);   //Gửi mail đi
        }
    }
}
