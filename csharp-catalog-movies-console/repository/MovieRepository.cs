using System.Text.Json;
using catalog_movies_console.model;

namespace catalog_movies_console.repository;

public class MovieRepository
{
    private List<Movie> movies;

    public MovieRepository()
    {
        movies = new List<Movie>();
    }

    public void AddMovie(Movie movie)
    {
        movies.Add(movie);
    }

    public List<Movie> GetMovies()
    {
        return movies.ToList(); 
    }

    public void RemoveMovie(Movie movie)
    {
        movies.Remove(movie);
    }

    public void SaveToJson(string filePath)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(movies, options);
        File.WriteAllText(filePath, json);
    }

    public void LoadFromJson(string filePath)
    {
        string json = File.ReadAllText(filePath);
        List<Movie> loadedMovies = JsonSerializer.Deserialize<List<Movie>>(json)
                                   ?? throw new InvalidDataException("Некорректный JSON-каталог");

        if (loadedMovies.Any(movie =>
                movie.Id <= 0 ||
                string.IsNullOrWhiteSpace(movie.Title) ||
                string.IsNullOrWhiteSpace(movie.Director) ||
                string.IsNullOrWhiteSpace(movie.Genre) ||
                movie.Year < 1900 ||
                movie.Year > 2025 ||
                double.IsNaN(movie.Rating) ||
                double.IsInfinity(movie.Rating) ||
                movie.Rating < 0 ||
                movie.Rating > 10))
        {
            throw new InvalidDataException("Файл содержит некорректные данные фильмов");
        }

        if (loadedMovies.Select(movie => movie.Id).Distinct().Count() != loadedMovies.Count)
        {
            throw new InvalidDataException("В файле обнаружены повторяющиеся ID");
        }

        movies = loadedMovies;
    }
}