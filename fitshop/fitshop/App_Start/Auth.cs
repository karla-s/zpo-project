using fitshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace fitshop.App_Start
{
    public static class Auth
    {
        public static string GenerateToken()
        {
            byte[] random = new Byte[192];
            string token = "";
            //RNGCryptoServiceProvider is an implementation of a random number generator.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(random);

            using (SHA512Managed SHA512 = new SHA512Managed())
                token = _getStringFromHash(SHA512.ComputeHash(random));

            return token;
        }

        public static string HashPassword(string password)
        {
            string hashedPassword = "";
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            using (SHA512Managed SHA512 = new SHA512Managed())
                hashedPassword = _getStringFromHash(SHA512.ComputeHash(passwordBytes));

            return hashedPassword;
        }

        public static user GetUserFromToken(string token)
        {
            fitshopEntities db = new fitshopEntities();

            List<token> tokenFromDB = db.token.Where(x => x.tokenValue == token).ToList();

            if (tokenFromDB.Count != 1)
                return null;

            return tokenFromDB.First().user;           
        }

        private static string _getStringFromHash(byte[] hash)
        {
            StringBuilder password = new StringBuilder();
            foreach (var item in hash)
                password.Append(item.ToString("X2"));

            return password.ToString();
        }
    }
}