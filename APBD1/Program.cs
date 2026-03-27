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

        Console.WriteLine($"--- STAN POCZĄTKOWY ---");
        Console.WriteLine($"Sprzęt: {laptop.name}, Status: {laptop.Status}");

       
        Console.WriteLine($"\n--- PRÓBA 1: Wypożyczamy MacBooka dla {student.name} ---");
        rentalService.RentEquipment("S100", laptop.id);

      
        Console.WriteLine($"\n--- PRÓBA 2: Ponowne wypożyczenie tego samego sprzętu ---");
        rentalService.RentEquipment("S100", laptop.id);

        Console.WriteLine($"\n--- PRÓBA 3: Nieistniejący użytkownik ---");
        rentalService.RentEquipment("999", laptop.id);

        Console.WriteLine($"\n--- STAN KOŃCOWY ---");
        Console.WriteLine($"Sprzęt: {laptop.name}, Status: {laptop.Status}");
        
        
        
        

    }
}