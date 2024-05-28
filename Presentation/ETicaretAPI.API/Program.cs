using ETicaret.Application;
using ETicaret.Application.Validators.Products;
using ETicaretAPI.Persistence;
using FluentValidation.AspNetCore; //El ile yazd�k, yoksa eletmiyor.
using ETicaretAPI.Infrastructure.Filters;
using ETicaretAPI.Infrastructure;
using ETicaretAPI.Infrastructure.Enums;
using ETicaretAPI.Infrastructure.Services.Storage.Azure;
using ETicaretAPI.Infrastructure.Services.Storage.Local;

using ETicaret.Application.Abstractions.Storage.Local;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
//builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(LibraryEntrypoint).Assembly));

builder.Services.AddAplicationServices();

//todo alttaki de�i�ik kullan�mlar var.
//builder.Services.AddStorage(StorageType.Azure);
builder.Services.AddStorage<LocalStorage>();
//builder.Services.AddStorage<AzureStorage>();
//builder.Services.AddStorage(ETicaretAPI.Infrastructure.Enums.StorageType.Local);
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>

    //policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()//Her yerden gelen istekler.
    policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod()
));
builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter=true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication("Admin")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,//Olu�turulacak token de�erini kimlerin/hangi orijinlerin/sitelerin kullanca��n� belirledi�imiz de�erdir. --->www.bilmemne.com
            ValidateIssuer = true,//Olu�turulacak token de�erini kimin da��tt���n� ifade edece�imiz aland�r.--->www.myapi.com
            ValidateIssuerSigningKey = true,//�retilecek token de�erinin uygulamam�za ait oldu�unu ifade eden security key de�erinin do�rulanmas�d�r.
            ValidateLifetime = true,//Olu�turulan token de�erinin s�resini kontrol edecek do�rulamad�r.

            ValidAudience = "www.bilmemne.com",
            ValidIssuer = "www.myapi.com",
            IssuerSigningKey= new SymmetricSecurityKey();
        };
    });

//builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var app = builder.Build();


//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
