using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
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
using WpfApp1.DbFirstModel;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();

            //Library2Entities _context = new Library2Entities();
            ////var result = from a in _context.Authors
            ////             select a;


            //var result = from b in _context.Books.Include(nameof(Book.Category))
            //             select b;

            //var list=result.ToList();
            //myDataGrid.ItemsSource = list;

            //UpdateAsync();
            //RemoveAsync();

            CallAdd();
            GetAsync();
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


        private ObservableCollection<Book> allBooks;

        public ObservableCollection<Book> AllBooks
        {
            get { return allBooks; }
            set { allBooks = value; OnPropertyChanged(); }
        }


        public async void CallAdd()
        {
            //var rowCount=await AddAsync();
            //MessageBox.Show(rowCount);
        }

        private async void GetAsync()
        {
            using (var context=new Library2Entities())
            {
                var books = await context
                    .Books
                    .Include(nameof(Book.Author))
                    .Include(nameof(Book.Category))
                    .ToListAsync();
                AllBooks = new ObservableCollection<Book>(books);
               // myDataGrid.ItemsSource = books;
            }
        }

        private async void UpdateAsync()
        {
            using (var context = new Library2Entities())
            {
                var book = await context
                    .Books
                    .FirstOrDefaultAsync(b => b.Id == 5);

                if (book != null)
                {
                    book.Pages=999;
                    context.Entry(book).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
                GetAsync();
            }
        }

        public async Task<string> AddAsync()
        {
            using (var context = new Library2Entities())
            {
                var book = new Book
                {
                    Name = "My New Book",
                    AuthorId = 1,
                    CategoryId = 1,
                    Pages = 1111
                };

                context.Entry(book).State = EntityState.Added;
                return (await context.SaveChangesAsync()).ToString();
            }
        }

        private async void RemoveAsync()
        {
            using (var context = new Library2Entities())
            {
                var book = await context
                    .Books
                    .FirstOrDefaultAsync(b => b.Id == 2);

                if (book != null)
                {
                    //context.Books.Remove(book);
                    //await context.SaveChangesAsync();

                    context.Entry(book).State=EntityState.Deleted;

                    await context.SaveChangesAsync();
                }
            }
            GetAsync();
        }
    }
}
