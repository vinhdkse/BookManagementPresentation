﻿using BookManagementPresentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookManagementPresentation.Models
{
    public class ItemGioHang
    {
        //đây là view model, tương tác dữ liệu, show ra view dùng class này.
        //k liên quan tới entity fw
        public int BookID { get; set; }
        public string BookName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string Image { get; set; }

        public ItemGioHang(int _BookID, int soLuong)//số lượng để nhập tùy ý, k cần phải +1 mỗi lần nhấn ADD
        {
            //using để đỡ tốn vùng nhớ
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Book sach = db.Books.Single(s => s.ID == _BookID);
                BookID = sach.ID;
                BookName = sach.Name;
                Quantity = soLuong;//chấm value vì decimal
                Price = (decimal)sach.Price;
                TotalPrice = Quantity * Price;
                Image = sach.Image;
            }
        }
        public ItemGioHang(int _BookID)
        {
            //using để đỡ tốn vùng nhớ
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Book sach = db.Books.Single(s => s.ID == _BookID);
                BookID = sach.ID;
                BookName = sach.Name;
                Quantity = 1;
                Price = (decimal)sach.Price;
                TotalPrice = Quantity * Price;
                Image = sach.Image;
            }
        }

        public ItemGioHang()
        {

        }
    }
}