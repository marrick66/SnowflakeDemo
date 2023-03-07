using Bogus;
using Demo.API.Configuration;
using Demo.API.Functions;
using Demo.API.Middleware;
using Demo.API.Models;

namespace Demo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //This is the custom Kafka sink created for Snowflake intake:
            builder.Services.AddSerilogKafka(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //This middleware is executed before the endpoint function:
            app.UseMiddleware<LogExecutionMiddleware>();

            //The minimal API implementation of the fake user endpoint:
            app.MapGet("/user", () =>
            {
                return User.CreateFake();
            })
            .WithName("GetUser")
            .Produces<UserRecord>(StatusCodes.Status200OK);

            app.Run();
        }
    }
}