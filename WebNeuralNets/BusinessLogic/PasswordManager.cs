using System;
using System.Security.Cryptography;
using System.Text;

namespace WebNeuralNets.BusinessLogic
{
    public static class PasswordManager
    {
        public static string GenerateSaltedHash(string password, string userName)
        {
            byte[] source = Encoding.Unicode.GetBytes(password);
            byte[] salt = Encoding.Unicode.GetBytes(userName);
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes = new byte[source.Length + salt.Length];

            for (int i = 0; i < source.Length; i++)
            {
                plainTextWithSaltBytes[i] = source[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[source.Length + i] = salt[i];
            }

            return Convert.ToBase64String(algorithm.ComputeHash(plainTextWithSaltBytes));
        }
    }
}
