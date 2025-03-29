using System.Security.Cryptography;
using System.Text;

namespace EduQuest_Application.Helper
{
    public class AuthenHelper
    {
        public static string GenerateRefreshToken(string id)
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            // Encode deviceId thành Base64 trước
            string encodedDeviceId = Convert.ToBase64String(Encoding.UTF8.GetBytes(id));
            string randomToken = Convert.ToBase64String(randomNumber);

            // Kết hợp và encode lại một lần nữa
            string combinedToken = $"{encodedDeviceId}.{randomToken}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(combinedToken));
        }


        public static string ExtractIdFromRefreshToken(string refreshToken)
        {
            string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(refreshToken));
            string[] parts = decodedToken.Split('.');
            return Encoding.UTF8.GetString(Convert.FromBase64String(parts[0]));

        }

        public static string GenerateOTP()
        {
            var randomNumber = new byte[6];
            RandomNumberGenerator.Fill(randomNumber);
            var otp = BitConverter.ToUInt32(randomNumber, 0) % 1000000;
            return otp.ToString("D6");
        }

        public static string GenerateRandomPassword(int length = 16)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_=+[]{}|;:,.<>?";
            char[] password = new char[length];

            for (int i = 0; i < length; i++)
                password[i] = chars[RandomNumberGenerator.GetInt32(chars.Length)];

            return new string(password);
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, string passwordHashBase64, string passwordSaltBase64)
        {
            byte[] passwordHash = Convert.FromBase64String(passwordHashBase64);
            byte[] passwordSalt = Convert.FromBase64String(passwordSaltBase64);

            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }




    }
}
