using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookManagementPresentation.Model;
using BookManagementPresentation.Models;
using Microsoft.AspNet.SignalR;

namespace BookManagementPresentation
{
    public class ChatHub : Hub
    {
        public void Send(string name, string userId, string BookId,string message,string img)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            DateTime date = DateTime.Now;
            Comment m = new Comment();
            m.BookId = int.Parse(BookId);
            m.UserId = userId;
            m.ContentDate = date;
            m.Content = message;
            m.Name = name;
            db.Comments.Add(m);
            db.SaveChanges();
            Clients.All.addNewMessageToPage(message, date.ToString(), img,name, BookId);
        }
    }
}