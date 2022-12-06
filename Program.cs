
//add a using statement for currency
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
//TODO: Add a using directive to give this controller access to your DAL folder
using Kundur_Meghna_HW3.DAL;
using Microsoft.AspNetCore.Hosting;
//create a web application builder
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
//TODO: Add database on Azure so you have a connection string
//TODO: (For HW3 and beyond) Add a connection string here once you have created it on Azure
String connectionString = "Server=tcp:fa22kundurmeghnahw3.database.windows.net,1433;Initial Catalog=fa22kundurmeghnahw3;Persist Security Info=False;User ID=MISAdmin;Password=Password123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(connectionString));
//build the app
var app = builder.Build();
//These lines allow you to see more detailed error messages
app.UseDeveloperExceptionPage();
app.UseStatusCodePages();
//This line allows you to use static pages like style sheets and images
app.UseStaticFiles();
//This marks the position in the middleware pipeline where a routing decision
//is made for a URL.
app.UseRouting();
//This allows the data annotations for currency to work on Macs
app.Use(async (context, next) =>
{
    CultureInfo.CurrentCulture =
System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
    CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;
    await next.Invoke();
});
//This method maps the controllers and their actions to a patter for
//requests that's known as the default route. This route identifies
//the Home controller as the default controller and the Index() action
//method as the default action. The default route also identifies a 
//third segment of the URL that's a parameter named id.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();




