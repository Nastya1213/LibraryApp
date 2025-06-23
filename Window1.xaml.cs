using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;

namespace LibraryApp
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private readonly MyBdContext _context = new MyBdContext();
        public Window1()
        {
            InitializeComponent();
            LoadBooks();
        }

        private void LoadBooks()
        {
            using var context = new MyBdContext();
            BooksGrid.ItemsSource = context.Книгаs.Include(b => b.ЧитательNavigation).ToList();
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            using var context = new MyBdContext();
            var book = new Книга
            {
                Артикул = Prompt("Артикул"),
                Название = Prompt("Название"),
                Жанр = Prompt("Жанр"),
                Описание = Prompt("Описание"),
                ДатаВыпуска = DateOnly.Parse(Prompt("Дата выпуска (гггг-мм-дд)")),
                Статус = "Свободна"
            };
            if (book.Артикул != null)
            {
                context.Книгаs.Add(book);
                context.SaveChanges();
                LoadBooks();
            }
        }

        private void EditBook_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is Книга book)
            {
                using var context = new MyBdContext();
                var dbBook = context.Книгаs.Find(book.Артикул);
                if (dbBook != null)
                {
                    dbBook.Название = Prompt("Название", book.Название);
                    dbBook.Жанр = Prompt("Жанр", book.Жанр);
                    dbBook.Описание = Prompt("Описание", book.Описание ?? "");
                    dbBook.ДатаВыпуска = DateOnly.Parse(Prompt("Дата выпуска (гггг-мм-дд)", book.ДатаВыпуска.ToString("yyyy-MM-dd")));
                    context.SaveChanges();
                    LoadBooks();
                }
            }
            else
                MessageBox.Show("Выберите книгу!");
        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is Книга book)
            {
                using var context = new MyBdContext();
                var dbBook = context.Книгаs.Find(book.Артикул);
                if (dbBook != null)
                {
                    context.Книгаs.Remove(dbBook);
                    context.SaveChanges();
                    LoadBooks();
                }
            }
            else
                MessageBox.Show("Выберите книгу!");
        }

        private void IssueBook_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is Книга book)
            {
                using var context = new MyBdContext();
                var dbBook = context.Книгаs.Find(book.Артикул);
                if (dbBook == null)
                {
                    MessageBox.Show("Книга не найдена!");
                    return;
                }
                var login = Prompt("Логин читателя");
                var user = context.Пользовательs.FirstOrDefault(u => u.Логин == login);
                if (user == null)
                {
                    MessageBox.Show("Пользователь не найден!");
                    return;
                }
                if (dbBook.Статус.Trim().ToLower() == "свободна")
                {
                    dbBook.Читатель = login;
                    dbBook.Статус = "Выдана";
                    context.SaveChanges();
                    LoadBooks();
                }
                else
                {
                    MessageBox.Show($"Книга не свободна! Текущий статус: '{dbBook.Статус}'");
                }
            }
            else
                MessageBox.Show("Выберите книгу!");
        }

        private void ReturnBook_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is Книга book)
            {
                using var context = new MyBdContext();
                var dbBook = context.Книгаs.Find(book.Артикул);
                if (dbBook != null && dbBook.Статус.Trim().ToLower() == "выдана")
                {
                    dbBook.Читатель = null;
                    dbBook.Статус = "Свободна";
                    context.SaveChanges();
                    LoadBooks();
                }
                else
                    MessageBox.Show("Выберите выданную книгу!");
            }
            else
                MessageBox.Show("Выберите книгу!");
        }

        private string Prompt(string field, string defaultValue = "")
        {
            var dialog = new Window { Title = $"Введите {field}", Width = 250, Height = 150, Owner = this };
            var stack = new StackPanel { Margin = new Thickness(10) };
            var textBox = new TextBox { Text = defaultValue, Margin = new Thickness(0, 0, 0, 10) };
            var button = new Button { Content = "OK", Width = 80 };
            button.Click += (s, args) => dialog.Close();
            stack.Children.Add(new TextBlock { Text = field });
            stack.Children.Add(textBox);
            stack.Children.Add(button);
            dialog.Content = stack;
            dialog.ShowDialog();
            return textBox.Text.Length > 10 ? textBox.Text.Substring(0, 10) : textBox.Text;
        }
    }
}
