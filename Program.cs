using ManageProduct_Microservice.Models;
using ManageProduct_Microservice.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//var MyCorsPolicy = "myCorsPolicy";

// Add services to the container.

/*builder.Services.Configure<ProductDatabaseSettings>(
    builder.Configuration.GetSection(nameof(ProductDatabaseSettings)));*/

builder.Services.Configure<ProductDatabaseSettings>(options =>
{
    options.ConnectionString = Environment.GetEnvironmentVariable("ConnectionString");
    options.DatabaseName = Environment.GetEnvironmentVariable("DatabaseName");
    options.ProductCollectionName = Environment.GetEnvironmentVariable("ProductCollectionName");
});

builder.Services.AddSingleton<IProductDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<ProductDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration.GetValue<string>("ProductDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<IProductService,  ProductService>();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:issuer"],
        ValidAudience = builder.Configuration["Jwt:issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

/*
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyCorsPolicy,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000/",
                                              "http://localhost:3001/",
                                              "http://localhost:3002/");
                      });
});
*/

builder.Services.AddCors(option =>
    option.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    }
    ));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
