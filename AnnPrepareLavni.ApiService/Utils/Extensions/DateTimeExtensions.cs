namespace AnnPrepareLavni.ApiService.Utils.Extensions;

public static class DateTimeExtensions
{
    public static int GetAge(this DateTime birthDate)
    {
        DateTime n = DateTime.Now;
        int age = n.Year - birthDate.Year;

        if (n.Month < birthDate.Month || (n.Month == birthDate.Month && n.Day < birthDate.Day))
            age--;

        return age;
    }
}
