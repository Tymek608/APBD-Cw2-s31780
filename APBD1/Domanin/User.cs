namespace APBD1;

public abstract class User
{
    public string id { get; set; }
    public string name { get; set; }
    public string lastname { get; set; }
    
    public abstract string userType { get;}
}

public class Student : User
{
    public override string userType => "Student";
}

public class Employee : User
{
    public override string userType => "Employee";
}
