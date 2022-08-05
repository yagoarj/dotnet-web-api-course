namespace IWantApp.Domain.Products;

public class Product : Entity
{
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
}
