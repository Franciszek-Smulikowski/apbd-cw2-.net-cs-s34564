namespace Cw2App.Models;

public class Camera : Equipment
{
    public int Megapixels { get; set; }
    public string LensType { get; set; }

    public Camera(int id, string name, int megapixels, string lensType, EquipmentStatus status = EquipmentStatus.Available) 
        : base(id, name, status)
    {
        Megapixels = megapixels;
        LensType = lensType;
    }

    public override string ToString()
    {
        return $"{base.ToString()} | Camera [Megapixels: {Megapixels} MP, Lens: {LensType}]";
    }
}
