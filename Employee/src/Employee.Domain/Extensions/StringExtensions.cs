namespace Employee.Domain.Extensions;

public static class StringExtensions
{
    public static bool IsBase64(this string base64)
    {
        if (string.IsNullOrEmpty(base64) || base64.Length % 4 != 0)
            return false;

        try
        {
            Convert.FromBase64String(base64);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}