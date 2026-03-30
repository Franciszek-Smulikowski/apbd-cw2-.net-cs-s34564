using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cw2App.Models;

namespace Cw2App.Services;

public class ReportService
{
    private readonly EquipmentService _equipmentService;
    private readonly RentalService _rentalService;
    private readonly UserService _userService;

    public ReportService(EquipmentService equipmentService, RentalService rentalService, UserService userService)
    {
        _equipmentService = equipmentService;
        _rentalService = rentalService;
        _userService = userService;
    }

    public string GetEquipmentReport()
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== Equipment Report ===");
        var equipment = _equipmentService.GetAllEquipment();
        
        if (!equipment.Any())
        {
            sb.AppendLine("No equipment found.");
            return sb.ToString();
        }

        foreach (var item in equipment)
        {
            sb.AppendLine(item.ToString());
        }
        return sb.ToString();
    }

    public string GetAvailableEquipmentReport()
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== Available Equipment Report ===");
        var availableEquipment = _equipmentService.GetAvailableEquipment();
        
        if (!availableEquipment.Any())
        {
            sb.AppendLine("No available equipment.");
            return sb.ToString();
        }

        foreach (var item in availableEquipment)
        {
            sb.AppendLine(item.ToString());
        }
        return sb.ToString();
    }

    public string GetUserActiveRentalsReport(int userId)
    {
        var sb = new StringBuilder();
        var user = _userService.GetUserById(userId);
        
        sb.AppendLine($"=== Active Rentals for User: {(user != null ? $"{user.FirstName} {user.LastName}" : $"ID {userId}")} ===");
        
        var rentals = _rentalService.GetActiveRentalsForUser(userId);
        if (!rentals.Any())
        {
            sb.AppendLine("No active rentals.");
            return sb.ToString();
        }

        foreach (var rental in rentals)
        {
            var equipment = _equipmentService.GetEquipmentById(rental.EquipmentId);
            var eqName = equipment?.Name ?? "Unknown Equipment";
            sb.AppendLine($"{rental} | Item: {eqName}");
        }
        return sb.ToString();
    }

    public string GetOverdueRentalsReport(DateTime today)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"=== Overdue Rentals Report (as of {today:yyyy-MM-dd}) ===");
        
        var overdueRentals = _rentalService.GetOverdueRentals(today);
        if (!overdueRentals.Any())
        {
            sb.AppendLine("No overdue rentals.");
            return sb.ToString();
        }

        foreach (var rental in overdueRentals)
        {
            var user = _userService.GetUserById(rental.UserId);
            var equipment = _equipmentService.GetEquipmentById(rental.EquipmentId);
            
            var userName = user != null ? $"{user.FirstName} {user.LastName}" : "Unknown User";
            var eqName = equipment?.Name ?? "Unknown Equipment";
            
            sb.AppendLine($"{rental} | User: {userName} | Item: {eqName}");
        }
        return sb.ToString();
    }

    public string GetSystemSummaryReport(DateTime today)
    {
        var sb = new StringBuilder();
        var equipment = _equipmentService.GetAllEquipment().ToList();
        var rentals = _rentalService.GetAllRentals().ToList();

        var totalEquipment = equipment.Count;
        var availableEquipment = equipment.Count(e => e.Status == EquipmentStatus.Available);
        var borrowedEquipment = equipment.Count(e => e.Status == EquipmentStatus.Borrowed);
        var unavailableEquipment = equipment.Count(e => e.Status == EquipmentStatus.Unavailable);

        var activeRentals = rentals.Count(r => !r.IsReturned);
        var overdueRentals = _rentalService.GetOverdueRentals(today).Count();

        sb.AppendLine("=== System Summary Report ===");
        sb.AppendLine($"Total Equipment: {totalEquipment}");
        sb.AppendLine($" - Available: {availableEquipment}");
        sb.AppendLine($" - Borrowed: {borrowedEquipment}");
        sb.AppendLine($" - Unavailable: {unavailableEquipment}");
        sb.AppendLine($"Total Active Rentals: {activeRentals}");
        sb.AppendLine($"Total Overdue Rentals: {overdueRentals}");

        return sb.ToString();
    }
}
