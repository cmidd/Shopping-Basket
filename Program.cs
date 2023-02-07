using Microsoft.EntityFrameworkCore;
using Shopping;
using Shopping.DataAccess;
using Shopping.Factories;
using Shopping.Repositories;
using Shopping.Services;
using Shopping.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddOptions();
builder.Services.Configure<ApiEndpoints>(builder.Configuration.GetSection(nameof(ApiEndpoints)));
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)));
builder.Services.AddTransient<IBasketRepository, BasketRepository>();
builder.Services.AddTransient<IBasketItemsRepository, BasketItemsRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IVoucherRepository, VoucherRepository>();
builder.Services.AddTransient<IBasketService, BasketService>();
builder.Services.AddTransient<IProductViewModelFactory, ProductViewModelFactory>();
builder.Services.AddTransient<IBasketViewModelFactory, BasketViewModelFactory>();
builder.Services.AddTransient<IHomeViewModelFactory, HomeViewModelFactory>();

builder.Services.AddDbContext<ShoppingContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("shopping")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
