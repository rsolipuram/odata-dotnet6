// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Logging;

// namespace quickstartcore
// {
//     public class Program
//     {
//         public static void Main(string[] args)
//         {
//             CreateHostBuilder(args).Build().Run();
//         }

//         public static IHostBuilder CreateHostBuilder(string[] args) =>
//             Host.CreateDefaultBuilder(args)
//                 .ConfigureWebHostDefaults(webBuilder =>
//                 {
//                     webBuilder.UseStartup<Startup>();
//                 });
//     }
// }

using Microsoft.AspNetCore.OData;
using Microsoft.OpenApi.Models;
using quickstartcore;
using todo;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IDocumentDBRepository<todo.Models.Item>>(o=>new DocumentDBRepository<todo.Models.Item>());
builder.Services.AddControllers().AddOData(options => options.Select().Filter().OrderBy().AddRouteComponents(
            routePrefix: "odata",
            model: Startup.GetEdmModel()));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AspNet6WithOData", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Item}/{action=Index}/{id?}");

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AspNet2WithOData v1"));


app.Run();


