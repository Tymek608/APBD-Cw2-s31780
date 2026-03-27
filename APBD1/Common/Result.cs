namespace APBD1;

public class Result
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public decimal Penalty { get; set; }     
    public int DaysLate { get; set; }        
}