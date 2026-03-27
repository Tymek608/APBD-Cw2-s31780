namespace APBD1.Services; 

public interface IRentalService
{
    void RentEquipment(string userId, string equipmentId);
    Result ReturnEquipment(string userId, string equipmentId);
}