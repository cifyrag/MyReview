using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyReviewWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MyReviewWeb.Models.Review>? Reviews { get; set; }
        public DbSet<MyReviewWeb.Models.Like> Likes { get; set; }
    }
}