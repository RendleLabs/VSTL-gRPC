using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
using MoviesApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Sqlite");

builder.Services.AddDbContextPool<MovieContext>(options =>
{
  options.UseSqlite(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/movies/genre/{genre}", async (string genre, [FromQuery] int? page, MovieContext db) =>
{
  int skip = ((page ?? 1) - 1) * 100;
  var movies = await db.Movies.Where(m => m.Genres.Any(g => g.Name == genre))
    .OrderBy(m => m.Title)
    .Skip(skip)
    .Take(100)
    .ToListAsync();

  var result = movies.Select(m => new MoviesApi.Models.Movie { Name = m.Title }).ToArray();

  return result;
});

app.Run();

