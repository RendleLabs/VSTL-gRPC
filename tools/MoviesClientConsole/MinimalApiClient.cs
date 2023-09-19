using MoviesApi.Models;
using System.Diagnostics;
using System.Net.Http.Json;

namespace MoviesClientConsole;

internal class MinimalApiClient
{
  private static readonly HttpClient http = new HttpClient();

  public static async Task<IList<Movie>> Get()
  {
    using var response = await http.GetAsync("https://localhost:7232/movies/genre/Comedy");

    var movies = await response.Content.ReadFromJsonAsync<Movie[]>();

    return movies ?? [];

  }

  public static async Task GetAll()
  {
    using var http = new HttpClient();
    var list = new List<Movie>();

    var timer = Stopwatch.StartNew();
    int page = 1;

    using var response = await http.GetAsync($"https://localhost:7232/movies/genre/Comedy?page={page}");

    var movies = await response.Content.ReadFromJsonAsync<Movie[]>();

    while (movies?.Length == 100)
    {
      list.AddRange(movies);
      ++page;
      using var response2 = await http.GetAsync($"https://localhost:7232/movies/genre/Comedy?page={page}");
      movies = await response2.Content.ReadFromJsonAsync<Movie[]>();
    }

    if (movies is { Length: > 0 })
    {
      list.AddRange(movies);
    }

    timer.Stop();

    Console.WriteLine($"Retrieved {list.Count} movies in {timer.Elapsed}");
  }
}
