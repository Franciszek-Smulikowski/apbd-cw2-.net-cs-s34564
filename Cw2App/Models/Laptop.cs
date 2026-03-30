namespace Cw2App.Models;

public class Laptop : Equipment
{
    public int RamGb { get; set; }
    public string CpuModel { get; set; }

    public Laptop(int id, string name, int ramGb, string cpuModel, EquipmentStatus status = EquipmentStatus.Available) 
        : base(id, name, status)
    {
        RamGb = ramGb;
        CpuModel = cpuModel;
    }

    public override string ToString()
    {
        return $"{base.ToString()} | Laptop [RAM: {RamGb} GB, CPU: {CpuModel}]";
    }
}
