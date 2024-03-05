namespace EntityFrameworkCoreMigrations.Entities;

public class ProductRating
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Rating { get; set; }
    public string Review { get; set; }
}