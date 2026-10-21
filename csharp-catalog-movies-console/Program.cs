using catalog_movies_console.presentation;
using catalog_movies_console.repository;
using catalog_movies_console.service;

Console.WriteLine("""
                  Привет!
                  Это конспольное приложение "Каталог фильмов"
                  """);
Console.WriteLine();

MovieRepository movieRepository = new MovieRepository();
MovieService movieService = new MovieService(movieRepository);
MovieConsole movieConsole = new MovieConsole(movieService);

if (!movieConsole.InitializeCatalog())
{
    return;
}

while (true)
{
    Console.Write("""
                  Меню:
                  1. Добавить фильм.
                  2. Показать все фильмы.
                  3. Удалить фильм.
                  4. Поставить/изменить оценку.
                  5. Поиск по назаваниию или режиссеру.
                  6. Показать фильмы по жанру.
                  7. Топ-5 фильмов по рейтингу.
                  8. Средний рейтинг по жанрам.
                  9. Сохранить каталог в файл.
                  10. Загрузить каталог из файла.
                  0. Выход

                  Выберите действие: 
                  """);

    string? input = Console.ReadLine();
    int choice;

    if (!int.TryParse(input, out choice))
    {
        Console.WriteLine("Введите корректный номер действия!");
        continue;
    }

    switch (choice)
    {
        case 1:
            movieConsole.CreateMovie();
            Console.WriteLine();
            break;

        case 2:
            movieConsole.ShowAllMovies();
            Console.WriteLine();
            break;

        case 3:
            movieConsole.DeleteMovie();
            Console.WriteLine();
            break;

        case 4:
            movieConsole.UpdateRatingMovieById();
            Console.WriteLine();
            break;

        case 5:
            movieConsole.SearchMoviesByTitleOrDirector();
            Console.WriteLine();
            break;

        case 6:
            movieConsole.SearchMoviesByGenre();
            Console.WriteLine();
            break;

        case 7:
            movieConsole.SearchMoviesByGenreTop5Rating();
            Console.WriteLine();
            break;

        case 8:
            movieConsole.SearchMoviesByGenreAvgRating();
            Console.WriteLine();
            break;

        case 9:
            movieConsole.SaveMovies();
            Console.WriteLine();
            break;

        case 10:
            movieConsole.LoadMovies();
            Console.WriteLine();
            break;

        case 0:
            Console.WriteLine("До свидания!");
            return;

        default:
            Console.WriteLine("Неизвестный пункт меню!");
            break;
    }
}
