namespace APBD1;

public interface IRentalRepository
{
    void Add(Rental rental);
    int GetCountByUser(string userId);
    Rental? GetActiveRental(string userId, string equipmentId);
    List<Rental> GetAll();
}