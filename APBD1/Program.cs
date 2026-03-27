namespace APBD1;

class Program
{
    static void Main(string[] args)
    {
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
        
        
        
        

    }
}