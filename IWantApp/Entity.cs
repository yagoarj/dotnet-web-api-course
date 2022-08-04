namespace IWantApp;

public abstract class Entity
{
    public Entity()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public string Name { get; }
    public string CreatedBy { get; }
    public DateTime CreatedOn { get; }
    public string EditedBy { get; }
    public DateTime EditedOn { get; }

}
