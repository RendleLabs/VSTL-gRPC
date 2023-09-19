namespace MoviesApi.Models;

public class Movie
{
  public int Id { get; set; }
  public required string Title { get; set; }
  public string? OriginalTitle { get; set; }
  public string? Overview { get; set; }
  public string? Homepage { get; set; }
  public float Popularity { get; set; }
  public DateTime ReleaseDate { get; set; }
  public long Revenue { get; set; }
  public int Runtime { get; set; }
  public string Status { get; set; }
  public string? TagLine { get; set; }
  public float VoteAverage { get; set; }
  public int VoteCount { get; set; }
  public List<string> Genres { get; } = new();
  public int Budget { get; set; }
  public string OriginalLanguage { get; set; }

  public static Movie FromEntity(Movies.Data.Movie entity)
  {
    var movie = new Movie
    {
      Id = entity.Id,
      Title = entity.Title,
      OriginalTitle = entity.OriginalTitle,
      Overview = entity.Overview,
      Homepage = entity.Homepage,
      Budget = entity.Budget,
      ReleaseDate = entity.ReleaseDate,
      OriginalLanguage = entity.OriginalLanguage,
      Popularity = entity.Popularity,
      Revenue = entity.Revenue,
      Runtime = entity.Runtime,
      Status = entity.Status.ToString(),
      TagLine = entity.TagLine,
      VoteAverage = entity.VoteAverage,
      VoteCount = entity.VoteCount,
    };
    movie.Genres.AddRange(entity.Genres.Select(g => g.Name));
    return movie;
  }
}
