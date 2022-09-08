using Flunt.Validations;

namespace IWantApp.Domain.Products;

public class Category : Entity
{
    public Category(string name, string createdBy, string editedBy)
    {
        Active = true;
        this.Name = name;
        CreatedOn = DateTime.Now;
        this.CreatedBy = createdBy;
        EditedOn = DateTime.Now;
        this.EditedBy = editedBy;

        Validate();
    }

    public void EditInfo(string name, bool active, string editedBy)
    {
        Active = active;
        this.Name = name;
        this.EditedBy = editedBy;
        EditedOn = DateTime.Now;

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Category>()
            .IsNotNull(Name, "Name")
            .IsNotNullOrWhiteSpace(Name, "Name")
            .IsGreaterOrEqualsThan(Name, 3, "Name")
            .IsNotNullOrEmpty(CreatedBy, "CreatedBy")
            .IsNotNullOrEmpty(EditedBy, "EditedBy");

        AddNotifications(contract);
    }
}
