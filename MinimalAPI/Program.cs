using MinimalAPI.Models;
using MinimalAPI.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme //Adding Bearer JWT Auth option
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference 
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>() 
        }
    });
}); //Adding swagger pages
builder.Services.AddEndpointsApiExplorer(); //adding endpoints to explorer

//adding JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateIssuer =true,
        ValidateAudience =true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});

//Adding Authorization
builder.Services.AddAuthorization();
//adding singleton -- adding dependency injection
builder.Services.AddSingleton<IMovieService, MovieService>();  // Interface , Implementation of the interface
builder.Services.AddSingleton<IUserServices, UserService>();

var app = builder.Build();

app.UseSwagger();   //adding swagger call

app.UseAuthorization();
app.UseAuthentication();
app.MapGet("/", () => "Hello World!").ExcludeFromDescription(); //excluded from swagger

app.MapPost("/login", (UserLogin user, IUserServices service) => Login(user, service))
    .Accepts<UserLogin>("application/json")
    .Produces<string>();

//adding routes to Movie service 
app.MapPost("/create",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "admin")]
    (Movie movie, IMovieService service) => Create(movie, service))
    .Accepts<Movie>("application/json")
    .Produces<Movie>(statusCode:201,contentType:"application/json");

app.MapGet("/get",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,user")]
(int id, IMovieService service) => Get(id, service))
       .Produces<Movie>(statusCode: 200, contentType: "application/json");

app.MapGet("/list",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,user")]
(IMovieService service) => List(service))
    .Produces<List<Movie>>(statusCode: 200, contentType: "application/json");

app.MapPut("/update",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
(Movie newMovie, IMovieService service) => Update(newMovie, service))
    .Accepts<Movie>("application/json")
    .Produces<Movie>(statusCode: 200, contentType: "application/json");

app.MapDelete("/delete",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
(int id, IMovieService service) => Delete(id, service))
    .Produces<string>(statusCode: 204, contentType: "application/json");

IResult Login(UserLogin user, IUserServices service)
{
    if(!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Password))
    {
        var loggedInUser = service.Get(user);
        if (loggedInUser is null) return Results.Unauthorized();

        var claims = new[]
        {
         new Claim(ClaimTypes.NameIdentifier, loggedInUser.UserName),
         new Claim(ClaimTypes.Email, loggedInUser.Email),
         new Claim(ClaimTypes.GivenName, loggedInUser.FirstName +" " + loggedInUser.LastName),
         new Claim(ClaimTypes.Role, loggedInUser.Role)

     };
        var token = new JwtSecurityToken(
            issuer: builder.Configuration["JWT:Issuer"],
            audience: builder.Configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),SecurityAlgorithms.HmacSha256)
            );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
    return Results.Ok(tokenString);

    }
    return Results.Unauthorized();
}

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
    