namespace APBD1;

public class InMemoryRentalRepository : IRentalRepository
{
    private readonly List<Rental> _rentals = new();

    public void Add(Rental rental) 
    {
        _rentals.Add(rental);
    }
}