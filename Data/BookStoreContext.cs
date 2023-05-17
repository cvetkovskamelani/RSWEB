using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BookStore.Areas.Identity.Data;
namespace BookStore.Data
{
    public class BookStoreContext : IdentityDbContext<BookStoreUser>
    {
        public BookStoreContext (DbContextOptions<BookStoreContext> options)
            : base(options)
        {
        }

        public DbSet<BookStore.Models.Books> Books { get; set; } 

        public DbSet<BookStore.Models.Author>? Author { get; set; }

        public DbSet<BookStore.Models.Genre>? Genre { get; set; }

        public DbSet<BookStore.Models.Review>? Review { get; set; }
        public DbSet<BookStore.Models.BookGenre>? BookGenres { get; set; }

        public DbSet<BookStore.Models.UserBooks>? UserBooks { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<BookGenre>()
                .HasOne<Books>(m => m.Books)
                .WithMany(m => m.BookGenres)
                .HasForeignKey(m => m.BookId);

            builder.Entity<BookGenre>()
                .HasOne<Genre>(m => m.Genre)
                .WithMany(m => m.BookGenres)
                .HasForeignKey(m => m.GenreId);

            builder.Entity<Books>()
                .HasOne<Author>(m => m.Author)
                .WithMany(m => m.Books)
                .HasForeignKey(m => m.AuthorId);

            builder.Entity<Review>()
                .HasOne<Books>(m => m.Books)
                .WithMany(m => m.Reviews)
                .HasForeignKey(m => m.BookId);

            builder.Entity<UserBooks>()
                .HasOne<Books>(m => m.Books)
                .WithMany(m => m.Users)
                .HasForeignKey(m => m.BookId);
        }
    }
}
