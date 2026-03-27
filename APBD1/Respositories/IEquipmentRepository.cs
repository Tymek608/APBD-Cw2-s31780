namespace APBD1;

public interface IEquipmentRepository
{
    void Add(Equipment equipment);
    List<Equipment> GetAll();
}