namespace Security.Domain.Models;

public class NavigationItem: Aggregate<long>
{
    public required string Identifier { get; set; }
    public string Module { get; set; }
    public string Name { get; set; }
    public string  Route { get; set; }
    public string  Icon { get; set; }
    public long? ParentId { get; set; }
    public bool IsActive { get; set; }
    public int OrderIndex { get; set; }
    public int Level { get; set; }
    public bool ShowInMenu { get; set; } = false;
    

    // Relacion
    public ICollection<NavigationItem> Children { get; set; } = new List<NavigationItem>();
    
}