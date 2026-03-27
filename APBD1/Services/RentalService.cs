using System;
using APBD1;

namespace APBD1.Services;

public class RentalService : IRentalService
{
    private readonly IUserRepository _userRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IRentalRepository _rentalRepository;
    private readonly IPenaltyCalculationStrategy _penaltyStrategy;

    public RentalService(IUserRepository userRepo, IEquipmentRepository eqRepo, IRentalRepository rentRepo, IPenaltyCalculationStrategy penaltyStrategy)
    {
        _userRepository = userRepo;
        _equipmentRepository = eqRepo;
        _rentalRepository = rentRepo;
        _penaltyStrategy = penaltyStrategy;
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

    public Result ReturnEquipment(string userId, string equipmentId)
    {
        var user = _userRepository.GetById(userId);
        var equipment = _equipmentRepository.GetById(equipmentId);

        if (user == null || equipment == null) {
            return new Result { Success = false, Message = "Nie znaleziono użytkownika lub sprzętu." };
        }

        var rental = _rentalRepository.GetActiveRental(userId, equipmentId);
        if (rental == null) {
            return new Result { Success = false, Message = $"Brak aktywnego wypożyczenia sprzętu '{equipment.name}' dla użytkownika {user.name}." };
        }

        rental.ReturnDate = DateTime.Now;
        rental.OnTime = rental.ReturnDate <= rental.DueDate;
        equipment.Status = "Active";

        if (rental.OnTime)
        {
            return new Result { 
                Success = true, 
                Message = $"[SUKCES] {user.name} zwrócił '{equipment.name}' W TERMINIE.",
                Penalty = 0,
                DaysLate = 0
            };
        }
        else
        {
            int daysLate = (rental.ReturnDate - rental.DueDate).Days;
            if (daysLate < 1) daysLate = 1; 

            decimal penalty = _penaltyStrategy.Calculate(daysLate);

            return new Result {
                Success = true,
                Message = $"[UWAGA] {user.name} zwrócił '{equipment.name}' PO TERMINIE " +
                          $"(Opóźnienie: {daysLate} dni). Naliczono karę: {penalty} zł.",
                Penalty = penalty,
                DaysLate = daysLate
            };
        }
    }
}

