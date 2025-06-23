using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MyBdContext _context = new MyBdContext();
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Login_Click(object sender, RoutedEventArgs e)
    {
        var user = _context.Пользовательs
            .FirstOrDefault(u => u.Логин == LoginTextBox.Text && u.Пароль == PasswordBox.Password);

        if (user != null)
        {
            MessageBox.Show("Вход успешен!");
            new Window1().Show();
            Close();
        }
        else
        {
            MessageBox.Show("Неверный логин или пароль!");
        }
    }

    private void Register_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(RegLoginTextBox.Text) ||
            string.IsNullOrEmpty(RegPasswordBox.Password) ||
            string.IsNullOrEmpty(FioTextBox.Text) ||
            string.IsNullOrEmpty(PhoneTextBox.Text))
        {
            MessageBox.Show("Заполните все поля!");
            return;
        }

        if (_context.Пользовательs.Any(u => u.Логин == RegLoginTextBox.Text))
        {
            MessageBox.Show("Логин уже занят!");
            return;
        }

        var newUser = new Пользователь
        {
            Логин = RegLoginTextBox.Text,
            Пароль = RegPasswordBox.Password,
            Фио = FioTextBox.Text,
            НомерТелефона = PhoneTextBox.Text,
            ДатаРегистрации = DateOnly.FromDateTime(DateTime.Now)
        };

        _context.Пользовательs.Add(newUser);
        _context.SaveChanges();
        MessageBox.Show("Регистрация успешна!");
    }
}