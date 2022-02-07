using MinimalAPI.Models;
using MinimalAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(); //Adding swagger pages
builder.Services.AddEndpointsApiExplorer(); //adding endpoints to explorer

//adding singleton -- adding dependency injection
builder.Services.AddSingleton<IMovieService, MovieService>();  // Interface , Implementation of the interface
builder.Services.AddSingleton<IUserServices, UserService>();

var app = builder.Build();

app.UseSwagger();   //adding swagger call
app.MapGet("/", () => "Hello World!");

//adding routes to Movie service 
app.MapPost("/create", (Movie movie, IMovieService service) => Create(movie, service));

app.MapGet("/get", (int id, IMovieService service) => Get(id, service));

app.MapGet("/list", (IMovieService service) => List(service));

app.MapPut("/update", (Movie newMovie, IMovieService service) => Update(newMovie, service));

app.MapDelete("/delete", (int id, IMovieService service) => Delete(id, service));

IResult Create (Movie movie, IMovieService service)
{
    var result = service.Create(movie);
    return Results.Ok(result);
}
IResult Update(Movie movie, IMovieService service)
{
    var result = service.Update(movie);
    if (result is null) return Results.NotFound("Requested Movie Not Found");
    return Results.Ok(result);
}
IResult Get(int id, IMovieService service)
{
    var result = service.Get(id);
    if (result is null) return Results.NotFound("Requested Movie Not Found");
    return Results.Ok(result);
}
IResult List(IMovieService service)
{
    var result = service.List();
    if (result is null) return Results.NotFound("Requested Movie Not Found");
    return Results.Ok(result);
}
IResult Delete(int id, IMovieService service)
{
    var result = service.Delete(id);
    if (result is false) return Results.BadRequest("Something went wrong");
    return Results.Ok(result);
}

app.UseSwaggerUI(); //Adding swagger UI

app.Run();
    