using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MobileShop.Service
{
    public interface IEncryptionService
    {
        string HashMD5(string input);
    }

    public class EncryptionService : IEncryptionService
    {
        public string HashMD5(string input)
        {
            var hash = MD5.HashData(Encoding.UTF8.GetBytes(input));
            var hashSb = new StringBuilder();
            foreach (var b in hash)
            {
                hashSb.Append(b.ToString("X2"));
            }
            return hashSb.ToString();
        }
    }
}
