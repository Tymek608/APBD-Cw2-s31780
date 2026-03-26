namespace APBD1;


public abstract class Equipment
{
    public string id { get; set;}
    public string name {get; set;}
    public string Status {get; set;}
}

public class Laptop : Equipment
{
    public string gpu { get; set;}
    public int RamSize { get; set;}
    
}

public class Projector : Equipment
{
    public string Color { get; set; }
    public int LampHours { get; set; }
    
}


public class Camera : Equipment
{
    public string Brand { get; set; }
    public int Megapixels { get; set; }
}

