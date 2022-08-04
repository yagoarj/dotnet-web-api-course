using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:Sqlserver"]);

// Add services to the container.
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

app.UseHttpsRedirection();

app.MapPost("/products", (ProductDto productDto, ApplicationDbContext context) =>
{
    var category = context.Categories.Where(c => c.Id == productDto.CategoryId).First();

    var product = new Product
    {
        Name = productDto.Name,
        Description = productDto.Description,
        Category = category
    };

    if (productDto.Tags != null)
    {
        product.Tags = new List<Tag>();

        foreach (var tag in productDto.Tags)
        {
            product.Tags.Add(new Tag
            {
                Name = tag
            });
        }
    }

    context.Products.Add(product);

    context.SaveChanges();

    return Results.Created($"/products/{product.Id}", product.Id);
});

app.MapGet("/products/{id}", ([FromRoute] int id, ApplicationDbContext context) =>
{
    var product = context.Products
    .Include(p => p.Category)
    .Include(p => p.Tags)
    .Where(p => p.Id == id).First();

    if (product != null)
        return Results.Ok(product);

    return Results.NotFound();
});

app.MapPut("/products/{id}", ([FromRoute] int id, ProductDto productDto, ApplicationDbContext context) =>
{
    var product = context.Products
    .Include(p => p.Tags)
    .Where(p => p.Id == id).First();

    var category = context.Categories.Where(c => c.Id == productDto.CategoryId).First();

    product.Category = category;
    product.Name = productDto.Name;
    product.Description = productDto.Description;

    if (productDto.Tags != null)
    {
        product.Tags = new List<Tag>();

        foreach (var tag in productDto.Tags)
        {
            product.Tags.Add(new Tag { Name = tag });
        }
    }

    context.SaveChanges();

    return Results.Ok();
});

app.MapDelete("/products/{id}", ([FromRoute] int id, ApplicationDbContext context) =>
{
    var product = context.Products.Where(p => p.Id == id).First();
    context.Products.Remove(product);

    context.SaveChanges();

    return Results.Ok();
});

app.Run();