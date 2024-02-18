using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.Windows;
namespace WpfApp19
{
    public class AppContext : DbContext
    {
        public DbSet<Book>? Books { get; set; } = null;
        public DbSet<PDFFile_Book>? PDFFile_Books { get; set; } = null;

        public AppContext()
        {
            this.Database.EnsureCreated();

        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptions)
        {
            string Server_Name = Environment.MachineName, DataBaseName = "DBLibrary";
            dbContextOptions.UseLazyLoadingProxies().UseSqlServer($"Data Source={Server_Name};" +
                $"DataBase= {DataBaseName}; TrustServerCertificate= true; Integrated Security= false; User ID= admin; Password= admin");
        }
    }

    public class PDFFile_Book
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string? FileName { get; set; }
        public byte[]? FileData { get; set; }

        public void AddFileInfoToThisObj(in string FileFullPath)
        {
            FileName = System.IO.Path.GetFileName(FileFullPath);
            try
            {
                FileData = File.ReadAllBytes(FileFullPath);
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Directory Not Found!");
                throw;
            }
        }

        public void UploadThisFileObjToDbServer()
        {
            using (AppContext db = new AppContext())
            {
                db.PDFFile_Books?.Add(this);
            }
        }

        public void DownloadFileFromDbServer()
        {
            if (FileData != null)
            {
                File.WriteAllBytes(
                    Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.UserProfile)
                    + "\\Downloads", this.FileName), this.FileData);
            }
        }

    }

    public class Book
    {
        public int Id { get; set; }

        [MaxLength(30)]
        public string? Title { get; set; }

        [MaxLength(130)]
        public string? Description { get; set; }

        [MaxLength(30)]
        public string? Author { get; set; }

        public DateTime? BookYear { get; set; }


        public int BookFileId { get; set; }

        public virtual PDFFile_Book? BookFile { get; set; }

        public PDFFile_Book? BookFile_
        {
            get
            {
                using (AppContext db = new AppContext())
                {
                    return db.PDFFile_Books?.Find(BookFileId);
                }
            }
        }

        public static async Task<int?> GetBooksByCountAsync()
        {
            return await Task.Run(() =>
            {
                using (AppContext db = new AppContext())
                {
                    return db.Books?.Count();
                }
            });
        }
        

        public static async Task<List<Book>?> GetBooksByAuthorAsync(string author)
        {
            return await Task.Run(() =>
            {
                using (AppContext db = new AppContext())
                {
                    return db.Books?.Where(b => b.Author == author).ToList();
                }
            });
        }

        public static async Task<List<Book>?> GetBooksByTitleAsync(string title)
        {
            return await Task.Run(() =>
            {
                using (AppContext db = new AppContext())
                {
                    return db.Books?.Where(b => b.Title == title).ToList();
                }
            });
        }

        public static async Task<List<Book>?> GetAllBooksAsync(int startRange,int count)
        {
            return await Task.Run(() =>
            {
                using (AppContext db = new AppContext())
                {
                    return db.Books?.OrderBy(book => book.Id).Take(count).Skip(startRange).ToList();
                }
            });
        }

        public static string CreateBookAndUploadToDB(in Book newBook, in string FileFullPath)
        {
            PDFFile_Book pdfFileBook = new PDFFile_Book();
            pdfFileBook.AddFileInfoToThisObj(FileFullPath);
            pdfFileBook.UploadThisFileObjToDbServer();

            Book book = newBook;
            book.BookFile = pdfFileBook;

            using (AppContext db = new AppContext())
            {
                db.Books?.Add(book);

                db.SaveChanges();
            }

            return "Объект успешно добавлен в базу данных.";
        }


    }
}
