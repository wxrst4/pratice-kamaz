namespace catalog_movies_console.model;

public class Movie
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Director { get; set; }

    public int Year { get; set; }

    public string Genre { get; set; }

    public double Rating { get; set; }


    public Movie(int id, string title, string director, int year, string genre)
    {
        Id = id;
        Title = title;
        Director = director;
        Year = year;
        Genre = genre;
    }
    
}