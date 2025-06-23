using System;
using System.Collections.Generic;
using System.Windows.Navigation;

namespace LibraryApp;

public partial class Пользователь
{
    public string Логин { get; set; } = null!;

    public string Пароль { get; set; } = null!;

    public DateOnly ДатаРегистрации { get; set; }

    public string Фио { get; set; } = null!;

    public string НомерТелефона { get; set; } = null!;

    public virtual ICollection<Книга> Книгаs { get; set; } = new List<Книга>();

}
