using System.Security.Cryptography;
using System.Text;

namespace MovieRentalAdminApi.CrossCutting
{
    public static class Encript
    {
        public static string ComputeSha256Hash(this string inputString)
        {
            using (var sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(inputString));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
