using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using BookManagementPresentation.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookManagementPresentation.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("BookModel", throwIfV1Schema: false)
        {
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<MyUser> MyUsers { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<DonDatHang> DonDatHangs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Message> Messages { get; set; }
        public static ApplicationDbContext Create()

        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().HasRequired(s => s.MyUser)
                .WithRequiredPrincipal(s => s.ApplicationUser);

            modelBuilder.Entity<IdentityUserLogin>().
               HasKey(_ => _.UserId);

            modelBuilder.Entity<IdentityUserRole>().
                    HasKey(_ => new { _.RoleId, _.UserId });

        }
    }
}