namespace Cw2App.Models;

public class Projector : Equipment
{
    public int Lumens { get; set; }
    public string Resolution { get; set; }

    public Projector(int id, string name, int lumens, string resolution, EquipmentStatus status = EquipmentStatus.Available) 
        : base(id, name, status)
    {
        Lumens = lumens;
        Resolution = resolution;
    }

    public override string ToString()
    {
        return $"{base.ToString()} | Projector [Lumens: {Lumens}, Resolution: {Resolution}]";
    }
}
