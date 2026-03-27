using System;
using APBD1;

namespace APBD1.Services;

public class RentalService : IRentalService
{
    private readonly IUserRepository _userRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IRentalRepository _rentalRepository;

    public RentalService(IUserRepository userRepo, IEquipmentRepository eqRepo, IRentalRepository rentRepo)
    {
        _userRepository = userRepo;
        _equipmentRepository = eqRepo;
        _rentalRepository = rentRepo;
    }

    public void RentEquipment(string userId, string equipmentId)
    {
        var user = _userRepository.GetById(userId);
        var equipment = _equipmentRepository.GetById(equipmentId);

        if (user == null || equipment == null) {
            Console.WriteLine("[BŁĄD] Nie znaleziono użytkownika lub sprzętu.");
            return;
        }

        if (equipment.Status != "Active") {
            Console.WriteLine($"[BŁĄD] Sprzęt '{equipment.name}' jest niedostępny (Status: {equipment.Status}).");
            return;
        }

        int limit = user.userType == "Student" ? 2 : 5;
        int aktualne = _rentalRepository.GetCountByUser(userId);
        if (aktualne >= limit) {
            Console.WriteLine($"[BŁĄD] {user.userType} {user.name} osiągnął limit {limit} wypożyczeń (aktualnie: {aktualne}).");
            return;
        }

        equipment.Status = "Rented";
        var rental = new Rental {
            idUser = userId,
            idEq = equipmentId,
            RentDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(7),  // termin zwrotu: 7 dni
            OnTime = true
        };

        _rentalRepository.Add(rental);
        Console.WriteLine($"[SUKCES] {user.name} wypożyczył '{equipment.name}'. " +
                          $"Termin zwrotu: {rental.DueDate:dd.MM.yyyy} (Aktywne: {aktualne + 1}/{limit})");
    }

    public void ReturnEquipment(string userId, string equipmentId)
    {
        var user = _userRepository.GetById(userId);
        var equipment = _equipmentRepository.GetById(equipmentId);

        if (user == null || equipment == null) {
            Console.WriteLine("[BŁĄD] Nie znaleziono użytkownika lub sprzętu.");
            return;
        }

        var rental = _rentalRepository.GetActiveRental(userId, equipmentId);
        if (rental == null) {
            Console.WriteLine($"[BŁĄD] Brak aktywnego wypożyczenia sprzętu '{equipment.name}' dla użytkownika {user.name}.");
            return;
        }

        rental.ReturnDate = DateTime.Now;
        rental.OnTime = rental.ReturnDate <= rental.DueDate;
        equipment.Status = "Active";

        if (rental.OnTime)
            Console.WriteLine($"[SUKCES] {user.name} zwrócił '{equipment.name}' W TERMINIE " +
                              $"(zwrot: {rental.ReturnDate:dd.MM.yyyy}, termin: {rental.DueDate:dd.MM.yyyy}).");
        else
            Console.WriteLine($"[UWAGA]  {user.name} zwrócił '{equipment.name}' PO TERMINIE " +
                              $"(zwrot: {rental.ReturnDate:dd.MM.yyyy}, termin: {rental.DueDate:dd.MM.yyyy}).");
    }
}
