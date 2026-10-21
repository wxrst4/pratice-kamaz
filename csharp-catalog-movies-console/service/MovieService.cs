using catalog_movies_console.model;
using catalog_movies_console.repository;

namespace catalog_movies_console.service;

public class MovieService
{
    private MovieRepository repository;
    private int id;
    private bool hasUnsavedChanges;

    public MovieService(MovieRepository repository)
    {
        this.repository = repository;
        id = repository
            .GetMovies()
            .Select(movie => movie.Id)
            .DefaultIfEmpty(0)
            .Max();
    }

    public void SaveMovies(string filePath)
    {
        repository.SaveToJson(filePath);
        hasUnsavedChanges = false;
    }

    public void LoadMovies(string filePath)
    {
        if (hasUnsavedChanges)
        {
            throw new InvalidOperationException(
                """
                Есть несохранённые изменения. Сначала сохраните каталог (пункт 9),
                чтобы загрузка их не потеряла.
                """
            );
        }

        repository.LoadFromJson(filePath);

        id = Math
            .Max(id, repository.GetMovies()
            .Select(movie => movie.Id)
            .DefaultIfEmpty(0)
            .Max());
    }

    public void AddMovie(string title, string director, string genre, int year)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Название не может быть пустым");
        }

        if (string.IsNullOrWhiteSpace(director))
        {
            throw new ArgumentException("Режисер не может быть пустым");
        }

        if (string.IsNullOrWhiteSpace(genre))
        {
            throw new ArgumentException("Жанр не может быть пустым");
        }

        if (year < 1900 || year > 2025)
            throw new ArgumentException("Год должен быть от 1900 до 2025");

        Movie movie = new Movie(
            ++id, title, director, year, genre
        );

        repository.AddMovie(movie);
        hasUnsavedChanges = true;
    }

    public List<Movie> GetAllMovies()
    {
        return repository.GetMovies();
    }

    public void DeleteMovieById(int id)
    {
        Movie? movie = repository
            .GetMovies()
            .FirstOrDefault(it => it.Id == id);

        if (movie == null)
        {
            Console.WriteLine("Такого фильма не существует");
            return;
        }

        repository.RemoveMovie(movie);
        hasUnsavedChanges = true;
    }

    public void UpdateRatingMovieById(int id, double rating)
    {
        if (rating < 0 || rating > 10)
        {
            throw new ArgumentException("Рейтинг должен быть от 0 до 10");
        }

        Movie? movie = repository
            .GetMovies()
            .FirstOrDefault(it => it.Id == id);

        if (movie == null)
        {
            Console.WriteLine("Такого фильма не существует");
            return;
        }

        movie.Rating = rating;
        hasUnsavedChanges = true;
    }

    public List<Movie> SearchMoviesByTitleOrDirector(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentException("Пользовательский ввод не может быть пустым");
        }

        return repository
            .GetMovies()
            .Where(it =>
                it.Title.ToLower().Contains(query.ToLower())
                || it.Director.ToLower().Contains(query.ToLower())
            )
            .ToList();
    }

    public List<Movie> SearchMoviesByGenre(string genre)
    {
        if (string.IsNullOrWhiteSpace(genre))
        {
            throw new ArgumentException("Пользовательский ввод не может быть пустым");
        }

        return repository
            .GetMovies()
            .Where(it => it.Genre.ToLower().Contains(genre.ToLower()))
            .ToList();
    }

    public List<Movie> SearchMoviesByGenreTop5Rating()
    {
        return repository
            .GetMovies()
            .OrderByDescending(it => it.Rating)
            .Take(5)
            .ToList();
    }

    public List<(string Genre, double AverageRating)> SearchMoviesByGenreAvgRating()
    {
        return repository
            .GetMovies()
            .GroupBy(movie => movie.Genre)
            .Select(it => (
                it.Key,
                it.Average(it => it.Rating)
            ))
            .ToList();
    }
}