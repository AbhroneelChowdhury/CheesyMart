namespace CheesyMart.Data.Entities;

public class BaseEntity
{
    public int Id { get; set; }
    
    public DateTimeOffset LastUpdated { get; set; }
    
}