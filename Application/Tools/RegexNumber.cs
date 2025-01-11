using System.Text.RegularExpressions;

namespace Application.Tools;

public static class RegexNumber
{
    public static bool IsValidMobileNumber(this string input)
    {
        const string pattern = @"^09[0|1|2|3][0-9]{8}$";
        var reg = new Regex(pattern);
        return reg.IsMatch(input);
    }
}