﻿using BookStore.Data;
using Microsoft.EntityFrameworkCore;
using BookStore.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;


namespace BookStore.Models
{
        public class SeedData
        {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<BookStoreUser>>();
            IdentityResult roleResult;
            //Add Admin Role
            var roleAdminCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleAdminCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            //Add User Role
            var roleUserCheck = await RoleManager.RoleExistsAsync("User");
            if (!roleUserCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("User")); }

            BookStoreUser user = await UserManager.FindByEmailAsync("admin@bookstore.com");
            if (user == null)
            {
                var User = new BookStoreUser();
                User.Email = "admin@bookstore.com";
                User.UserName = "admin@bookstore.com";
                string pass = "Admin123";
                IdentityResult userr = await UserManager.CreateAsync(User, pass);
                //Add default User to Role Admin      
                if (userr.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }
        }
        public static void Initialize(IServiceProvider serviceProvider)
            {
                using (var context = new BookStoreContext(
                    serviceProvider.GetRequiredService<
                        DbContextOptions<BookStoreContext>>()))
                {
                CreateUserRoles(serviceProvider).Wait();
                if (context.Author.Any() || context.Books.Any() || context.BookGenres.Any() || context.Genre.Any() || context.Review.Any() || context.UserBooks.Any())
                    {
                        return;   // DB has been seeded
                    }


                    context.Author.AddRange(
                        new Author { /*Id = 1, */FirstName = "Yuval", LastName = "Harari", BirthDate = DateTime.Parse("1976-02-24"), Nationality = "Israeli", Gender = "Male" },
                        new Author { /*Id = 2, */FirstName = "Margaret", LastName = "Atwood", BirthDate = DateTime.Parse("1939-11-18"), Nationality = "Canadian", Gender = "Female" },
                        new Author { /*Id = 3, */FirstName = "Rebecca", LastName = "Skloot", BirthDate = DateTime.Parse("1972-09-19"), Nationality = "American", Gender = "Female" },
                        new Author { /*Id = 4, */FirstName = "Francis Scott", LastName = "Fitzgerald", BirthDate = DateTime.Parse("1896-09-24"), Nationality = "American", Gender = "Male" },
                        new Author { /*Id = 5, */FirstName = "John", LastName = "Carreyrou", BirthDate = DateTime.Parse("1973-08-21"), Nationality = "French-American", Gender = "Male" },
                        new Author { /*Id = 6, */FirstName = "Julie", LastName = "James", BirthDate = DateTime.Parse("1995-6-17"), Nationality = "American", Gender = "Female" }
                        
                    );
                    context.SaveChanges();

                context.Books.AddRange(
                    new Books
                    {
                        //Id = 1,
                        Title = "Sapiens: A Brief History of Humankind",
                        ReleaseYear = 2014,
                        NumPages = 443,
                        Description = "A sweeping history of the human species and its evolution from prehistoric times to the present day.",
                        Publisher = "Harper Collins",
                        FrontPage = "1.png",
                        DownloadUrl = "book.pdf",
                        AuthorId = 1
                    },
                    new Books
                    {
                        //Id = 2,
                        Title = "The Testaments",
                        ReleaseYear = 2019,
                        NumPages = 419,
                        Description = "A sequel to The Handmaid's Tale that continues the story of the Republic of Gilead.",
                        Publisher = "Nan A.Talese",
                        FrontPage = "2.png",
                        DownloadUrl = "book.pdf",
                        AuthorId = 2
                    },
                    new Books
                    {
                        //Id = 3,
                        Title = "The Immortal Life of Henrietta Lacks",
                        ReleaseYear = 2010,
                        NumPages = 381,
                        Description = "A non - fiction work that tells the story of Henrietta Lacks, a woman whose cells were used without her knowledge to create the first immortal human cell line.",
                        Publisher = "Crown Publishing Group",
                        FrontPage = "3.jpg",
                        DownloadUrl = "book.pdf",
                        AuthorId = 3
                    },
                    new Books
                    {
                        //Id = 4,
                        Title = "The Great Gatsby",
                        ReleaseYear = 1925,
                        NumPages = 180,
                        Description = "A classic novel that explores the decadence and excess of the Roaring Twenties.",
                        Publisher = "Charles Scribner's Sons",
                        FrontPage = "4.jpg",
                        DownloadUrl = "book.pdf",
                        AuthorId = 4
                    },
                    new Books
                    {
                        //Id = 5,
                        Title = "Bad Blood: Secrets and Lies in a Silicon Valley Startup",
                        ReleaseYear = 2018,
                        NumPages = 339,
                        Description = "An investigative journalism work that uncovers the deception and fraud behind the rise and fall of the biotech company Theranos.",
                        Publisher = "Knopf Doubleday Publishing Group",
                        FrontPage = "5.jpg",
                        DownloadUrl = "book.pdf",
                        AuthorId = 5
                    },
                    new Books
                    {
                        //Id = 6,
                        Title = "Just the sexiest man alive",
                        ReleaseYear = 2020,
                        NumPages = 320,
                        Description = " A novel by Julie James about a man that brings down all the stereotipes about sexy men and their dumb decisions. ",
                        Publisher = "I want books Publishment",
                        FrontPage = "6.jpg",
                        DownloadUrl = "book.pdf",
                        AuthorId = 6
                    },
                    new Books
                    {
                        //Id = 7,
                        Title = "The Beautiful and Damned",
                        ReleaseYear = 1922,
                        NumPages = 351,
                        Description = "A novel that explores the lives of wealthy socialites Anthony and Gloria Patch as they descend into alcoholism and excess in 1920s New York.",
                        Publisher = "Scribner's Sons",
                        FrontPage = "7.jpg",
                        DownloadUrl = "book.pdf",
                        AuthorId = 4
                    }

                  );

                    context.SaveChanges();

                    context.Genre.AddRange(
                        new Genre
                        {
                            // Id = 1
                            GenreName = "History"
                        },
                        new Genre
                        {
                            // Id = 2
                            GenreName = "Dystopian Fiction"
                        },
                        new Genre
                        {
                            // Id = 3
                            GenreName = "Science"
                        },
                        new Genre
                        {
                            // Id = 4
                            GenreName = "Literary"
                        },
                        new Genre
                        {
                            // Id = 5
                            GenreName = "Investigative Journalism"
                        },
                        new Genre
                        {
                            // Id = 6
                            GenreName = "Romance"
                        },
                        new Genre
                        {
                            // Id = 7
                            GenreName = "Fiction Literary"
                        }
                        
                    );

                    context.SaveChanges();

                    context.BookGenres.AddRange(
                        new BookGenre
                        {
                            BookId = 1,
                            GenreId = 1
                        },
                        new BookGenre
                        {
                            BookId = 5,
                            GenreId = 7
                        },
                        new BookGenre
                        {
                            BookId = 1,
                            GenreId = 6
                        },
                        new BookGenre
                        {
                            BookId = 3,
                            GenreId = 3
                        }
                        
                    );

                    context.SaveChanges();

                    context.Review.AddRange(
                        new Review
                        {
                            BookId = 1,
                            AppUser = "Melani",
                            Rating = 8,
                            Comment = "Best history book."
                        },
                        new Review
                        {
                            BookId = 7,
                            AppUser = "Tamara",
                            Rating = 7,
                            Comment = "beautiful words from one of the best authors there have ever been"
                        },
                        new Review
                        {
                            BookId = 5,
                            AppUser = "Nenad",
                            Rating = 7,
                            Comment = "BEST BOOK EVER!!! I relate to it so much"
                        }
                        
                    );

                    context.SaveChanges();
                }
            }
        }
    }

