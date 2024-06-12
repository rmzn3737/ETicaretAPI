﻿using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace ETicaretAPI.API.Extensions
{
    static public class ConfigureExceptionHandlerExtension
    {
        public static void ConfigureExceptionHandler<T>(this WebApplication application,ILogger<T> logger)
        {
            application.UseExceptionHandler(builder=> 
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json; //"application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature!=null)
                    {
                        logger.LogError(contextFeature.Error.Message);//todo Burada logları özelleştirelim, araştıralım.
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new 
                        {
                            StatusCode =context.Response.StatusCode,
                            Message =contextFeature.Error.Message,
                            Tittle = "Beklenmeyen Hata!"
                        }));
                    }
                });
            });
        }
    }
}
