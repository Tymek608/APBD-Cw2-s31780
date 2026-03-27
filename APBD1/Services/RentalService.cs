using System; // Dla Console i DateTime

namespace APBD1.Services; // MUSI być .Services

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
            Console.WriteLine($"[BŁĄD] Sprzęt {equipment.name} jest zajęty.");
            return;
        }

        equipment.Status = "Rented";
        var rental = new Rental {
            idUser = userId,
            idEq = equipmentId,
            RentDate = DateTime.Now,
            OnTime = true
        };

        _rentalRepository.Add(rental);
        Console.WriteLine($"[SUKCES] {user.name} wypożyczył {equipment.name}.");
    }
}