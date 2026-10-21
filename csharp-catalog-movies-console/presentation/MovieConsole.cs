using catalog_movies_console.model;
using catalog_movies_console.service;

namespace catalog_movies_console.presentation;

public class MovieConsole
{
    private MovieService service;
    private static readonly string filePath = Path.Combine(AppContext.BaseDirectory, "movies.json");

    public MovieConsole(MovieService service)
    {
        this.service = service;
    }

    public void CreateMovie()
    {
        Console.Write("Введите название фильма: ");
        string? title = Console.ReadLine();

        Console.Write("Введите режиссера: ");
        string? director = Console.ReadLine();

        Console.Write("Введите жанр: ");
        string? genre = Console.ReadLine();

        Console.Write("Введите год выпуска: ");
        string? yearInput = Console.ReadLine();

        if (!int.TryParse(yearInput, out int year))
        {
            Console.WriteLine("Ошибка: год должен быть числом!");
            return;
        }

        try
        {
            service.AddMovie(
                title ?? "",
                director ?? "",
                genre ?? "",
                year
            );

            Console.WriteLine("Фильм успешно добавлен!");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    public void ShowAllMovies()
    {
        List<Movie> movies = service.GetAllMovies();
        PrintMovies(movies);
    }

    public void DeleteMovie()
    {
        Console.Write("Введите ID фильма для удаления: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Некорректный ID!");
            return;
        }

        service.DeleteMovieById(id);
    }

    public void UpdateRatingMovieById()
    {
        Console.Write("Введите ID фильма: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Некорректный ID!");
            return;
        }

        Console.Write("Введите рейтинг фильма: ");
        if (!double.TryParse(Console.ReadLine(), out double rating))
        {
            Console.WriteLine("Ошибка: рейтинг должен быть числом!");
            return;
        }

        try
        {
            service.UpdateRatingMovieById(id, rating);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    public void SearchMoviesByTitleOrDirector()
    {
        Console.Write("Введите название режисера или название фильма: ");
        string? input = Console.ReadLine();

        List<Movie> movies = service.SearchMoviesByTitleOrDirector(input);
        PrintMovies(movies);
    }

    public void SearchMoviesByGenre()
    {
        Console.Write("Введите жанр фильма: ");
        string? input = Console.ReadLine();

        List<Movie> movies = service.SearchMoviesByGenre(input);
        PrintMovies(movies);
    }

    public void SearchMoviesByGenreTop5Rating()
    {
        List<Movie> movies = service.SearchMoviesByGenreTop5Rating();
        PrintMovies(movies);
    }

    public void SearchMoviesByGenreAvgRating()
    {
        var statistics = service.SearchMoviesByGenreAvgRating();

        if (statistics.Count == 0)
        {
            Console.WriteLine("Каталог фильмов пуст!");
            return;
        }

        Console.WriteLine("Средний рейтинг по жанрам:");

        foreach (var item in statistics)
        {
            Console.WriteLine(
                $"{item.Genre}: {item.AverageRating:F2}"
            );
        }
    }

    public bool InitializeCatalog()
    {
        try
        {
            service.LoadMovies(filePath);
            Console.WriteLine($"Автоматически загружено фильмов: {service.GetAllMovies().Count}.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось загрузить каталог: {filePath}. {ex.Message}");
            return false;
        }
    }

    public void SaveMovies()
    {
        try
        {
            service.SaveMovies(filePath);
            Console.WriteLine($"Сохранено фильмов: {service.GetAllMovies().Count}. Файл: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при работе с файлом: {ex.Message}");
        }
    }

    public void LoadMovies()
    {
        try
        {
            service.LoadMovies(filePath);
            Console.WriteLine($"Загружено фильмов: {service.GetAllMovies().Count}. Файл: {filePath}");
            ShowAllMovies();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при работе с файлом: {ex.Message}");
        }
    }

    private void PrintMovies(List<Movie> movies)
    {
        if (movies.Count == 0)
        {
            Console.WriteLine("Каталог фильмов пуст!");
            return;
        }

        Console.WriteLine(
            $"{"ID",-5} {"Название",-25} {"Режиссер",-25} " +
            $"{"Год",-8} {"Жанр",-20} {"Рейтинг",-8}"
        );

        Console.WriteLine(new string('-', 100));

        foreach (Movie movie in movies)
        {
            Console.WriteLine(
                $"{movie.Id,-5} {movie.Title,-25} {movie.Director,-25} " +
                $"{movie.Year,-8} {movie.Genre,-20} {movie.Rating,-8:F1}"
            );
        }
    }
}