namespace AnnPrepareLavni.ApiService.Infrastructure;

public interface IDateTimeService
{
    DateTime GetCurrentDateTime();
    DateTime GetUtcNow();
    DateTime AddDays(DateTime date, int days);
    string FormatDateTime(DateTime date, string format);
}

public class DateTimeService : IDateTimeService
{
    public DateTime GetCurrentDateTime()
    {
        return DateTime.Now;
    }

    public DateTime GetUtcNow()
    {
        return DateTime.UtcNow;
    }

    public DateTime AddDays(DateTime date, int days)
    {
        return date.AddDays(days);
    }

    public string FormatDateTime(DateTime date, string format)
    {
        return date.ToString(format);
    }
}