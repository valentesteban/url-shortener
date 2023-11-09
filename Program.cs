using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using url_shortener.Data;
using url_shortener.Data.Implementations;
using url_shortener.Models.Repository.Interface;
using url_shortener.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "AllowOrigin",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// Add services to the container.
builder.Services.AddDbContext<UrlShortenerContext>();
builder.Services.AddControllers();
builder.Services.AddScoped<IXYZService, XYZService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<APIException>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<APIException>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("ConsultaAlumnosApiBearerAuth", new OpenApiSecurityScheme() //Esto va a permitir usar swagger con el token.
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Acá pegar el token generado al loguearse."
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ConsultaAlumnosApiBearerAuth" } //Tiene que coincidir con el id seteado arriba en la definición
            }, new List<string>() }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var context = new UrlShortenerContext())
{
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.UseCors("AllowOrigin");


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();