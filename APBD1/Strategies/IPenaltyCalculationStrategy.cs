namespace APBD1;

public interface IPenaltyCalculationStrategy
{
    decimal Calculate(int daysLate);
}