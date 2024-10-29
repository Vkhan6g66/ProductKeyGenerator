using System;
using System.Security.Cryptography;
using System.Text;

public class ProductKeyGenerator
{
    private static string secretKey = "your_secret_key_here"; // Replace with your secret key

    public static string GenerateProductKey()
    {
        // Generate a GUID as the base for the product key
        Guid guid = Guid.NewGuid();
        string serialKey = guid.ToString("N").Substring(0, 15); // Take the first 15 characters

        // Generate HMAC-SHA256 hash of the serial key using the secret key
        byte[] hmacBytes = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)).ComputeHash(Encoding.UTF8.GetBytes(serialKey));

        // Convert the HMAC bytes to a hexadecimal string and take the first 15 characters
        string hmacKey = BitConverter.ToString(hmacBytes).Replace("-", "").Substring(0, 15);

        // Combine the serial key and HMAC key to form the full product key
        return serialKey + hmacKey;
    }

    public static bool ValidateProductKey(string productKey)
    {
        if (productKey.Length != 30)
            return false;

        string serialKey = productKey.Substring(0, 15);
        string hmacKey = productKey.Substring(15);

        byte[] expectedHmacBytes = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)).ComputeHash(Encoding.UTF8.GetBytes(serialKey));
        string expectedHmacKey = BitConverter.ToString(expectedHmacBytes).Replace("-", "").Substring(0, 15);

        return hmacKey.Equals(expectedHmacKey, StringComparison.OrdinalIgnoreCase);
    }
}

class Program
{
    static void Main()
    {
        string productKey = ProductKeyGenerator.GenerateProductKey();
        Console.WriteLine("Generated Product Key: " + productKey);

        bool isValid = ProductKeyGenerator.ValidateProductKey(productKey);
        Console.WriteLine("Is Product Key Valid? " + isValid);
    }
}
