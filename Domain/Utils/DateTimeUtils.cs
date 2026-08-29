namespace Domain.Utils;

public static class DateTimeUtils
{
    public static DateTime Combine(DateOnly date, TimeOnly time)
    {
        return date.ToDateTime(time);
    }
    
    public static DateTime AddDuration(DateTime date, int hoursDuration)
    {
        return date.AddHours(hoursDuration);
    }
}