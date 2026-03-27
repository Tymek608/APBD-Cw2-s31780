namespace APBD1;

public interface IRentalRepository
{
    void Add(Rental rental);
    int GetCountByUser(string userId);
}