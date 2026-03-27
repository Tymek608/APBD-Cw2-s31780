using System;
using System.Collections.Generic;
using APBD1.Services;

namespace APBD1;

class Program
{
    static void Main(string[] args)
    {
        
        IEquipmentRepository equipmentRepository = new InMemoryEquipmentRepository();
        IUserRepository userRepository = new InMemoryUserRepository();
        IRentalRepository rentalRepo = new InMemoryRentalRepository();
        IPenaltyCalculationStrategy penaltyStrategy = new StandardPenaltyCalculator();
        
        var rentalService = new RentalService(userRepository, equipmentRepository, rentalRepo, penaltyStrategy);
        var reportService = new ReportService(rentalRepo, userRepository, equipmentRepository);

        
        Laptop laptop = new Laptop() { name = "hp", gpu = "nvidia", RamSize = 4 };
        equipmentRepository.Add(laptop);
        
        Console.WriteLine("--- SPRZET ---");
        foreach (Equipment eq in equipmentRepository.GetAll())
        {
            Console.WriteLine($"ID: {eq.id} | Nazwa: {eq.name} | Status: {eq.Status}");
        }

        
        Student student = new Student() { id = "1", name = "ojcze", lastname = "donis" };
        userRepository.Add(student);
        
        Console.WriteLine("\n--- UZYTKOWNICY ---");
        foreach (User user in userRepository.GetAll())
        {
            Console.WriteLine($"ID: {user.id} | Nazwa: {user.name} | Status: {user.lastname} | Typ {user.userType}");
        }

        
        var s2 = new Student { id = "S100", name = "Tymoteusz", lastname = "Tester" };
        var l2 = new Laptop { name = "MacBook Pro", gpu = "M3", RamSize = 16 };

        userRepository.Add(s2);
        equipmentRepository.Add(l2);

        Console.WriteLine($"\n--- STAN POCZATKOWY ---");
        Console.WriteLine($"Sprzet: {laptop.name}, Status: {laptop.Status}");

        Console.WriteLine($"\n--- PROBA 1: Wypozyczamy MacBooka dla {student.name} ---");
        rentalService.RentEquipment("S100", laptop.id);

        Console.WriteLine($"\n--- PROBA 2: Ponowne wypozyczenie tego samego sprzetu ---");
        rentalService.RentEquipment("S100", laptop.id);

        Console.WriteLine($"\n--- PROBA 3: Nieistniejacy uzytkownik ---");
        rentalService.RentEquipment("999", laptop.id);

        Console.WriteLine($"\n--- STAN KONCOWY ---");
        Console.WriteLine($"Sprzet: {laptop.name}, Status: {laptop.Status}");

        Console.WriteLine("\n========================================");
        Console.WriteLine("ZADANIE 14: Niepoprawne operacje (limity)");
        Console.WriteLine("========================================");

        var student14 = new Student { id = "STU14", name = "Anna", lastname = "Kowalska" };
        var eq1 = new Laptop    { name = "Laptop A", gpu = "RTX", RamSize = 8 };
        var eq2 = new Projector { name = "Projektor B", Color = "Bialy", LampHours = 500 };
        var eq3 = new Camera    { name = "Aparat C", Brand = "Canon", Megapixels = 24 };
        var eqRented = new Laptop { name = "Laptop ZAJETY", gpu = "GTX", RamSize = 4 };
        eqRented.Status = "Rented";

        userRepository.Add(student14);
        equipmentRepository.Add(eq1);
        equipmentRepository.Add(eq2);
        equipmentRepository.Add(eq3);
        equipmentRepository.Add(eqRented);

        Console.WriteLine($"\n[TEST A] Wypozyczenie sprzetu juz niedostepnego:");
        rentalService.RentEquipment("STU14", eqRented.id);

        Console.WriteLine($"\n[TEST B] Student wypozycza 1. przedmiot:");
        rentalService.RentEquipment("STU14", eq1.id);

        Console.WriteLine($"\n[TEST C] Student wypozycza 2. przedmiot (ostatni dozwolony):");
        rentalService.RentEquipment("STU14", eq2.id);

        Console.WriteLine($"\n[TEST D] Student probuje wypozyczac 3. przedmiot - PRZEKROCZENIE LIMITU:");
        rentalService.RentEquipment("STU14", eq3.id);

        Console.WriteLine($"\n[TEST E] Nieistniejacy uzytkownik:");
        rentalService.RentEquipment("GHOST", eq3.id);


        
        Console.WriteLine("\n========================================");
        Console.WriteLine("ZADANIE 15: Logika zwrotu i weryfikacja (Kary)");
        Console.WriteLine("========================================");

        var student15 = new Student { id = "STU15", name = "Marek", lastname = "Nowak" };
        var laptop15  = new Laptop  { name = "Dell XPS", gpu = "Intel Iris", RamSize = 16 };
        var camera15  = new Camera  { name = "Sony Alpha", Brand = "Sony", Megapixels = 33 };

        userRepository.Add(student15);
        equipmentRepository.Add(laptop15);
        equipmentRepository.Add(camera15);

        
        Console.WriteLine("\n--- ZWROT W TERMINIE ---");
        rentalService.RentEquipment("STU15", laptop15.id);
        Result res1 = rentalService.ReturnEquipment("STU15", laptop15.id);
        Console.WriteLine(res1.Message);

       
        Console.WriteLine("\n--- PROBA ZWROTU NIEWYPOZYCZONEGO ---");
        Result resErr = rentalService.ReturnEquipment("STU15", camera15.id);
        Console.WriteLine(resErr.Message);

        
        Console.WriteLine("\n--- ZWROT PO TERMINIE (KARA) ---");
        rentalService.RentEquipment("STU15", camera15.id);
        
        
        var lateRental = rentalRepo.GetActiveRental("STU15", camera15.id);
        if (lateRental != null)
        {
            lateRental.DueDate = DateTime.Now.AddDays(-5); 
        }

        Result res2 = rentalService.ReturnEquipment("STU15", camera15.id);
        Console.WriteLine(res2.Message);

        Console.WriteLine($"[INFO] Naliczona kara wynosi: {res2.Penalty} PLN za {res2.DaysLate} dni spoznienia.");

        
        
        
        var activeLateRental = rentalRepo.GetActiveRental("STU14", eq1.id);
        if (activeLateRental != null)
        {
            activeLateRental.DueDate = DateTime.Now.AddDays(-2); 
        }

        
        reportService.GenerateLateReturnsReport();
        reportService.GenerateSystemStateReport();
    }
}