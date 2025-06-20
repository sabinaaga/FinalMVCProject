using FinalProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data
{
    public class ApDbContext : IdentityDbContext<AppUser>
    {
        public ApDbContext(DbContextOptions<ApDbContext> option) : base(option) { }

        public DbSet<Slayder> Sliders { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Janr> Janrs { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookImages> BookImages { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<ProductDiscount> ProductDiscounts { get; set; }
        public DbSet<BookAutor> BookAutors { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Autor> Autors { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<NewsBlog> NewsBlogs { get; set; }
        public DbSet<Pictures> Pictures { get; set; }
        public DbSet<SlayderAutor> SlayderAutors { get; set; }
    }
}
