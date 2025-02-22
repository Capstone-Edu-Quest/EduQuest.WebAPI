using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.Helper;

public class CodeGenerator
{
    private static Random random = new Random();

    public static string GenerateRandomCouponCode()
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string digits = "0123456789";
        const int codeLength = 12;

        StringBuilder codeBuilder = new StringBuilder(codeLength);

        for (int i = 0; i < codeLength; i++)
        {
            // random between 0 and 1, if random.next = 0, random append letter, esle random append digit
            if (random.Next(2) == 0) // 50% probability
            {
                // letter
                codeBuilder.Append(letters[random.Next(letters.Length)]);
            }
            else
            {
                // digit
                codeBuilder.Append(digits[random.Next(digits.Length)]);
            }
        }

        return codeBuilder.ToString();
    }
}
