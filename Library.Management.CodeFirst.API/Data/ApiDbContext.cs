using Library.Management.CodeFirst.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Management.CodeFirst.API.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BorrowingRecord> BorrowingRecords { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Book>()
        //        .Property(b => b.Title)
        //        .IsRequired()
        //        .HasMaxLength(200);
        //    modelBuilder.Entity<User>()
        //        .Property(u => u.Name)
        //        .IsRequired()
        //        .HasMaxLength(100);
        //    modelBuilder.Entity<BorrowingRecord>()
        //        .Property(br => br.BorrowDate)
        //        .HasDefaultValueSql("GETDATE()");
        //}
    }
}
