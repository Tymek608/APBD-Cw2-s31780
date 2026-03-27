namespace APBD1;

public class InMemoryRentalRepository : IRentalRepository
{
    private readonly List<Rental> _rentals = new();

    public void Add(Rental rental) 
    {
        _rentals.Add(rental);
    }
    public int GetCountByUser(string userId)
    {
        return _rentals.Count(r => r.idUser == userId && r.ReturnDate == default);
    }

    public Rental? GetActiveRental(string userId, string equipmentId)
    {
        return _rentals.FirstOrDefault(r => r.idUser == userId && r.idEq == equipmentId && r.ReturnDate == default);
    }
}
