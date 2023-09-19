using MoviesClientConsole;
using System.Diagnostics;

var timer = Stopwatch.StartNew();

var movies = await MinimalApiClient.Get();

Console.WriteLine($"Retrieved {movies.Count} movies in {timer.Elapsed}");
