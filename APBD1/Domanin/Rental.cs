namespace APBD1;

public class Rental
{
    public String idUser { get; set; }
    public String id{ get; set; }
    public String idEq { get; set; }
    public DateTime RentDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public bool OnTime { get; set; }
    public DateTime DueDate { get; set; } 
}