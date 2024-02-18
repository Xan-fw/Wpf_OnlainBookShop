using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp19
{
    /// <summary>
    /// Interaction logic for Page_Authors.xaml
    /// </summary>
    public partial class Page_Authors : Page
    {
        const int AddedBookCount = 16;

        List<Book>? books = new List<Book>();

        static int? BooksCountByDBServer = null;

        private delegate Task<List<Book>?> BooksGetDelegate();

        private static BooksGetDelegate? booksGetDelegate = null;

        private async void LoadBooksAsync()
        {
            List<Book>? newBooks = await booksGetDelegate();

            for (int i = 0; i < newBooks?.Count; i++)
            {
                WrapPanel_Books.Children.Add(
                     CreateGridWithTitleAndText(newBooks[i]?.Title, newBooks[i]?.Description, newBooks[i].Id)
                    );
            }

            books?.AddRange(newBooks);
        }

        private async void LoadBooksCountAsync()
        {
            BooksCountByDBServer = await Book.GetBooksByCountAsync();
        }

        public Page_Authors()
        {
            InitializeComponent();


            LoadBooksCountAsync();

            if (booksGetDelegate == null)
            {
                booksGetDelegate = async () => await Book.GetAllBooksAsync(0, 16);
            }

            LoadBooksAsync();



            //foreach (var book in books)
            //{
            //    Console.WriteLine($"Title: {book.Title}, Description: {book.Description}, Year: {book.BookYear} FileName: {book.BookFile_?.FileName}");
            //    book.BookFile_?.DownloadFileFromDbServer();
            //}


        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            double tolerance = 5.8;

            if (Math.Abs(e.VerticalOffset + e.ViewportHeight - e.ExtentHeight) < tolerance)
            {
                if (books?.Count < BooksCountByDBServer)
                {
                    booksGetDelegate = async () => await Book.GetAllBooksAsync(
                        books.Count, books.Count + AddedBookCount
                        );
                    LoadBooksAsync();
                }
            }
        }

        private Grid CreateGridWithTitleAndText(string title, string description, int id)
        {
            Grid grid = new Grid
            {
                Width = 160,
                Height = 200,
                Background = new SolidColorBrush(Colors.Wheat),
                Margin = new Thickness(18)
            };

            StackPanel stackPanel = new StackPanel();

            TextBlock textTitleBook = new TextBlock
            {
                Text = title + $" ({id})",
                Foreground = Brushes.Black,
                FontSize = 18,
                Width = Double.NaN,
                TextAlignment = TextAlignment.Center,
                Background = new SolidColorBrush(Colors.Tan)
            };

            TextBlock textBoxAdditionalText = new TextBlock
            {
                Text = description,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(5)
            };

            stackPanel.Children.Add(textTitleBook);
            stackPanel.Children.Add(textBoxAdditionalText);

            grid.Children.Add(stackPanel);

            return grid;

        }

        private void GetBooksByAllBook(object sender, RoutedEventArgs e)
        {
            booksGetDelegate = async () => await Book.GetAllBooksAsync(
                       0, 16
                       );
            WrapPanel_Books.Children.Clear();
            LoadBooksAsync();
            textBlock_Heading.Text = "All Books";
            ButtonAllBook.Visibility = Visibility.Collapsed;
        }

        private void GetBooksByAuthors(object sender, RoutedEventArgs e)
        {
            booksGetDelegate = async () => await Book.GetBooksByAuthorAsync(TextBoxTitleBook.Text);
            WrapPanel_Books.Children.Clear();
            books?.Clear();
            LoadBooksAsync();
            textBlock_Heading.Text = "Search Book By Authors";
            ButtonAllBook.Visibility = Visibility.Visible;
        }
    }
}
