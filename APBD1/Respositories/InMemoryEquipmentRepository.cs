namespace APBD1;

public class InMemoryEquipmentRepository : IEquipmentRepository
{
    public readonly List<Equipment> _equipments = new List<Equipment>();

    public void Add(Equipment equipment)
    {
        _equipments.Add(equipment);
    }

    public List<Equipment> GetAll()
    {
        return  _equipments;
    }
}