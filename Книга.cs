using System;
using System.Collections.Generic;

namespace LibraryApp;

public partial class Книга
{
    public string Артикул { get; set; } = null!;

    public string Название { get; set; } = null!;

    public string Жанр { get; set; } = null!;

    public string? Описание { get; set; }

    public DateOnly ДатаВыпуска { get; set; }

    public string Статус { get; set; } = null!;

    public string? Читатель { get; set; }

    public virtual Пользователь? ЧитательNavigation { get; set; }
}
