namespace Cw2App.Models;

public abstract class Equipment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public EquipmentStatus Status { get; set; }

    protected Equipment(int id, string name, EquipmentStatus status = EquipmentStatus.Available)
    {
        Id = id;
        Name = name;
        Status = status;
    }

    public override string ToString()
    {
        return $"[{Id}] {Name} (Status: {Status})";
    }
}
