﻿using System.Security.Claims;
using System.Text;
using ETicaret.Application;
using ETicaret.Application.Validators.Products;
using ETicaretAPI.Persistence;
using FluentValidation.AspNetCore; //El ile yazdık, yoksa eletmiyor.
using ETicaretAPI.Infrastructure.Filters;
using ETicaretAPI.Infrastructure;
using ETicaretAPI.Infrastructure.Enums;
using ETicaretAPI.Infrastructure.Services.Storage.Azure;
using ETicaretAPI.Infrastructure.Services.Storage.Local;

using ETicaret.Application.Abstractions.Storage.Local;
using ETicaretAPI.API.Configurations.ColumnWriters;
using ETicaretAPI.API.Extensions;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using ETicretAPI.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddHttpContextAccessor();//Client'tan gelen request neticesinde oluşmakta olan HttpContext nesnesine katmanlardaki class'lar üzerinden(business Logic) erişebilmemizi sağlayan bir servistir.
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
//builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(LibraryEntrypoint).Assembly));

builder.Services.AddApplicationServices();
builder.Services.AddSignalRServices();
//todo alttaki değişik kullanımlar var.
//builder.Services.AddStorage(StorageType.Azure);
builder.Services.AddStorage<LocalStorage>();
//builder.Services.AddStorage<AzureStorage>();
//builder.Services.AddStorage(ETicaretAPI.Infrastructure.Enums.StorageType.Local);
/*builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>

    //policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()//Her yerden gelen istekler.
    policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));*/ //Burayı Archa geçtikten sonra deneme amaçlı yorum satırı yaptık, sonra duruma göre açabiliriz.

    builder.Services.AddCors(options => options.AddDefaultPolicy(policy=>policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"),"logs",needAutoCreateTable:true,columnOptions:new Dictionary<string, ColumnWriterBase>
    {
        {"message",new RenderedMessageColumnWriter()},
        {"message_tamplate",new MessageTemplateColumnWriter()},
        {"level",new LevelColumnWriter()},
        {"time_stamp",new TimestampColumnWriter()},
        {"exception",new ExceptionColumnWriter()},
        {"porperties",new PropertiesColumnWriter()},
        {"log_event",new LogEventSerializedColumnWriter()},
        {"user_name",new UsernameColumnWriter()}
    })
    .WriteTo.Seq(builder.Configuration["Seq:ServerURL"])
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();
builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    //logging.ResponseHeaders.Add("MyResponseHeader");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.CombineLogs = true;
});

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,//Oluşturulacak token değerini kimlerin/hangi orijinlerin/sitelerin kullancağını belirlediğimiz değerdir. --->www.bilmemne.com
            ValidateIssuer = true,//Oluşturulacak token değerini kimin dağıttığını ifade edeceğimiz alandır.--->www.myapi.com

            ValidateLifetime = true,//Oluşturulan token değerinin süresini kontrol edecek doğrulamadır.
            ValidateIssuerSigningKey = true,//Üretilecek token değerinin uygulamamıza ait olduğunu ifade eden security key değerinin doğrulanmasıdır.
            
            //ClockSkew = TimeSpan.Zero,

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow: false, //Bu bir delegatemiş.

            NameClaimType = ClaimTypes.Name //JWT üzerinde Name claimne karþýlýk gelen deðeri User.Identity.Name propertysinden elde edebiliriz.
            /*LifetimeValidator = (notBefore, expires, token, parameters) =>
            {
                // Burada kendi özel doğrulama mantığınızı yazabilirsiniz.
                if (expires != null)
                {
                    return expires > DateTime.UtcNow.AddHours(3);
                }
                return false;//Bu kodu chtGPT ye yazdırmıştık, üstteki hocaya ait, JWT ile apiye bağlanma sürecinde aşmakta zorlandığımız en önemli hatalardan biriydi.
            }*/,

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]))
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

app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());

app.UseStaticFiles();

app.UseSerilogRequestLogging();

app.UseHttpLogging();

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("user_name", username);
    await next();
});

app.MapControllers();
app.MapHubs();

app.Run();

//**************//
//using ETicaret.Application.Validators.Products;
//using ETicaretAPI.Application;
//using ETicaret.Application.Validators.Products;
//using ETicaretAPI.Infrastructure;
//using ETicaretAPI.Infrastructure.Filters;
//using ETicaretAPI.Infrastructure.Services.Storage.Azure;
//using ETicaretAPI.Infrastructure.Services.Storage.Local;
//using ETicaretAPI.Persistence;
//using FluentValidation.AspNetCore;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using ETicaret.Application;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddPersistenceServices();
//builder.Services.AddInfrastructureServices();
//builder.Services.AddApplicationServices();
////builder.Services.AddAplicationServices();

////builder.Services.AddStorage<LocalStorage>();
//builder.Services.AddStorage<AzureStorage>();
////builder.Services.AddStorage();

//builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
//    policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod()
//));

//builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
//    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
//    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer("Admin", options =>
//    {
//        options.TokenValidationParameters = new()
//        {
//            ValidateAudience = true, //Olu�turulacak token de�erini kimlerin/hangi originlerin/sitelerin kullan�c� belirledi�imiz de�erdir. -> www.bilmemne.com
//            ValidateIssuer = true, //Olu�turulacak token de�erini kimin da��tt�n� ifade edece�imiz aland�r. -> www.myapi.com
//            ValidateLifetime = true, //Olu�turulan token de�erinin s�resini kontrol edecek olan do�rulamad�r.
//            ValidateIssuerSigningKey = true, //�retilecek token de�erinin uygulamam�za ait bir de�er oldu�unu ifade eden suciry key verisinin do�rulanmas�d�r.

//            ValidAudience = builder.Configuration["Token:Audience"],
//            ValidIssuer = builder.Configuration["Token:Issuer"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]))
//        };
//    });

//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseStaticFiles();
//app.UseCors();
//app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();
