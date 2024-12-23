using System.Globalization;

namespace Application.Tools;

public static class Date
{
    public static string ToShamsi(this DateTime date)
    {
        var pc = new PersianCalendar();
        return pc.GetYear(date) + "/" +
               pc.GetMonth(date).ToString("00") + "/" +
               pc.GetDayOfMonth(date).ToString("00");
    }
    public static DateTime ToMiladi(this DateTime shamsiDate)
    {
        var pc = new PersianCalendar();
        return new DateTime(
            pc.GetYear(shamsiDate),
            pc.GetMonth(shamsiDate),
            pc.GetDayOfMonth(shamsiDate),
            new GregorianCalendar()
        );
    }

    public static DateTime ToMiladiString(this string shamsiDate)
    {
        var pc = new PersianCalendar();
        try
        {
            var parts = shamsiDate.Split('/');
            if (parts.Length != 3)
                throw new FormatException("Invalid Shamsi date format. Expected format: yyyy/MM/dd");

            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);

            return pc.ToDateTime(year, month, day, 0, 0, 0, 0);
        }
        catch (Exception ex)
        {
            return new DateTime();
        }
    }
}