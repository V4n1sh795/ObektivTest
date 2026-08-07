namespace Utility;
using System.IO.Hashing;
using System.Text;
public static class Validator
{
    private static readonly System.Text.RegularExpressions.Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        System.Text.RegularExpressions.RegexOptions.Compiled |
        System.Text.RegularExpressions.RegexOptions.IgnoreCase
    );

    public static bool IsEmailValid(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        if (email.Length > 254)
            return false;

        return EmailRegex.IsMatch(email);
    }
    public static bool IsValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            return false;

        // Только HTTPS
        if (!string.Equals(uri.Scheme, "https", StringComparison.OrdinalIgnoreCase))
            return false;

        // Хост должен быть ровно prinzip.su (или www.prinzip.su при желании)
        if (!string.Equals(uri.Host, "prinzip.su", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(uri.Host, "www.prinzip.su", StringComparison.OrdinalIgnoreCase))
            return false;

        // Путь должен начинаться с /flats/
        return uri.AbsolutePath.StartsWith("/flats/", StringComparison.OrdinalIgnoreCase);
    }
}
public static class ParseValues
{
    public static bool Price(string text, out int result)
    {
        result = 0;

        if (string.IsNullOrWhiteSpace(text))
            return false;

        string digits = new string(text.Where(char.IsDigit).ToArray());

        if (digits.Length == 0 || digits.Length > 10)
            return false;

        return int.TryParse(digits, out result);
    }
}
public static class Hash
{
    public static ulong GetXxHash64(string input)
    {
        byte[] data = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = XxHash64.Hash(data);
        return BitConverter.ToUInt32(hashBytes, 0);
    }
}
