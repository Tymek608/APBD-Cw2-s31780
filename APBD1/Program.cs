namespace APBD1;

class Program
{
    static void Main(string[] args)
    {
        Laptop laptop = new Laptop()
        {
            name = "hp", gpu = "nvidia", RamSize = 4
        };
        
        Console.WriteLine($"Wygenerowane ID: {laptop.id}");
        Console.WriteLine($"Początkowy status: {laptop.Status}");
        Console.WriteLine($"Nazwa: {laptop.name}");
        Console.WriteLine($"Karta graficzna: {laptop.gpu}");
        Console.WriteLine($"RAM: {laptop.RamSize} GB");

        Student student = new Student()
        {
            id = "1", name = "ojcze", lastname = "donis"
        };

        IUserRepository userRepository = new InMemoryUserRepository();
        
        userRepository.Add(student);
        
        Console.WriteLine($"Dodano użytkownika: {student.name} {student.lastname}");
        Console.WriteLine($"Typ: {student.userType}");

    }
}