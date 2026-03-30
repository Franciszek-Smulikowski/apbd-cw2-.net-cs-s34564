using System.Collections.Generic;
using System.Linq;
using Cw2App.Models;

namespace Cw2App.Services;

public class EquipmentService
{
    private readonly List<Equipment> _equipments = new();

    public void AddEquipment(Equipment equipment)
    {
        _equipments.Add(equipment);
    }

    public IEnumerable<Equipment> GetAllEquipment()
    {
        return _equipments;
    }

    public IEnumerable<Equipment> GetAvailableEquipment()
    {
        return _equipments.Where(e => e.Status == EquipmentStatus.Available);
    }

    public Equipment? GetEquipmentById(int id)
    {
        return _equipments.FirstOrDefault(e => e.Id == id);
    }

    public void MarkAsUnavailable(int equipmentId)
    {
        var equipment = GetEquipmentById(equipmentId);
        if (equipment != null)
        {
            equipment.Status = EquipmentStatus.Unavailable;
        }
    }
}
