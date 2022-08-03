using Microsoft.AspNetCore.Mvc;

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

    if(productDto.Tags != null)
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

app.MapGet("/products/{id}", ([FromRoute] int id) =>
{
    var product = ProductRepository.GetBy(id);

    if (product != null)
        return Results.Ok(product);

    return Results.NotFound();
});

app.MapPut("/products", (Product product) =>
{
    var productSaved = ProductRepository.GetBy(product.Id);
    productSaved.Name = product.Name;

    return Results.Ok();
});

app.MapDelete("/products/{id}", ([FromRoute] int id) =>
{
    var productSaved = ProductRepository.GetBy(id);
    ProductRepository.Remove(productSaved);

    return Results.Ok();
});

app.Run();