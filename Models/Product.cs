namespace wt_lab12_12auth_validation_savostikova.Models;
// ... и дальше ваш класс Product

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public string Description { get; set; } = "";
}