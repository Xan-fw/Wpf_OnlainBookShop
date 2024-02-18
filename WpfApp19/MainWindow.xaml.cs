using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp19
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<Book> newBooks = new List<Book>();

            //newBooks.Add(new Book
            //{
            //    Title = "The Great Gatsby",
            //    Description = "A classic novel by F. Scott Fitzgerald",
            //    Author = "F. Scott Fitzgerald",
            //    BookYear = DateTime.ParseExact("1925.04.10", "yyyy.MM.dd", CultureInfo.InvariantCulture)
            //});

            //newBooks.Add(new Book
            //{
            //    Title = "To Kill a Mockingbird",
            //    Description = "A Pulitzer Prize-winning novel by Harper Lee",
            //    Author = "Harper Lee",
            //    BookYear = DateTime.ParseExact("1960.07.11", "yyyy.MM.dd", CultureInfo.InvariantCulture)
            //});

            //newBooks.Add(new Book
            //{
            //    Title = "1984",
            //    Description = "A dystopian novel by George Orwell",
            //    Author = "George Orwell",
            //    BookYear = DateTime.ParseExact("1949.06.08", "yyyy.MM.dd", CultureInfo.InvariantCulture)
            //});

            //foreach (var newBook in newBooks)
            //{
            //    string fileFullPath = @"C:\Users\rolde\Downloads\1.pdf";

            //    string result = Book.CreateBookAndUploadToDB(newBook, fileFullPath);
            //    Console.WriteLine(result);
            //}


        }
        #region ---- TextBlock Click Prop -----
        private static Int16 MyTimeSpan = 220;

        static Button? ButtonClickedNew = null;
        static Button? ButtonClickedOld = null;

        private void Button_ChangeAppearance(object sender, RoutedEventArgs e)
        {
            Button button = ((Button)sender);

            if (button == ButtonClickedNew) { return; }

            button.Background = new SolidColorBrush(Color.FromArgb(255, 45, 28, 103));
            button.Foreground = new SolidColorBrush();

            ColorAnimation foregroundAnimation = new ColorAnimation
            {
                To = Colors.White,
                Duration = TimeSpan.FromMilliseconds(MyTimeSpan),
            };

            button.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, foregroundAnimation);
        }

        private void Button_ChangeToOriginal(object sender, RoutedEventArgs e)
        {

            Button button = ((Button)sender);

            if (button == ButtonClickedNew) { return; }

            button.Background = new SolidColorBrush(Color.FromArgb(255, 66, 66, 66));
            button.Foreground = new SolidColorBrush(Colors.Wheat);

            ColorAnimation foregroundAnimation = new ColorAnimation
            {
                To = Color.FromArgb(255, 100, 100, 200),
                Duration = TimeSpan.FromMilliseconds(MyTimeSpan),
            };


            button.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, foregroundAnimation);
        }

        public void HandleCategoryButtonClick(Button button, RoutedEventArgs e)
        {
            ButtonClickedOld = ButtonClickedNew;
            ButtonClickedNew = button;
            if (ButtonClickedOld != null)
            {
                Button_ChangeToOriginal(ButtonClickedOld, e);
            }
            Button_ChangeAppearance(button, e);
        }

        #endregion

        #region ----- Open Page -----
        private void Button_OpenPage_General(object sender, RoutedEventArgs e)
        {
            HandleCategoryButtonClick(((Button)sender), e);
            OpenCotegory.NavigationService.Navigate(new Page_General());
        }

        private void Button_OpenPage_Books(object sender, RoutedEventArgs e)
        {
            HandleCategoryButtonClick(((Button)sender), e);
            OpenCotegory.NavigationService.Navigate(new Page_Books());

        }

        private void Button_OpenPage_Authors(object sender, RoutedEventArgs e)
        {
            HandleCategoryButtonClick(((Button)sender), e);
            OpenCotegory.NavigationService.Navigate(new Page_Authors());
        }

        private void Button_OpenPage_Сategories(object sender, RoutedEventArgs e)
        {
            HandleCategoryButtonClick(((Button)sender), e);
            OpenCotegory.NavigationService.Navigate(new Page_Сategories());
        }
        #endregion
    }
}