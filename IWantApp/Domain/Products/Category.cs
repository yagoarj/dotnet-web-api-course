namespace IWantApp.Domain.Products;

public class Category : Entity
{
    public Category(string Name)
    {
        Active = true;
        this.Name = Name;
    }
}
