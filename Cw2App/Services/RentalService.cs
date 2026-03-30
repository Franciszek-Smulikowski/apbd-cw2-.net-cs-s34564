using System;
using System.Collections.Generic;
using System.Linq;
using Cw2App.Models;

namespace Cw2App.Services;

public class RentalService
{
    private readonly List<Rental> _rentals = new();
    private readonly EquipmentService _equipmentService;
    private readonly UserService _userService;
    private readonly LoanPolicy _loanPolicy;
    private readonly PenaltyCalculator _penaltyCalculator;
    private int _nextRentalId = 1;

    public RentalService(EquipmentService equipmentService, UserService userService, LoanPolicy loanPolicy, PenaltyCalculator penaltyCalculator)
    {
        _equipmentService = equipmentService;
        _userService = userService;
        _loanPolicy = loanPolicy;
        _penaltyCalculator = penaltyCalculator;
    }

    public Rental BorrowEquipment(int userId, int equipmentId, int days)
    {
        var user = _userService.GetUserById(userId);
        if (user == null)
            throw new InvalidOperationException($"User with ID {userId} not found.");

        var equipment = _equipmentService.GetEquipmentById(equipmentId);
        if (equipment == null)
            throw new InvalidOperationException($"Equipment with ID {equipmentId} not found.");

        if (equipment.Status != EquipmentStatus.Available)
            throw new InvalidOperationException($"Equipment '{equipment.Name}' is not available (Status: {equipment.Status}).");

        var activeRentals = GetActiveRentalsForUser(userId).Count();
        var maxLoans = _loanPolicy.GetMaxActiveLoans(user);

        if (activeRentals >= maxLoans)
            throw new InvalidOperationException($"User '{user.FirstName} {user.LastName}' has reached the maximum number of active rentals ({maxLoans}).");

        var borrowedAt = DateTime.Now;
        var dueDate = borrowedAt.AddDays(days);
        
        var rental = new Rental(_nextRentalId++, userId, equipmentId, borrowedAt, dueDate);
        _rentals.Add(rental);

        equipment.Status = EquipmentStatus.Borrowed;

        return rental;
    }

    public void ReturnEquipment(int rentalId, DateTime returnDate)
    {
        var rental = _rentals.FirstOrDefault(r => r.Id == rentalId);
        if (rental == null)
            throw new InvalidOperationException($"Rental with ID {rentalId} not found.");

        if (rental.IsReturned)
            throw new InvalidOperationException($"Rental with ID {rentalId} has already been returned.");

        var equipment = _equipmentService.GetEquipmentById(rental.EquipmentId);
        if (equipment == null)
            throw new InvalidOperationException($"Equipment with ID {rental.EquipmentId} linked to this rental was not found.");

        rental.ReturnedAt = returnDate;
        rental.Penalty = _penaltyCalculator.CalculatePenalty(rental.DueDate, returnDate);

        equipment.Status = EquipmentStatus.Available;
    }

    public IEnumerable<Rental> GetActiveRentalsForUser(int userId)
    {
        return _rentals.Where(r => r.UserId == userId && !r.IsReturned);
    }

    public IEnumerable<Rental> GetOverdueRentals(DateTime today)
    {
        return _rentals.Where(r => !r.IsReturned && r.DueDate.Date < today.Date);
    }

    public IEnumerable<Rental> GetAllRentals()
    {
        return _rentals;
    }
}
