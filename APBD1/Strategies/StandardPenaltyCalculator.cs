namespace APBD1;

public class StandardPenaltyCalculator : IPenaltyCalculationStrategy
{
    private const decimal PenaltyPerDay = 10m;

    public decimal Calculate(int daysLate)
    {
        if (daysLate <= 0) return 0m;
        return daysLate * PenaltyPerDay;
    }
}