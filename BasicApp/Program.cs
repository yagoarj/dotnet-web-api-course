using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

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

app.MapPost("/saveproduct", (Product product) =>
{
    ProductRepository.Add(product);
});

app.MapGet("getproduct/{id}", ([FromRoute] int id) =>
{
    var product = ProductRepository.GetBy(id);
    return product;
});

app.MapPut("editproduct", (Product product) =>
{
    var productSaved = ProductRepository.GetBy(product.Id);
    productSaved.Name = product.Name;
});

app.MapDelete("deleteproduct/{id}", ([FromRoute] int id) =>
{
    var productSaved = ProductRepository.GetBy(id);
    ProductRepository.Remove(productSaved);
});

app.Run();

public static class ProductRepository
{
    public static List<Product> Products { get; set; }

    public static void Add(Product product)
    {
        if (Products == null)
            Products = new List<Product>();

        Products.Add(product);
    }

    public static Product GetBy(int id)
    {
        return Products.FirstOrDefault(p => p.Id == id);
    }

    public static void Remove(Product product)
    {
        Products.Remove(product);
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
}