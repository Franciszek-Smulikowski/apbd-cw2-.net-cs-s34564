using System;
using Cw2App.Models;
using Cw2App.Services;

namespace Cw2App.Demo;

public class DemoRunner
{
    public void Run()
    {
        Console.WriteLine("=== INICJALIZACJA SYSTEMU WYPOŻYCZALNI ===");
        
        // Inicjalizacja serwisów
        var equipmentService = new EquipmentService();
        var userService = new UserService();
        var loanPolicy = new LoanPolicy();
        var penaltyCalculator = new PenaltyCalculator();
        var rentalService = new RentalService(equipmentService, userService, loanPolicy, penaltyCalculator);
        var reportService = new ReportService(equipmentService, rentalService, userService);

        // 1. Dodawanie urządzeń
        Console.WriteLine("\n--- 1. Rejestracja Sprzętu ---");
        equipmentService.AddEquipment(new Laptop(1, "Dell XPS 15", 32, "Intel Core i7"));
        equipmentService.AddEquipment(new Laptop(2, "MacBook Air", 16, "Apple M2"));
        equipmentService.AddEquipment(new Projector(3, "Epson EB-FH52", 4000, "1080p"));
        equipmentService.AddEquipment(new Camera(4, "Sony Alpha 7", 24, "50mm F1.8"));
        Console.WriteLine("Pomyślnie dodano 4 urządzenia do magazynu.");

        // 2. Dodawanie użytkowników
        Console.WriteLine("\n--- 2. Rejestracja Użytkowników ---");
        userService.AddUser(new Student(1, "Adam", "Kowalski"));
        userService.AddUser(new Employee(2, "Anna", "Nowak"));
        Console.WriteLine("Pomyślnie zarejestrowano studenta i pracownika.");

        // 3. Poprawne wypożyczenia
        Console.WriteLine("\n--- 3. Poprawne Wypożyczenia ---");
        var wypoz1 = rentalService.BorrowEquipment(1, 1, 5); // Student pożycza sprzęt nr 1 na 5 dni
        Console.WriteLine($"Student pomyslnie wypozycza: {wypoz1}");
        
        var wypoz2 = rentalService.BorrowEquipment(1, 2, 3); // Student pożycza sprzęt nr 2 na 3 dni
        Console.WriteLine($"Student pomyslnie wypozycza: {wypoz2}");

        // 4. Niepoprawna operacja - przekroczenie limitu dla studenta (Max 2)
        Console.WriteLine("\n--- 4. Blokada: Przekroczenie limitu ---");
        try
        {
            rentalService.BorrowEquipment(1, 3, 2);
            Console.WriteLine("[BŁĄD] Wypożyczono sprzęt pomimo przekroczenia limitu!");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"[OCZEKIWANE] Blokada systemu: {ex.Message}");
        }

        // 5. Niepoprawna operacja - wypożyczenie niedostępnego sprzętu
        Console.WriteLine("\n--- 5. Blokada: Niedostępny sprzęt ---");
        try
        {
            rentalService.BorrowEquipment(2, 1, 5); // Pracownik próbuje wypożyczyć już zajęty laptop nr 1
            Console.WriteLine("[BŁĄD] Wypożyczono niedostępny sprzęt!");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"[OCZEKIWANE] Blokada systemu: {ex.Message}");
        }

        // 6. Zwrot w terminie
        Console.WriteLine("\n--- 6. Zwracanie sprzętu w terminie ---");
        rentalService.ReturnEquipment(wypoz1.Id, DateTime.Now.AddDays(2)); // Oddano po 2 dniach (na 5 regulaminowych)
        Console.WriteLine($"Sprzęt z ID {wypoz1.EquipmentId} zwrócony! Naliczone kary: {wypoz1.Penalty} PLN.");

        // 7. Zwrot opóźniony (z karą)
        Console.WriteLine("\n--- 7. Zwracanie sprzętu ze statusem opóźnienia ---");
        var opoznionyZwrot = DateTime.Now.AddDays(7); // Zwrot 7 po dniach (regulaminowe 3)
        rentalService.ReturnEquipment(wypoz2.Id, opoznionyZwrot);
        Console.WriteLine($"Sprzęt z ID {wypoz2.EquipmentId} zwrócony opóźniony! Naliczone kary: {wypoz2.Penalty} PLN.");

        // Dodanie wypozyczenia dla pracownika, zeby pokazac cos w aktywnych
        var wypoz3 = rentalService.BorrowEquipment(2, 3, 14);
        Console.WriteLine($"\nDodatkowe wypożyczenie (Pracownik): {wypoz3}");

        // 8. Podsumowanie i raporty
        Console.WriteLine("\n--- 8. Raporty Końcowe ---");
        Console.WriteLine(reportService.GetSystemSummaryReport(DateTime.Now.AddDays(4)));
        Console.WriteLine(reportService.GetEquipmentReport());
    }
}
