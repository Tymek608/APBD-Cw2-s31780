namespace APBD1;
using APBD1.Services;
class Program
{
    static void Main(string[] args)
    {
        //11
        Laptop laptop = new Laptop()
        {
            name = "hp", gpu = "nvidia", RamSize = 4
        };
        IEquipmentRepository equipmentRepository = new InMemoryEquipmentRepository();
        List<Equipment> equipments = equipmentRepository.GetAll();
        equipmentRepository.Add(laptop);
        foreach (Equipment eq in equipments)
        {
            Console.WriteLine($"ID: {eq.id} | Nazwa: {eq.name} | Status: {eq.Status}");
        }

        //12
        Student student = new Student()
        {
            id = "1", name = "ojcze", lastname = "donis"
        };
        IUserRepository userRepository = new InMemoryUserRepository();
        List<User> users = userRepository.GetAll();
        userRepository.Add(student);
        foreach (User user in users)
        {
            Console.WriteLine($"ID: {user.id} | Nazwa: {user.name} | Status: {user.lastname} | Typ {user.userType}");
        }
        //13
        var rentalRepo = new InMemoryRentalRepository();
        var rentalService = new RentalService(userRepository, equipmentRepository, rentalRepo);

        var s2 = new Student { id = "S100", name = "Tymoteusz", lastname = "Tester" };
        var l2 = new Laptop { name = "MacBook Pro", gpu = "M3", RamSize = 16 };

        userRepository.Add(s2);
        equipmentRepository.Add(l2);

        Console.WriteLine($"\n--- STAN POCZĄTKOWY ---");
        Console.WriteLine($"Sprzęt: {laptop.name}, Status: {laptop.Status}");

        Console.WriteLine($"\n--- PRÓBA 1: Wypożyczamy MacBooka dla {student.name} ---");
        rentalService.RentEquipment("S100", laptop.id);

        Console.WriteLine($"\n--- PRÓBA 2: Ponowne wypożyczenie tego samego sprzętu ---");
        rentalService.RentEquipment("S100", laptop.id);

        Console.WriteLine($"\n--- PRÓBA 3: Nieistniejący użytkownik ---");
        rentalService.RentEquipment("999", laptop.id);

        Console.WriteLine($"\n--- STAN KOŃCOWY ---");
        Console.WriteLine($"Sprzęt: {laptop.name}, Status: {laptop.Status}");

        //14 — Próba wykonania niepoprawnych operacji
        Console.WriteLine("\n========================================");
        Console.WriteLine("ZADANIE 14: Niepoprawne operacje");
        Console.WriteLine("========================================");

        // Przygotowanie: student z limitem 2, trzy różne sprzęty
        var student14 = new Student { id = "STU14", name = "Anna", lastname = "Kowalska" };
        var eq1 = new Laptop  { name = "Laptop A", gpu = "RTX", RamSize = 8 };
        var eq2 = new Projector { name = "Projektor B", Color = "Biały", LampHours = 500 };
        var eq3 = new Camera  { name = "Aparat C", Brand = "Canon", Megapixels = 24 };
        var eqRented = new Laptop { name = "Laptop ZAJĘTY", gpu = "GTX", RamSize = 4 };
        eqRented.Status = "Rented"; // ręcznie ustawiamy jako niedostępny

        userRepository.Add(student14);
        equipmentRepository.Add(eq1);
        equipmentRepository.Add(eq2);
        equipmentRepository.Add(eq3);
        equipmentRepository.Add(eqRented);

        Console.WriteLine($"\n[TEST A] Wypożyczenie sprzętu już niedostępnego (Status=Rented):");
        rentalService.RentEquipment("STU14", eqRented.id);

        Console.WriteLine($"\n[TEST B] Student wypożycza 1. przedmiot:");
        rentalService.RentEquipment("STU14", eq1.id);

        Console.WriteLine($"\n[TEST C] Student wypożycza 2. przedmiot (ostatni dozwolony):");
        rentalService.RentEquipment("STU14", eq2.id);

        Console.WriteLine($"\n[TEST D] Student próbuje wypożyczyć 3. przedmiot — PRZEKROCZENIE LIMITU:");
        rentalService.RentEquipment("STU14", eq3.id);

        Console.WriteLine($"\n[TEST E] Nieistniejący użytkownik próbuje wypożyczyć sprzęt:");
        rentalService.RentEquipment("GHOST", eq3.id);

        Console.WriteLine("\n========================================");
        Console.WriteLine($"Stan eq1: {eq1.name} => {eq1.Status}");
        Console.WriteLine($"Stan eq2: {eq2.name} => {eq2.Status}");
        Console.WriteLine($"Stan eq3: {eq3.name} => {eq3.Status} (nie wypożyczony — limit)");
        Console.WriteLine("========================================");
    }
}
        
        
        
        

   