using System;
using System.Collections.Generic;
using System.Linq;

namespace APBD1.Services;

public class ReportService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEquipmentRepository _equipmentRepository;

    public ReportService(IRentalRepository rentalRepo, IUserRepository userRepo, IEquipmentRepository eqRepo)
    {
        _rentalRepository = rentalRepo;
        _userRepository = userRepo;
        _equipmentRepository = eqRepo;
    }

    
    public void GenerateLateReturnsReport()
    {
        Console.WriteLine("\n--- RAPORT NIETERMINOWYCH ZWROTOW I OPÓZNIEŃ ---");
        var allRentals = _rentalRepository.GetAll();
        bool anyFound = false;

        foreach (var rental in allRentals)
        {
            
            bool isActiveAndLate = rental.ReturnDate == default && DateTime.Now > rental.DueDate;
            
            bool returnedLate = rental.ReturnDate != default && !rental.OnTime;

            if (isActiveAndLate || returnedLate)
            {
                var user = _userRepository.GetById(rental.idUser);
                var eq = _equipmentRepository.GetById(rental.idEq);
                anyFound = true;

                if (isActiveAndLate)
                {
                    int daysLate = (DateTime.Now - rental.DueDate).Days;
                    Console.WriteLine($"[W TRAKCIE] {user?.name} zalega ze sprzetem '{eq?.name}'. " +
                                      $"Termin mijal: {rental.DueDate:dd.MM.yyyy} ({daysLate} dni opoznienia)");
                }
                else if (returnedLate)
                {
                    int daysLate = (rental.ReturnDate - rental.DueDate).Days;
                    Console.WriteLine($"[ZWRÓCONO]  {user?.name} oddal '{eq?.name}' ze spoznieniem. " +
                                      $"Termin: {rental.DueDate:dd.MM.yyyy}, Zwrot: {rental.ReturnDate:dd.MM.yyyy} " +
                                      $"({daysLate} dni opoznienia)");
                }
            }
        }

        if (!anyFound)
        {
            Console.WriteLine("Brak nieterminowych wypozyczen! Wszyscy uzytkownicy sa punktualni.");
        }
        Console.WriteLine("------------------------------------------------\n");
    }

    
    public void GenerateSystemStateReport()
    {
        Console.WriteLine("\n========================================");
        Console.WriteLine("RAPORT KONCOWY: STAN SYSTEMU");
        Console.WriteLine("========================================");

        var allEquipment = _equipmentRepository.GetAll();
        var allUsers = _userRepository.GetAll();
        var activeRentals = _rentalRepository.GetAll().Where(r => r.ReturnDate == default).ToList();

        int availableEq = allEquipment.Count(e => e.Status == "Active");
        int rentedEq = allEquipment.Count(e => e.Status == "Rented");

        Console.WriteLine($"[STATYSTYKI]");
        Console.WriteLine($"- Calkowita liczba uzytkownikow: {allUsers.Count}");
        Console.WriteLine($"- Calkowita kategoria sprzetu:   {allEquipment.Count}");
        Console.WriteLine($"- W tym dostepnych (Active):     {availableEq}");
        Console.WriteLine($"- W tym wypozyczonych (Rented):  {rentedEq}");

        Console.WriteLine("\n[AKTYWNE WYPOZYCZENIA]");
        if (activeRentals.Count > 0)
        {
            foreach (var rental in activeRentals)
            {
                var user = _userRepository.GetById(rental.idUser);
                var eq = _equipmentRepository.GetById(rental.idEq);
                int daysLeft = (rental.DueDate - DateTime.Now).Days;

                Console.WriteLine($"> Sprzet '{eq?.name}' jest u uzytkownika '{user?.name}' (Typ: {user?.userType}).");
            }
        }
        else
        {
            Console.WriteLine("Brak aktywnych wypozyczen. Wszystko jest na magazynie.");
        }
        
        Console.WriteLine("========================================\n");
    }
}