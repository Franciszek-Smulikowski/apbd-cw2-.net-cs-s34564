using System;

namespace Cw2App.Models;

public class Rental
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EquipmentId { get; set; }
    public DateTime BorrowedAt { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedAt { get; set; }
    public decimal Penalty { get; set; }

    public bool IsReturned => ReturnedAt.HasValue;

    public Rental(int id, int userId, int equipmentId, DateTime borrowedAt, DateTime dueDate)
    {
        Id = id;
        UserId = userId;
        EquipmentId = equipmentId;
        BorrowedAt = borrowedAt;
        DueDate = dueDate;
        Penalty = 0m;
    }

    public override string ToString()
    {
        var returnStatus = IsReturned ? $"Returned at {ReturnedAt:yyyy-MM-dd}" : "Not returned";
        return $"[Rental {Id}] User: {UserId}, Eq: {EquipmentId} | Borrowed: {BorrowedAt:yyyy-MM-dd}, Due: {DueDate:yyyy-MM-dd} | {returnStatus} | Penalty: {Penalty} PLN";
    }
}
